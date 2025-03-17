using System.Text;
using Book_Your_Hotel.Models;
using BookHotel_Frontend.Models;
using BookHotel_Frontend.Services.IServices;
using Newtonsoft.Json;
using static BookHotel_Utilities.StaticDetails;
using APIResponse = BookHotel_Frontend.Models.APIResponse;

namespace BookHotel_Frontend.Services
{
    public class BaseServiceClass : IBaseService
    {
        public APIResponse APIResponse { get; set; }
        public IHttpClientFactory HttpClient { get; set; }
        public BaseServiceClass(IHttpClientFactory httpClientFactory)
        {
            this.APIResponse = new APIResponse();
            this.HttpClient = httpClientFactory; 
        }
        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClient.CreateClient("HotelsApi");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add("Accept", "application/json");
                httpRequestMessage.RequestUri = new Uri(apiRequest.Url);
                if(apiRequest.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");

                }
                switch (apiRequest.ApiType)
                {
                    case ApiType.PUT:
                        httpRequestMessage.Method = HttpMethod.Put;
                        break;
                    case ApiType.POST:
                        httpRequestMessage.Method = HttpMethod.Post;
                        break;
                    case ApiType.DELETE:
                        httpRequestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
                var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
                var ApiReturnedData = JsonConvert.DeserializeObject<T>(apiContent);
                return ApiReturnedData;

            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    Errors = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false,
                };
            var res = JsonConvert.SerializeObject(dto);
            var returnObj = JsonConvert.DeserializeObject<T>(res);
            return returnObj;
            }
        }
    }
}
