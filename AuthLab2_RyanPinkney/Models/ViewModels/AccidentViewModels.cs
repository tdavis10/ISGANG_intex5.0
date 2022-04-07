using System;
using System.Linq;

namespace AuthLab2_RyanPinkney.Models.ViewModels
{
    public class AccidentViewModels
    {
        // make it queryable
        public IQueryable<Accident> Accidents { get; set; }

        // create an instance of the pageinfo class
        public PageInfo PageInfo { get; set; }

    }
}
