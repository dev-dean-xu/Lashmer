using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LashmerAdmin.Data;
using LashmerAdmin.Models.DataModels;
using LashmerAdmin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace LashmerAdmin.Controllers
{
	[Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
	    private readonly ILogger _logger;
	    private readonly UserManager<ApplicationUser> _userManager;
	    private readonly RoleManager<IdentityRole> _roleManager;

		public EmployeesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger, ApplicationDbContext context)
        {
	        _roleManager = roleManager;
	        _userManager = userManager;
	        _logger = logger;
			_dbContext = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<EmployeeViewModel>> Get()
        {
			_logger.Information("Get employees");

			var employees = new List<EmployeeViewModel>();
			var users = _userManager.Users.Where(x => x.UserName != User.Identity.Name).ToList();
			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
				employees.Add(new EmployeeViewModel
				{
					UserName = user.UserName,
					FullName = user.FullName,
					Email = user.Email,
					Id = user.Id,
					Role = roles.FirstOrDefault(),
					Coupons = string.Join(",", _dbContext.UserCoupons.Where(x => x.UserId == user.Id).Select(x=>x.Coupon))
				});
			}

	        return employees;
        }

		// GET: api/Employees/5
		[HttpGet("{id}")]
        public async Task<EmployeeViewModel> Get(string id)
		{
			var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
			if (user != null)
			{
				var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
				return new EmployeeViewModel
				{
					UserName = user.UserName,
					FullName = user.FullName,
					Email = user.Email,
					Id = user.Id,
					Role = roles.FirstOrDefault(),
					Coupons = string.Join(",", _dbContext.UserCoupons.Where(x => x.UserId == user.Id).Select(x=>x.Coupon))
				};
			}

			return null;
		}

		// PUT: api/Employees/5
		[HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] EmployeeViewModel model)
        {
	        _logger.Information("Start updating employee's information");
			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors.Select(err => err.ErrorMessage)));
            }

            if (id != model.Id)
            {
                return BadRequest(new[]{"User Id is wrong."});
            }

            try
            {
				var user = await _userManager.FindByIdAsync(model.Id).ConfigureAwait(false);
				if (user != null)
				{
					// Check if email is used
					var existingUser = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
					if (existingUser != null && existingUser.Id != user.Id)
					{
						return BadRequest(new[] { $"Email {model.Email} is already taken." });
					}

					user.FullName = model.FullName;
					user.Email = model.Email;
					user.UserName = model.UserName;
					var result = await _userManager.UpdateAsync(user).ConfigureAwait(false);
					if (result.Succeeded && !await _userManager.IsInRoleAsync(user, model.Role).ConfigureAwait(false))
					{
						var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
						await _userManager.RemoveFromRolesAsync(user, roles).ConfigureAwait(false);

						result = await _userManager.AddToRoleAsync(user, model.Role).ConfigureAwait(false);
					}

					if (result.Succeeded)
					{
						return Ok();
					}

					var errorMessage = $"Updating user failed. {result.Errors.FirstOrDefault()?.Description}";
					_logger.Error(errorMessage);
					return BadRequest(new[]{ errorMessage });
				}
				else
				{
					var errorMessage = "Cannot find the user.";
					_logger.Error(errorMessage);
					return BadRequest(new[] { errorMessage });
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error happened while updating user {userName}", model.UserName);
				return BadRequest(new[]{ex.Message});
			}
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok();
        }

		[Route("{id}/coupons")]
		[ProducesResponseType(typeof(UserCoupon[]),200)]
		[HttpGet]
	    public async Task<IActionResult> GetUserCoupons(string id)
		{
			var coupons = await _dbContext.UserCoupons.Where(x => x.UserId == id).Select(x=>x.Coupon).ToArrayAsync().ConfigureAwait(false);
			return Ok(coupons);
		}

	    [Route("{id}/coupons")]
	    [HttpPost]
	    public async Task<IActionResult> AddUserCoupons([FromRoute] string id, [FromBody] UserCoupon coupon)
	    {
		    if (id != coupon.UserId)
		    {
			    return BadRequest(new[] {"User Id is wrong."});
		    }
		    var code = coupon.Coupon.Trim();
		    var validationResult = await ValidateUserAndPromotionCodeAsync(id, code).ConfigureAwait(false);

			if (!string.IsNullOrEmpty(validationResult))
			{
				return BadRequest(new[] {validationResult});
			}

			var existingCode = await _dbContext.UserCoupons.FirstOrDefaultAsync(x =>
				x.UserId == id && x.Coupon.Equals(code, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
		    if (existingCode != null)
		    {
				return BadRequest(new[] { $"This user already has the promotion code {code}." });
			}

		    await _dbContext.UserCoupons.AddAsync(new UserCoupon {UserId = id, Coupon = code}).ConfigureAwait(false);
		    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return Ok();
	    }

	    [Route("{id}/coupons/{code}")]
	    [HttpDelete]
	    public async Task<IActionResult> DeleteUserCoupons(string id, string code)
	    {
		    code = code.Trim();
		    var validationResult = await ValidateUserAndPromotionCodeAsync(id, code).ConfigureAwait(false);

		    if (!string.IsNullOrEmpty(validationResult))
		    {
			    return BadRequest(new[] { validationResult });
		    }

		    var existingCode = await _dbContext.UserCoupons.FirstOrDefaultAsync(x =>
			    x.UserId == id && x.Coupon.Equals(code, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
		    if (existingCode != null)
		    {
			    _dbContext.UserCoupons.Remove(existingCode);
			    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
			}
		   
			return Ok();
	    }

	    private async Task<string> ValidateUserAndPromotionCodeAsync(string id, string code)
	    {
			if (string.IsNullOrEmpty(code))
			{
				return "The promotion code is empty.";
			}

		    var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
		    if (user == null)
		    {
			    return "User Id is wrong.";
		    }

			return null;
		}
	}
}