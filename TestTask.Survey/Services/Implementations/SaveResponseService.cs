using Microsoft.EntityFrameworkCore;
using SurveyDb;
using System.ComponentModel.DataAnnotations;
using TestTask.Survey.Data.Dtos;
using TestTask.Survey.Data.Models;
using TestTask.Survey.Services.Interfaces;

namespace TestTask.Survey.Services.Implementations
{
    public class SaveResponseService : ISaveResponseService
    {
        private readonly SurveyDbContext _context;

        public int ErrorCode { get; private set; }

        // Внедрение контекста через конструктор
        public SaveResponseService(SurveyDbContext context)
        {
            _context = context;
        }

        public async Task<NextQuestionResponseDto> SaveResponseAsync(SaveAnswerRequestDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Валидация и получение интервью
                var interview = await ValidateAndGetInterviewAsync(request.InterviewId);
                var currentQuestion = await GetQuestionWithAnswersAsync(request.CurrentQuestionId, interview.Id);

                // Получаем соответствие порядка ответов и их ID
                var answerOrderIdMap = currentQuestion.Answers
                    .ToDictionary(a => a.DisplayOrder, a => a.Id);

                // Валидация и преобразование DisplayOrder в AnswerIds
                var answerIds = ValidateAndConvertOrdersToIds(
                    request.SelectedAnswerOrders,
                    answerOrderIdMap);

                // Сохранение ответов
                await SaveAnswersAsync(request, answerIds);

                // Поиск следующего вопроса
                var nextQuestion = await FindNextQuestionAsync(interview, currentQuestion);

                // Обновление статуса интервью
                UpdateInterviewStatus(interview, nextQuestion);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new NextQuestionResponseDto(nextQuestion?.Id, nextQuestion == null);
            }
            catch (ValidationException ex)
            {
                await transaction.RollbackAsync();
                throw new ValidationException(ex.Message); // Обработка в middleware
            }
        }

        private void UpdateInterviewStatus(Interview interview, Question nextQuestion)
        {
            if (nextQuestion == null)
            {
                interview.EndTime = DateTime.UtcNow;
                interview.Status = "completed";
            }
        }

        private async Task<Question?> FindNextQuestionAsync(Interview interview, Question currentQuestion)
        {
            var nextQuestion = await _context.Questions
                    .Where(q => q.SurveyId == interview.SurveyId)
                    .Where(q => q.DisplayOrder > currentQuestion.DisplayOrder)
                    .OrderBy(q => q.DisplayOrder)
                    .FirstOrDefaultAsync();

            return nextQuestion;
        }

        private async Task<Interview> ValidateAndGetInterviewAsync(int interviewId)
        {
            var interview = await _context.Interviews
                .FirstOrDefaultAsync(i => i.Id == interviewId);

            if (interview == null)
            {
                ErrorCode = 404;
                throw new ValidationException("Interview not found");
            }
            if (interview.Status == "completed")
            {
                ErrorCode = 400;
                throw new ValidationException("Interview already completed");
            }

            return interview;
        }

        // Новые вспомогательные методы
        private List<int>? ValidateAndConvertOrdersToIds(
            List<int>? selectedOrders,
            Dictionary<int, int> orderIdMap)
        {
            if (selectedOrders == null)
            {
                return null;
            }

            var invalidOrders = selectedOrders.Except(orderIdMap.Keys).ToList();
            if (invalidOrders.Any())
            {
                ErrorCode = 400;
                throw new ValidationException($"Неверные порядковые номера ответов: {string.Join(", ", invalidOrders)}");
            }

            return selectedOrders.Select(order => orderIdMap[order]).ToList();
        }

        private async Task<Question> GetQuestionWithAnswersAsync(int questionId, int interviewId)
        {
            var surveyId = await _context.Interviews
                .Where(i => i.Id == interviewId)
                .Select(i => i.SurveyId)
                .FirstOrDefaultAsync();

            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if(question == null)
            {
                ErrorCode = 404;
                throw new ValidationException("Question not found");
            }

            if(question.SurveyId != surveyId)
            {
                ErrorCode = 400;
                throw new ValidationException("Question does not belong to the interview");
            }

            return question;
        }

        private async Task SaveAnswersAsync(SaveAnswerRequestDto request, List<int> answerIds)
        {
            var responseTime = DateTime.UtcNow;

            if (answerIds != null)
            {
                foreach (var answerId in answerIds)
                {
                    await _context.Results.AddAsync(new Result
                    {
                        InterviewId = request.InterviewId,
                        QuestionId = request.CurrentQuestionId,
                        AnswerId = answerId,
                        ResponseTime = responseTime
                    });
                }
            }

            if (!string.IsNullOrEmpty(request.TextResponse))
            {
                await _context.Results.AddAsync(new Result
                {
                    InterviewId = request.InterviewId,
                    QuestionId = request.CurrentQuestionId,
                    TextResponse = request.TextResponse,
                    ResponseTime = responseTime
                });
            }
        }
    }
}
