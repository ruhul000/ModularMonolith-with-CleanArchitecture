using Helper.Services;
using Microsoft.AspNetCore.Mvc;
using UserAccess.Domain.Models;

namespace ControlHub.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1")]
    public class EmailController : ControllerBase
    {
        private ILogger<EmailController> _logger;
        private IEmailSender _emailSender;

        public EmailController(ILogger<EmailController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost("Send")]
        [MapToApiVersion("1")]
        public async Task<ActionResult> Send(string to, string subject, string body)
        {
            await _emailSender.SendEmailAsync(to, subject, body);

            return Ok();
        }

    }
}
