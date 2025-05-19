using System.Net.Http.Headers;
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
                if(apiRequest.ContentType == ContentType.MultipartFormData)
                {

                    httpRequestMessage.Headers.Add("Accept", "*");
                }else
                {

                httpRequestMessage.Headers.Add("Accept", "application/json");
                }
                httpRequestMessage.RequestUri = new Uri(apiRequest.Url);
             
                if (apiRequest.ContentType == ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var item in apiRequest.Data.GetType().GetProperties())
                    {
                        // Get the value of the current property
                        var value = item.GetValue(apiRequest.Data);
                        if (value is FormFile)
                        {
                            //the returned value is of type object even if it's formfile
                            var file = (FormFile)value;
                            if (file != null)
                            {
                                // Add the file stream to the multipart content with the property name and file name
                                content.Add(new StreamContent(file.OpenReadStream()), item.Name, file.FileName);
                            }
                        }
                        else
                        {
                            // If it's not a file, convert it to a string and add it as regular form data
                            // If the value is null, use an empty string
                            content.Add(new StringContent(value == null ? "" : value.ToString()), item.Name);
                        }
                    }
                    // Set the built multipart content as the body of the HTTP request
                    httpRequestMessage.Content = content;
                }


                if (apiRequest.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }

                // Set request method based on ApiType
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
                //Sets authorisation header only when Token.length > 0 i.e. Token isnt empty.
                if (!string.IsNullOrEmpty(apiRequest.token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.token);
                }
                HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
                var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
                try
                {
                    APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (apiResponse.HttpStatusCode == System.Net.HttpStatusCode.BadRequest
                        || apiResponse.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        apiResponse.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
                        apiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(apiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception e)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }
                var ApiReturnedData = JsonConvert.DeserializeObject<T>(apiContent);

                return ApiReturnedData;
            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    Errors = new List<string> { e.Message },
                    IsSuccess = false,
                };

                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dto));
            }
        }

    }
}
