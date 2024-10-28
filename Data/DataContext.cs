using Microsoft.EntityFrameworkCore;
using CryptoDashboard.Api.Models;

namespace CryptoDashboard.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}