using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuestionBankService.Data;

namespace QuestionBankService.Factories
{
    public class QuestionDbContextFactory 
        : IDesignTimeDbContextFactory<QuestionDbContext>
    {
        public QuestionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuestionDbContext>();

           optionsBuilder.UseSqlServer(
    "Server=SHIVAPRASAD\\MSSQL2014;Database=QuestionBankDB;Trusted_Connection=True;TrustServerCertificate=True"
);


            return new QuestionDbContext(optionsBuilder.Options);
        }
    }
}
