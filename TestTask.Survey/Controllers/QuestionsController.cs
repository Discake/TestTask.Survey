using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyDb;
using TestTask.Survey.Data.Dtos;

namespace TestTask.Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly SurveyDbContext _context;

        public QuestionsController(SurveyDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(QuestionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestion(int id)
        {
            var question = await _context.Questions
                .AsNoTracking()
                .Include(q => q.Answers)
                .OrderBy(a => a.DisplayOrder)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            var result = new QuestionDto(
                Id: question.Id,
                Text: question.QuestionText,
                Answers: question.Answers.Select(a =>
                    new AnswerDto(a.Id, a.AnswerText, a.DisplayOrder))
            );

            return Ok(result);
        }
    }
}
