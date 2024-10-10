using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using T_Microservices.Services.AuthAPI.Models.Dto;
using T_Microservices.Services.AuthAPI.Service.IService;

namespace T_Microservices.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            LoginResponseDto? loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";

                return BadRequest(_response);
            }

            _response.Result = loginResponse;

            return Ok(_response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            string? errorMessage = await _authService.Register(model);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;

                return BadRequest(_response);
            }

            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            bool assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());

            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";

                return BadRequest(_response);
            }

            return Ok(_response);
        }

    }
}
