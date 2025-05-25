using AutoMapper;
using Trustesse_Assessment.AuthServices;
using Trustesse_Assessment.Dto;
using Trustesse_Assessment.EmailService;
using Trustesse_Assessment.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Trustesse_Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuth _auth;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, IMapper mapper, ILogger<AccountController> logger, IAuth auth, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration attempt for {userDto.Email}!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<AppUser>(userDto);
            user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                //Give exact Error message info on the action performed 
                foreach (var error in result.Errors){
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, token }, Request.Scheme);

            await _emailService.SendEmailAsync(user.Email, "Verify Your Email", $"Click <a href='{confirmationLink}'>here</a> to verify your email.");

            return Accepted();

        }

        [HttpGet]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto verifyDto)
        {
            var user = await _userManager.FindByEmailAsync(verifyDto.email);

            if (user == null || verifyDto.token == null)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.ConfirmEmailAsync(user, verifyDto.token);

            if (result.Succeeded)
            {
                _logger.LogInformation($"{nameof(VerifyEmail)}");
                return Ok();
            }
            else
            {
                ModelState.AddModelError("", "Email cannot be confirmed");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
 
            _logger.LogInformation($"Login attempt for {loginDto.Email}!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _auth.ValidateUser(loginDto))
            {
                return Unauthorized();
            }
            else
            {
                 return Accepted( new { Token = await _auth.CreateToken()});
            }

        }
    }
}
