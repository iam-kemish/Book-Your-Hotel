using System.Net;


namespace Book_Your_Hotel.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            Errors = new List<string>();

        }

        public HttpStatusCode HttpStatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; }
        public object Result { get; set; }
    }

}
