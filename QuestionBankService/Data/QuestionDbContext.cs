using Microsoft.EntityFrameworkCore;
using QuestionBankService.Models;

namespace QuestionBankService.Data
{
    public class QuestionDbContext : DbContext
    {
        public QuestionDbContext(DbContextOptions<QuestionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
    }
}
