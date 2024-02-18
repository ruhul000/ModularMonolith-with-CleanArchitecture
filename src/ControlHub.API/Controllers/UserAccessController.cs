using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using UserAccess.Application.Services;
using UserAccess.Domain.Models;

namespace ControlHub.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/UserAccess")]
    [ApiVersion("1")]
    public class UserAccessController : ControllerBase
    {
        private ILogger<UserAccessController> _logger;
        private IUserService _userService;
        public UserAccessController(ILogger<UserAccessController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> Registration(UserRequest userRequest)
        {
            var result = await _userService.Registration(userRequest);

            if (result.IsFailure)
            {
                
                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> UserLogin(UserLoginRequest userLoginRequest)
        {
            var result = await _userService.UserLogin(userLoginRequest);

            if (result.IsFailure)
            {

                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Refresh")]
        [MapToApiVersion("1")]
        public async Task<ActionResult<AuthInformation>> RefreshToken(AuthInformation authInfo)
        {
            AuthInformation response;
            try
            {
                response = await _userService.RefreshToken(authInfo);

                if (response.Token.IsNullOrEmpty() && response.RefreshToken.IsNullOrEmpty())
                {
                    _logger.LogError("Error: {Error}, {@DateTimeUtc}", Unauthorized(), DateTime.UtcNow);
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {Message}, {InnerException}, {Exception}, {DateTimeUtc}", ex.Message, ex.InnerException, ex, DateTime.UtcNow);
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("VerifyEmail")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> VerifyEmailAddress([FromBody] VerifyEmailRequest request)
        {
            var result = await _userService.VerifyEmailAddress(request.Token);

            if (result.IsFailure)
            {
                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _userService.ForgotPassword(request.Email);

            if (result.IsFailure)
            {
                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _userService.ResetPassword(request.Email, request.ResetCode, request.NewPassword);

            if (result.IsFailure)
            {
                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
