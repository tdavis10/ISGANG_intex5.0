using System;
namespace AuthLab2_RyanPinkney.Models.ViewModels
{
    public class PageInfo
    {
        // pass this info on to the PaginationTagHelper
        public int iTotalProjectsNum { get; set; }

        public int iProjectsPerPage { get; set; }

        public int iCurrentPage { get; set; }

        // Figure out how many pages we need
        public int iTotalPages => (int)Math.Ceiling(((double)iTotalProjectsNum / iProjectsPerPage)); // This is how we cast



    }
}
