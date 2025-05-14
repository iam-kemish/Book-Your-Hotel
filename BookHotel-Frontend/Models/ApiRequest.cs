using static BookHotel_Utilities.StaticDetails;

namespace BookHotel_Frontend.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }

        public string token { get; set; }   

        public ContentType ContentType { get; set; } = ContentType.json;
    }
}  