using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace TestTask.Survey.Models
{
    [Table("Interviews")]
    public class Interview
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("survey_id")]
        public int SurveyId { get; set; }
        [Column("start_time")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow; // Время начала сессии
        [Column("end_time")]
        public DateTime? EndTime { get; set; } // Время завершения сессии
        [Column("status")]
        public string? Status { get; set; } // Статус сессии (in_progress, completed, abandoned)

        // Навигационные свойства
        public Survey? Survey { get; set; }
        public ICollection<Result>? Results { get; set; }
    }
}
