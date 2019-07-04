using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LashmerAdmin.Data;
using LashmerAdmin.Models.DataModels;
using LashmerAdmin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace LashmerAdmin.Controllers
{
	[Authorize(Policy = "ApiUser")]
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ILogger _logger;
		private readonly ApplicationDbContext _dbContext;

		public EmployeeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
			ILogger logger, ApplicationDbContext dbContext)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_logger = logger;
			_dbContext = dbContext;
		}
		//public async Task<IActionResult> Employees()
		//{
		//    _logger.Information("Get employees");

		//    var employees = new List<EmployeeViewModel>();
		//    var users = _userManager.Users.Where(x => x.UserName != User.Identity.Name).ToList();
		//    foreach (var user in users)
		//    {
		//        var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
		//        employees.Add(new EmployeeViewModel
		//        {
		//            UserName = user.UserName,
		//            FullName = user.FullName,
		//            Email = user.Email,
		//            Id = user.Id,
		//            Role = roles.FirstOrDefault(),
		//            Coupons = string.Join(",", _dbContext.UserCoupons.Where(x => x.UserId == user.Id))
		//        });
		//    }

		//    return View(employees);
		//}

		//public async Task<IActionResult> EditEmployee(string id)
		//{
		//    ViewData["Roles"] = _roleManager.Roles.Select(x => new SelectListItem { Text = x.Name, Value = x.Name }).ToList();
		//    var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
		//    if (user != null)
		//    {
		//        var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
		//        return View(new EmployeeViewModel
		//        {
		//            UserName = user.UserName,
		//            FullName = user.FullName,
		//            Email = user.Email,
		//            Id = user.Id,
		//            Role = roles.FirstOrDefault(),
		//            Coupons = string.Join(",", _dbContext.UserCoupons.Where(x => x.UserId == user.Id))
		//        });
		//    }
		//    return View();
		//}

		//[HttpPost]
		//public async Task<IActionResult> EditEmployee(EmployeeViewModel model)
		//{
		//    _logger.Information("Start updating employee's information");

		//    try
		//    {
		//        ViewData["Roles"] = _roleManager.Roles.Select(x => new SelectListItem {Text = x.Name, Value = x.Name})
		//            .ToList();
		//        if (!ModelState.IsValid)
		//        {
		//            throw new Exception("Validation error.");
		//        }

		//        var user = await _userManager.FindByIdAsync(model.Id).ConfigureAwait(false);
		//        if (user != null)
		//        {
		//            user.FullName = model.FullName;
		//            user.Email = model.Email;
		//            var result = await _userManager.UpdateAsync(user).ConfigureAwait(false);
		//            if (result.Succeeded && ! await _userManager.IsInRoleAsync(user, model.Role).ConfigureAwait(false))
		//            {
		//                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
		//                await _userManager.RemoveFromRolesAsync(user, roles).ConfigureAwait(false);

		//                result = await _userManager.AddToRoleAsync(user, model.Role).ConfigureAwait(false);
		//            }

		//            if (result.Succeeded)
		//            {
		//                return RedirectToAction("Employees");
		//            }

		//            var errorMessage = $"Updating user failed. {result.Errors.FirstOrDefault()?.Description}";
		//            ModelState.AddModelError("", errorMessage);
		//            throw new Exception(errorMessage);
		//        }
		//        else
		//        {
		//            var errorMessage = "Cannot find the user.";
		//            ModelState.AddModelError("", errorMessage);
		//            throw new Exception(errorMessage);
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        _logger.Error(ex, ex.Message);
		//    }
		//    return View(model);
		//}

		[Route("coupons")]
		public IActionResult Coupons()
		{
			//return View(_dbContext.UserCoupons.ToList());
			return Json(_dbContext.UserCoupons.ToList());
		}
	}
}