using Microsoft.EntityFrameworkCore;
using TestTask.Survey.Data.Models;

namespace SurveyDb
{
    public class SurveyDbContext : DbContext
    {
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<Survey> Surveys => Set<Survey>();
        public DbSet<Result> Results => Set<Result>();
        public DbSet<Interview> Interviews => Set<Interview>();

        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options) { }
    }
}
