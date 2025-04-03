using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.IdentityModel.Tokens;

namespace Book_Your_Hotel.Repositary
{
    public class UserRepositary : IUser
    {
        private readonly ApplicationDbContext _Db;
        private string SecretKey;
        public UserRepositary(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _Db = applicationDbContext;
            SecretKey = configuration.GetValue<string>("JwtSettings:SecretKey");
        }

        public bool IsUniqueUser(string user)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            var user = _Db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == request.UserName.ToLower() && u.Password == request.Password);
            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var ResultedToken = tokenHandler.CreateToken(tokenDesc);
            LoginResponseDTO dto = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(ResultedToken),
                User = user
            };
            return Task.FromResult(dto);
        }

        public Task<LocalUser> Register(LocalUser user)
        {
            throw new NotImplementedException();
        }
    }
}
