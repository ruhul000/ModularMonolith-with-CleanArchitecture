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
        public ActionResult<UserResponse> Registration(UserRequest userRequest)
        {
            UserResponse? response;
            try
            {
                response = _userService.Registration(userRequest).Result;

                if (response == null)
                {
                    _logger.LogError("Error: {Error}, {@DateTimeUtc}", BadRequest(), DateTime.UtcNow);

                    return BadRequest("User registration failed!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {Message}, {InnerException}, {Trace}, {DateTimeUtc}", ex.Message, ex.InnerException, ex, DateTime.UtcNow);
                return BadRequest("User registration failed!");
            }


            return Ok(response);
        }

        [HttpPost("Login")]
        [MapToApiVersion("1")]
        public ActionResult<AuthInformation> UserLogin(UserLoginRequest userLoginRequest)
        {
            AuthInformation response;
            try
            {
                response = _userService.UserLogin(userLoginRequest).Result;

                if (response.Token.IsNullOrEmpty() || response.RefreshToken.IsNullOrEmpty())
                {
                    _logger.LogError("Error: {Error}, {@DateTimeUtc}", BadRequest(), DateTime.UtcNow);
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {Message}, {InnerException}, {Trace}, {DateTimeUtc}", ex.Message, ex.InnerException, ex, DateTime.UtcNow);
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("Refresh")]
        [MapToApiVersion("1")]
        public ActionResult<AuthInformation> RefreshToken(AuthInformation authInfo)
        {
            AuthInformation response;
            try
            {
                response = _userService.RefreshToken(authInfo).Result;

                if (response.Token.IsNullOrEmpty() && response.RefreshToken.IsNullOrEmpty())
                {
                    _logger.LogError("Error: {Error}, {@DateTimeUtc}", Unauthorized(), DateTime.UtcNow);
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {Message}, {InnerException}, {Trace}, {DateTimeUtc}", ex.Message, ex.InnerException, ex, DateTime.UtcNow);
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

    }
}
