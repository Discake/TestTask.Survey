using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Survey.Data.Models
{
    [Table("questions")]
    public class Question
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("survey_id")]
        public int SurveyId { get; set; }
        [Column("question_text")]
        public string? QuestionText { get; set; }
        [Column("display_order")]
        public int DisplayOrder { get; set; } // Порядковый номер вопроса в опросе

        public ICollection<Answer>? Answers { get; set; }
    }
}
