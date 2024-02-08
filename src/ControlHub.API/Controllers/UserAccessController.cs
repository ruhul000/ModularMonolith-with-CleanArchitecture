using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using UserAccess.Application.Services;
using UserAccess.Domain.Models;

namespace ControlHub.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
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

        [HttpGet("VerifyEmail")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> VerifyEmailAddress(string verificationToken)
        {
            var result = await _userService.VerifyEmailAddress(verificationToken);

            if (result.IsFailure)
            {

                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("ForgotPassword")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var result = await _userService.ForgotPassword(email);

            if (result.IsFailure)
            {

                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("ResetPassword")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> ResetPassword(string passwordResetToken, string newPassword)
        {
            var result = await _userService.ResetPassword(passwordResetToken, newPassword);

            if (result.IsFailure)
            {

                _logger.LogError("Error: {Error}, {@DateTimeUtc}", result.Error, DateTime.UtcNow);

                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
