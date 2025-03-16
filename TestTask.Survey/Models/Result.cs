using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Survey.Models
{
    [Table("Results")]
    public class Result
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("interview_id")]
        public int InterviewId { get; set; }
        [Column("question_id")]
        public int QuestionId { get; set; }
        [Column("answer_id")]
        public int? AnswerId { get; set; }
        [Column("text_response")]
        public string? TextResponse { get; set; } // Текстовый вариант ответа
        [Column("response_time")]
        public DateTime ResponseTime { get; set; } // Время ответа

        public Interview? Interviews { get; set; }
        public Question? Questions { get; set; }
        public Answer? Answers { get; set; }
    }
}
