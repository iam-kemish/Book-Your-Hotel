using BookHotel_Frontend.Models.DTOs;
using BookHotel_Frontend.Services.IServices;
using BookHotel_Utilities;

namespace BookHotel_Frontend.Services
{
    public class TokenClass : IToken
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenClass(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetails.AccessToken);
        }

        public LoginResponseDTO? GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(StaticDetails.AccessToken, out string AccessToken);
             
                    LoginResponseDTO loginResponseDTO = new()
                    {
                        Token = AccessToken
                    };
                
                    return hasAccessToken ? loginResponseDTO : null;
                }
            
            catch (Exception ex) {
                return null;
            }

        }

        public void SetToken(LoginResponseDTO loginResponseDTO)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(StaticDetails.AccessToken, loginResponseDTO.Token, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });
        }
    }
}
