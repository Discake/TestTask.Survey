using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Survey.Models
{
    [Table("Surveys")]
    public class Survey
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string? Title { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("status")]
        public string? Status { get; set; } // Статус анкеты (active/inactive)

        public ICollection<Question>? Questions { get; set; }
        public ICollection<Interview>? Interviews { get; set; }
    }
}
