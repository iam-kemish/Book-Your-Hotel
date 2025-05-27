using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Azure;
using Book_Your_Hotel.Models;
using BookHotel_Frontend.Models;
using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static BookHotel_Utilities.StaticDetails;
using APIResponse = BookHotel_Frontend.Models.APIResponse;

namespace BookHotel_Frontend.Services
{
    public class BaseServiceClass : IBaseService
    {
        public APIResponse APIResponse { get; set; }
        public IHttpClientFactory HttpClient { get; set; }
        private readonly IToken _IToken;
        private readonly HttpContextAccessor _HttpContextAccessor;
        private string HotelUrl;
        public BaseServiceClass(IHttpClientFactory httpClientFactory, IToken iToken, IConfiguration configuration, HttpContextAccessor httpContextAccessor)
        {
            this.APIResponse = new APIResponse();
            this.HttpClient = httpClientFactory;
            _IToken = iToken;
            HotelUrl = configuration.GetValue<string>("ServiceUrls:BookHotelApi");
            _HttpContextAccessor = httpContextAccessor;
        }
        public async Task<T> SendAsync<T>(ApiRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = HttpClient.CreateClient("HotelsApi");
                var MessageFactory = () =>
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    if (apiRequest.ContentType == ContentType.MultipartFormData)
                    {

                        httpRequestMessage.Headers.Add("Accept", "*/*");
                    }
                    else
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
                    else
                    {
                        if (apiRequest.Data != null)
                        {
                            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                        }
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
                    return httpRequestMessage;
                };
                //Sets authorisation header only when Token.length > 0 i.e. Token isnt empty.
                if (!string.IsNullOrEmpty(apiRequest.token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.token);
                }
                HttpResponseMessage httpResponseMessage = await SendWithRefreshTokenAsync(client, MessageFactory, withBearer);
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

        private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(HttpClient httpClient,
            Func<HttpRequestMessage> httpRequestMessageFactory, bool withBearer = true)
        {

            if (!withBearer)
            {
                return await httpClient.SendAsync(httpRequestMessageFactory());
            }
            else
            {
                LoginResponseDTO loginResponse = _IToken.GetToken();
                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
                }

                try
                {
                    var response = await httpClient.SendAsync(httpRequestMessageFactory());
                    if (response.IsSuccessStatusCode)
                        return response;

                    // IF this fails then we can pass refresh token!
                    if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //GENERATE NEW Token from Refresh token / Sign in with that new token and then retry
                        await InvokeRefreshTokenEndpoint(httpClient, loginResponse.Token, loginResponse.RefreshToken);
                        response = await httpClient.SendAsync(httpRequestMessageFactory());
                        return response;
                    }
                    return response;

                }
              
                catch (HttpRequestException httpRequestException)
                {
                    if (httpRequestException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // refresh token and retry the request
                        await InvokeRefreshTokenEndpoint(httpClient, loginResponse.Token, loginResponse.RefreshToken);
                        return await httpClient.SendAsync(httpRequestMessageFactory());
                    }
                    throw;
                }



            }


        }



        private async Task InvokeRefreshTokenEndpoint(HttpClient httpClient, string AccessToken, string RefreshToken)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Headers.Add("Accept", "*/*");
            httpRequestMessage.RequestUri = new Uri($"{HotelUrl}/api/Users/RefreshToken");
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(new LoginResponseDTO
            {
                Token = AccessToken,
                RefreshToken = RefreshToken
            }), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var ApiResponse = JsonConvert.DeserializeObject<APIResponse>(content);
            if(!APIResponse.IsSuccess)
            {
                await _HttpContextAccessor.HttpContext.SignOutAsync();
                _IToken.ClearToken();
            
            }else
            {
                var tokenDataStr = JsonConvert.SerializeObject(ApiResponse.Result);
                var tokenResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(tokenDataStr);

                if(tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.Token))
                {
                    //new method to sign in with the token that we received
                    var handler = new JwtSecurityTokenHandler();
                    var jwtExtraction = handler.ReadJwtToken(tokenResponse.Token);
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwtExtraction.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                    identity.AddClaim(new Claim(ClaimTypes.Role, jwtExtraction.Claims.FirstOrDefault(u => u.Type == "role").Value));
                    var Principal = new ClaimsPrincipal(identity);
                    await _HttpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal);
                    _IToken.SetToken(tokenResponse);

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);
                }
            }
        }
    }
}
