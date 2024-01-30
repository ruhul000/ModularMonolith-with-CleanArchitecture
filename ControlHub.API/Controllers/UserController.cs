using Microsoft.AspNetCore.Mvc;
using UserAccess.Application.Services;
using UserAccess.Domain.Models;

namespace ControlHub.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        [MapToApiVersion("1.0")]
        public ActionResult<UserResponse> Registration(UserRequest userRequest)
        {
            UserResponse? response;
            try
            {
                response = _userService.Registration(userRequest).Result;

                if (response == null)
                {
                    return BadRequest("User registration failed!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        [MapToApiVersion("1.0")]
        public ActionResult<AuthInformation> UserLogin(UserLoginRequest userLoginRequest)
        {
            AuthInformation response;
            try
            {
                response = _userService.UserLogin(userLoginRequest).Result;

                if (response.Token.IsNullOrEmpty() && response.RefreshToken.IsNullOrEmpty())
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("Refresh")]
        [MapToApiVersion("1.0")]
        public ActionResult<AuthInformation> RefreshToken(AuthInformation authInfo)
        {
            AuthInformation response;
            try
            {
                response = _userService.RefreshToken(authInfo).Result;

                if (response.Token.IsNullOrEmpty() && response.RefreshToken.IsNullOrEmpty())
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

    }
}
