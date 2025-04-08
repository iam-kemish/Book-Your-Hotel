using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.IdentityModel.Tokens;

namespace Book_Your_Hotel.Repositary
{
    public class UserClass : IUser
    {
        private readonly ApplicationDbContext _Db;
        private string SecretKey;
        private readonly IMapper _Imapper;
        public UserClass(ApplicationDbContext applicationDbContext, IConfiguration configuration, IMapper mapper)
        {
            _Db = applicationDbContext;
            SecretKey = configuration.GetValue<string>("JwtSettings:SecretKey");
            _Imapper = mapper;
        }

        public bool IsUniqueUser(string user)
        {
            var checkUser = _Db.LocalUsers.FirstOrDefault(u=>u.UserName ==  user);
            if (checkUser == null) {
                return true;
            }
            return false;

        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
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
            return dto;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            // var newUser = _Imapper.Map<LocalUser>(registerationRequestDTO);
            // _Db.LocalUsers.Add(newUser);
            //await  _Db.SaveChangesAsync();
            // newUser.Password = "";
            // return newUser;
            LocalUser user = new()
            {
                UserName = registerationRequestDTO.UserName,
                Password = registerationRequestDTO.Password,
                Name = registerationRequestDTO.Name,
                Role = registerationRequestDTO.Role
            };

            _Db.LocalUsers.Add(user);
            await _Db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
