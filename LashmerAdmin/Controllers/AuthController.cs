using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LashmerAdmin.Helper;
using LashmerAdmin.Helper.Auth;
using LashmerAdmin.Models;
using LashmerAdmin.Models.DataModels;
using LashmerAdmin.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace LashmerAdmin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ILogger _logger;
		private readonly IJwtFactory _jwtFactory;
		private readonly JwtIssuerOptions _jwtOptions;

		public AuthController(SignInManager<ApplicationUser> signInManager, ILogger logger, IJwtFactory jwtFactory,
			IOptions<JwtIssuerOptions> jwtOptions)
		{
			_signInManager = signInManager;
			_logger = logger;
			_jwtFactory = jwtFactory;
			_jwtOptions = jwtOptions.Value;
		}

		// POST: api/Auth
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CredentialsViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, set lockoutOnFailure: true
			var signedUser = await _signInManager.UserManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
			if (signedUser == null)
			{
				return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid email address.", ModelState));
			}

			var result = await _signInManager.UserManager.CheckPasswordAsync(signedUser, model.Password)
				.ConfigureAwait(false);
			if (!result)
			{
				return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid password.", ModelState));
			}
			_logger.Information("User logged in.");

			var roles = await _signInManager.UserManager.GetRolesAsync(signedUser).ConfigureAwait(false);

			var identity = _jwtFactory.GenerateClaimsIdentity(signedUser.UserName, signedUser.Id);
			var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, signedUser.UserName, roles?.FirstOrDefault(), _jwtOptions,
				new JsonSerializerSettings {Formatting = Formatting.Indented}).ConfigureAwait(false);
			return new OkObjectResult(jwt);
		}
	}
}
