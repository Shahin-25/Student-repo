using Microsoft.EntityFrameworkCore;
using user.Model;

namespace user.Data
{
    public class AppDbContext:DbContext
    {
        
            public DbSet<Item1> Itemslist{ get; set; }
            // Add other DbSets as needed

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }
        

    }
}
