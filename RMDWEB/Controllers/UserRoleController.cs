using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using RMDWEB.Models;
using System.Runtime.Intrinsics.Arm;
using RMDWEB.Interface;
using RMDWEB.Impl;
using RMDWEB.Services.Interface;
using RMDWEB.Services.Impl;

namespace RMDWEB.Controllers
{
    [Authorize(Roles = "sys-admin")]
    public class UserRoleController : Controller
    {
        private readonly ILogger<UserRoleController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly InterfaceSystemConfig _iConfig;
        private readonly InterfaceActivityLog _activitylog;
        private readonly InterfaceBank _ibank;
        private readonly InterfaceDepartment _idepartment;

        public UserRoleController(ILogger<UserRoleController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _iConfig = new RepoSystemConfig();
            _roleManager = roleManager;
            _userManager = userManager;
            _activitylog = new RepoActivityLog();
            _ibank = new RepoBank();
            _idepartment = new RepoDepartment();
        }



        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            //Add log for editing Banks
            ActivityLog log = new ActivityLog()
            {
                Activity = "UserRole/View Users",
                ActivityDate = DateTime.Now,
                UserName = User.Identity.Name,
                TableName = "Users",
                Detail = "View Users",
            };
            _activitylog.Add(log);
            return View(users.ToList());
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;

            var user = new ApplicationUser();
            //check if role is user admin else other one
            user = _userManager.Users.Single(a => a.UserId.ToString() == userId);
            //var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            else
            {
                ViewBag.UserName = user.UserName;
                var model = new List<ManageUserRolesViewModel>();
                foreach (var role in _roleManager.Roles)
                {
                    var userRolesViewModel = new ManageUserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRolesViewModel.Selected = true;
                    }
                    else
                    {
                        userRolesViewModel.Selected = false;
                    }
                    model.Add(userRolesViewModel);
                }

                // Add log for editing Banks
                ActivityLog l = new ActivityLog()
                {
                    Activity = "UserRole/Manage Users",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Users",
                    Detail = "View User Roles",
                };
                _activitylog.Add(l);

                return View(model);

            }

        }

        public IActionResult Details(int userId)
        {
            ApplicationUser u = _userManager.Users.Single(a => a.UserId == userId);

            if (u == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            else
            {
                // Add log for editing Banks
                ActivityLog l = new ActivityLog()
                {
                    Activity = "UserRole/Users Details",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Users",
                    Detail = "View User Details",
                };
                _activitylog.Add(l);
                return View(u);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {

            var user = _userManager.Users.Single(a => a.UserId.ToString() == userId);
            //var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            TempData["success"] = "User Role updated!";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(string id)
        {

            if (id == null || id == "")
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }


            ApplicationUser u = new ApplicationUser();

            if (id != null && id != "")
            {
                u = await _userManager.FindByIdAsync(id);
                u.DepartmentTbl = _idepartment.singleDepartment(u.DepartmentId);
                u.StatusTbl = _iConfig.singelStatus(u.StatusId);

            }
            var dep = _iConfig.AllDepartments().Single(a => a.DepartmentId == u.DepartmentId);

            ViewBag.StatusId = new SelectList(_iConfig.AllStatus(), "StatusId", "Name", u.StatusId);
            ViewBag.DepartmentId = new SelectList(_iConfig.AllDepartments(), "DepartmentId", "Name", u.DepartmentId);
            ViewBag.BankId = new SelectList(_iConfig.AllBanks(), "BankId", "Name", dep.BankId);

            if (u == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                ViewBag.MSG = TempData["MSG"];
                return View(u);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);

            if (existingUser == null)
            {
                // User not found
                return NotFound();
            }

            // Update the properties of the existing user
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.FatherName = user.FatherName;
            existingUser.Email = user.Email;
            existingUser.UserId = user.UserId;
            existingUser.RegisterDate = user.RegisterDate;
            existingUser.PhoneNumberConfirmed=user.PhoneNumberConfirmed;
            existingUser.IpAddress=user.IpAddress;
            existingUser.ExpiryDate=user.ExpiryDate;
            existingUser.Designation=user.Designation;
            existingUser.PhoneNumber=user.PhoneNumber;
            existingUser.StatusId=user.StatusId;
            existingUser.DepartmentId=user.DepartmentId;
            existingUser.BankId=user.BankId;
           
            // Update other properties as needed

            // Save the changes
            var result = await _userManager.UpdateAsync(existingUser);

            if (result.Succeeded)
            {
                // Update successful
                TempData["success"] = "User updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                // Failed to update user
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
               
                return View(user); // Show the edit view with validation errors
            }
        }


        public async Task<IdentityResult> ResetPasswordAsync(int UserId, string UserPassword)
        {
            var uID = _userManager.Users.Single(a => a.UserId == UserId).Id;
            ApplicationUser u = await _userManager.FindByIdAsync(uID);
            string token = await _userManager.GeneratePasswordResetTokenAsync(u);
            return await _userManager.ResetPasswordAsync(u, token, UserPassword);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> IndexRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName, string id)
        {
            if (id == null && roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            else
            {

                if (id != null)
                {
                    IdentityRole r = await _roleManager.FindByIdAsync(id);
                    await _roleManager.UpdateAsync(r);
                }

            }
            return RedirectToAction("IndexRole");
        }

        [HttpGet]
        public async Task<ActionResult> DetailsRole(string id)
        {
            if (id == null)
            {
                return NoContent();
            }
            var role = await _roleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUserView>();
            // Get the list of Users in this Role

            // var urole = _userManager.Users.ToList().(role.Id);
            var urole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (urole != null)
            {
                foreach (var user in urole)
                {
                    ApplicationUserView u = new ApplicationUserView();
                    u.UserName = user.UserName;
                    users.Add(u);
                }
            }
            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

    }

}
