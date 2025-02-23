using Microsoft.EntityFrameworkCore;
using TransactionService.Core.Domain.Entities;

namespace TransactionService.Infrastructure.Repositories
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
