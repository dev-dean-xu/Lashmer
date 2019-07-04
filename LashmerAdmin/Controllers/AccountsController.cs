using System.Linq;
using System.Threading.Tasks;
using LashmerAdmin.Helper;
using LashmerAdmin.Models.DataModels;
using LashmerAdmin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LashmerAdmin.Controllers
{
	[Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public AccountsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            ILogger logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
				return BadRequest(ModelState.Values.Select(x=>x.Errors.Select(err=>err.ErrorMessage)));
            }

			// Check if email is used
	        var existingUser = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
			if (existingUser != null)
	        {
				return BadRequest(new []{$"Email {model.Email} is already taken."});
			}

            var user = new ApplicationUser {UserName = model.UserName, Email = model.Email, FullName = model.FullName};
            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors.Select(x=>x.Description));
            }

            _logger.Information("User created a new account with password.");

            //Assign a role to the User
            if (!await _userManager.IsInRoleAsync(user, model.Role).ConfigureAwait(false))
            {
                await _userManager.AddToRoleAsync(user, model.Role).ConfigureAwait(false);
            }
            _logger.Information("Assign {Role} role to the user", model.Role);

            return new OkResult();
        }

		[HttpGet("roles")]
	    public string[] GetRoles()
		{
			return _roleManager.Roles.Select(x => x.Name).ToArray();
		}
    }
}