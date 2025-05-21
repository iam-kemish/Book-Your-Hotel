using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Book_Your_Hotel.Repositary
{
    public class UserClass : IUser
    {
        private readonly ApplicationDbContext _Db;
        private string SecretKey;
        private readonly IMapper _Imapper;
        private readonly UserManager<AppUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public UserClass(ApplicationDbContext applicationDbContext, IConfiguration configuration, IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _Db = applicationDbContext;
            SecretKey = configuration.GetValue<string>("JwtSettings:SecretKey");
            _Imapper = mapper;
            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        public bool IsUniqueUser(string user)
        {
            var checkUser = _Db.AppUsers.FirstOrDefault(u => u.UserName == user);
            if (checkUser == null)
            {
                return true;
            }
            return false;

        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            var user = await _Db.AppUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == request.UserName.ToLower());

            bool isValid = await _UserManager.CheckPasswordAsync(user, request.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponseDTO
                {
                    Token = "",
                 
                };
            }
            var roles = await _UserManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var ResultedToken = tokenHandler.CreateToken(tokenDesc);
            LoginResponseDTO dto = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(ResultedToken),
              
            };
            return dto;
        }
        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            //var newUser = _Imapper.Map<AppUser>(registerationRequestDTO);

            AppUser newUser = new()
            {
                UserName = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
                Email = registerationRequestDTO.UserName,
                Name = registerationRequestDTO.Name
            };
            var result = await _UserManager.CreateAsync(newUser, registerationRequestDTO.Password);

            if (result.Succeeded)
            {
                //Testing purpose(Test completed).
                if (!_RoleManager.RoleExistsAsync(registerationRequestDTO.Role).GetAwaiter().GetResult())
                {
                    await _RoleManager.CreateAsync(new IdentityRole(registerationRequestDTO.Role));
                   
                }

                await _UserManager.AddToRoleAsync(newUser, registerationRequestDTO.Role);
                var userToReturn = _Db.AppUsers.FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);
                var userDto = _Imapper.Map<UserDTO>(userToReturn);
                return userDto;
            }

            return null; // or throw exception / return error DTO
        }



    }
}

