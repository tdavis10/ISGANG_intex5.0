using System;
using Microsoft.EntityFrameworkCore;
// Authors Jacob Poor, Ryan Pinkney, Kevin Gutierrez, Tanner Davis
namespace AuthLab2_RyanPinkney.Models
{
    public class AccidentDbContext : DbContext
    {
        public AccidentDbContext(DbContextOptions<AccidentDbContext> blah) : base(blah)
        {

        }

        // create an instance of the Accident object containing all accidents
        public DbSet<Accident> Accidents { get; set; }
    



    }
}
