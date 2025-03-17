using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Survey.Data.Models
{
    [Table("answers")]
    public class Answer
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("question_id")]
        public int QuestionId { get; set; }
        [Column("answer_text")]
        public string? AnswerText { get; set; }
        [Column("display_order")]
        public int DisplayOrder { get; set; } // Порядковый номер ответа в вопросе
    }
}
