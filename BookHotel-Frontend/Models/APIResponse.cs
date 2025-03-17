using System.Net;

namespace BookHotel_Frontend.Models
{
    public class APIResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public bool IsSuccess { get; set; } 
        public List<string> Errors { get; set; } 

        public object Result { get; set; }
    }
}
