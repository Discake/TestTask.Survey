namespace TestTask.Survey.Data.Dtos
{
    public record QuestionDto(int Id, string Text, IEnumerable<AnswerDto> Answers);
}
