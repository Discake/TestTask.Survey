namespace TestTask.Survey.Data.Dtos
{
    public record SaveAnswerRequestDto(
        int InterviewId,
        int CurrentQuestionId,
        List<int>? SelectedAnswerOrders,
        string? TextResponse);
}
