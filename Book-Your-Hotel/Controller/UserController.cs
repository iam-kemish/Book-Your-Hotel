using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace Book_Your_Hotel.Controller
{
    [Route("api/Users")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUser _IUser;
        private readonly APIResponse _ApiResponse;
        public UserController(IUser user)
        {
            _IUser = user;
            _ApiResponse = new APIResponse();
        }
        [HttpPost("Login")]

        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var LoginResult = await _IUser.Login(loginRequest);
            if (LoginResult == null || string.IsNullOrEmpty(LoginResult.Token))
            {
                _ApiResponse.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
                _ApiResponse.IsSuccess = false;
                _ApiResponse.Errors.Add("Username or password is invalid");
                return BadRequest(_ApiResponse);
            }
            _ApiResponse.HttpStatusCode = System.Net.HttpStatusCode.OK;
            _ApiResponse.Result = LoginResult;
            return Ok(_ApiResponse);
        }
        [HttpPost("Register")]

        public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterationRequestDTO registerationRequest)
        {
            bool CheckUser = _IUser.IsUniqueUser(registerationRequest.UserName);
            if (!CheckUser)
            {
                _ApiResponse.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
                _ApiResponse.IsSuccess = false;
                _ApiResponse.Errors.Add("Username already exists.");
                return BadRequest(_ApiResponse);
            }
            var registeredUser = await _IUser.Register(registerationRequest);
            if (registeredUser == null)
            {
                _ApiResponse.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
                _ApiResponse.IsSuccess = false;
                _ApiResponse.Errors.Add("Error while registering.");
                return BadRequest(_ApiResponse);

            }
            _ApiResponse.HttpStatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_ApiResponse);

        }
    }
}