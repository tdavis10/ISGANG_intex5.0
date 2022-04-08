using System;
using System.Linq;
// Authors Jacob Poor, Ryan Pinkney, Kevin Gutierrez, Tanner Davis
namespace AuthLab2_RyanPinkney.Models
{
    public class EFCrashRepository : ICrashRepository
    {
        private AccidentDbContext context { get; set; }

        // initialize repository
        public EFCrashRepository(AccidentDbContext temp)
        {
            context = temp;
        }

        public IQueryable<Accident> Accidents => context.Accidents;

        // method to add an accident record
        public void AddAccident(Accident a)
        {
            context.Add(a);
            context.SaveChanges();
        }

        // method to edit an accident record
        public void EditAccident(Accident a)
        {
            context.Update(a);
            context.SaveChanges();
        }

        // method to delete an accident record
        public void DeleteAccident(Accident a)
        {
            context.Remove(a);
            context.SaveChanges();
        }

    }
}
