using System;

namespace AuthLab2_RyanPinkney.Models
{
    // throws error
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
