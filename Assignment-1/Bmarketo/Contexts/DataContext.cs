using Bmarketo.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ProductEntity> Products { get; set; }
    }
}
