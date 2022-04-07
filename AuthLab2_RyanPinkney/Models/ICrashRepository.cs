using System;
using System.Linq;

namespace AuthLab2_RyanPinkney.Models
{
    public interface ICrashRepository
    {
        // make it queryable
        public IQueryable<Accident> Accidents { get; }

        // methods for CRUD
        public void AddAccident(Accident a);
        public void EditAccident(Accident a);
        public void DeleteAccident(Accident a);




    }
}
