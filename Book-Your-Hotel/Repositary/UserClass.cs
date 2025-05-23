using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Book_Your_Hotel.Database;
using Book_Your_Hotel.Migrations;
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
                    Token = ""
                   
                };
            }
            var JwtTokenId = $"JTI-{Guid.NewGuid()}";
             var resultedToken = await GetAccessToken(user, JwtTokenId);
            var ResultedRefreshToken = await CreateRefreshToken(user.Id, JwtTokenId);
            LoginResponseDTO dto = new LoginResponseDTO()
            {
                Token = resultedToken,
                 RefreshToken = ResultedRefreshToken

            };
            return dto;
        }

     //
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

        ////// token generating/////////
        ///

        public async Task<string> GetAccessToken(AppUser user, string jwtTokenId)
        {
            var roles = await _UserManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var ResultedToken = tokenHandler.CreateToken(tokenDesc);
            var TokenString = tokenHandler.WriteToken(ResultedToken);
            return TokenString;
        }

        public async Task<LoginResponseDTO> RefreshAccessToken(LoginResponseDTO request)
        {
           //This method creates new accesstoken using refresh token//
           //first we find an exisiting refresh token
           var existingRefreshToken =await  _Db.RefreshTokens.FirstOrDefaultAsync(u=>u.Refresh_Token ==  request.RefreshToken);
            if(existingRefreshToken == null)
            {
                return new LoginResponseDTO();
            }
            //now we compare user data from existing  token and refresh token if it didnt matched, it will be considered security alert.
            var exisitingTokenData = GetAccessTokenData(request.Token);
            if (!exisitingTokenData.isSuccessful || existingRefreshToken.JwtTokenId != exisitingTokenData.tokenId || existingRefreshToken.UserId != exisitingTokenData.tokenId)
            {
                //we mark exisiting refresh token as invalid
                existingRefreshToken.IsValid = false;
                _Db.SaveChanges();
            }
            //if invalid mark it invalid and return empty
            if(existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                existingRefreshToken.IsValid = false;
                _Db.SaveChanges();
            }
            //replace old refreshtoken with new one//
            var newRefreshToken = await CreateRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            //mark exisiting refresh token invalid
            existingRefreshToken.IsValid = false;
            _Db.SaveChanges();
            //lets now create a new accessToken(Token)
            var appUser = await _Db.Users.FirstOrDefaultAsync(u => u.Id == existingRefreshToken.UserId);
            if(appUser == null)
            {
                return new LoginResponseDTO();
            }
            //we are using exisitng existingRefreshToken.JwtTokenId, so that after the accesstoken expires, we use to same user already logggedin information to create a new accesstoken
            var newAccessToken = await GetAccessToken(appUser, existingRefreshToken.JwtTokenId);
            return new LoginResponseDTO
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };


        }

        private async Task<string>  CreateRefreshToken(string userId, string TokenId)
        {
            RefreshToken refreshToken = new()
            {
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                JwtTokenId = TokenId,
                Refresh_Token = Guid.NewGuid() + "_" + Guid.NewGuid(),
                IsValid = true
            };
            await _Db.RefreshTokens.AddAsync(refreshToken);
            await _Db.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }
        private (bool isSuccessful, string userId, string tokenId) GetAccessTokenData(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ReadJwtToken(accessToken);

                var TokenId = principal.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var userId = principal.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

                return (true, userId, TokenId);
            }
            catch
            {
                return (false, null, null);
            }
        }
    }
}

