using TestTask.Survey.Data.Dtos;

namespace TestTask.Survey.Services.Interfaces
{
    public interface ISaveResponseService
    {
        public int ErrorCode { get; }
        public Task<NextQuestionResponseDto> SaveResponseAsync(SaveAnswerRequestDto request);
    }
}
