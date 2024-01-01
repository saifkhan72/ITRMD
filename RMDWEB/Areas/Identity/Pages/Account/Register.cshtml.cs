// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using HELPER;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using RMDWEB.Models;
using RMDWEB.Impl;
using RMDWEB.Interface;
using RMDWEB.Models;

namespace RMDWEB.Areas.Identity.Pages.Account
{

    [Authorize(Roles = "sys-admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
 
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager=userManager;
            _userStore=userStore;
            _emailStore=GetEmailStore();
            _signInManager=signInManager;
            _logger=logger;
            _emailSender=emailSender;



        }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 


        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

         


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 

            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            public int? UserId { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public string? Designation { get; set; }
            public string FatherName { get; set; }
            public int? Dab { get; set; }
            public DateTime? RegisterDate { get; set; }
            public string? IpAddress { get; set; }

            public int StatusId { get; set; }
            public int DepartmentId { get; set; }
            public int? BankId { get; set; }


            [ForeignKey("StatusId")]
            public virtual StatusTbl StatusTbl { get; set; }

            [ForeignKey("DepartmentId")]
            public virtual DepartmentTbl DepartmentTbl { get; set; }


            [ForeignKey("BankId")]
            public virtual BankTbl BankTbl { get; set; }

            public string PhoneNumber { get; set; }
  
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

         }
 

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl=returnUrl; 
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl??=Url.Content("~/");
            ExternalLogins=(await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if(ModelState.IsValid)
            {
                var user = CreateUser();
                user.FirstName=Input.FirstName;
                user.LastName=Input.LastName;
                user.Email=Input.Email;
                user.ExpiryDate=Input.ExpiryDate;
                user.Dab=Input.Dab;
                user.PhoneNumber=Input.PhoneNumber;
                user.EmailConfirmed=true;
                user.UserId=int.Parse(RandomGeneration.GenerateRandomNumber(6));
                user.Designation=Input.Designation;
                user.FatherName=Input.FatherName;
                user.IpAddress=Input.IpAddress;
                user.RegisterDate=Input.RegisterDate;
                user.PhoneNumberConfirmed=true;
                user.StatusId=Input.StatusId;
                user.DepartmentId=Input.DepartmentId;
                user.BankId=Input.BankId;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);
                if(result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    TempData["success"] = "User created successfully!";
                    return RedirectToAction("Index", "UserRole");
                    
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();

            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. "+
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively "+
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if(!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
