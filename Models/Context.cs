using Microsoft.EntityFrameworkCore;

namespace ProsNCats.Models 
{
    public class ProsNCatsContext: DbContext
    {
        // base(options) calls the parent class's constructor
        public ProsNCatsContext(DbContextOptions<ProsNCatsContext> options): base(options) {}
        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<Association> Associations {get; set;}
    }
}