// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using DrivePal.Models;

namespace DrivePal.Areas.Identity.Pages.Account
{
    public class RegisterModelInstructor : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModelInstructor> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModelInstructor(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModelInstructor> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
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

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Street")]
            public string Street { get; set; }

            [Required]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [Display(Name = "Post Code")]
            [RegularExpression(@"^[A-Z]{1,2}\d{1,2}\s?\d[A-Z]{2}$", ErrorMessage = "The Post Code is not in a valid format.")]
            public string PostCode { get; set; }

            [Required]
            [Display(Name = "Date of Birth")]
            public DateOnly DOB { get; set; }

            [Display(Name = "License Number")]
            [Required]
            [StringLength(16, MinimumLength = 16, ErrorMessage = "The License Number must be exactly 16 characters.")]
            [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The License Number must contain only letters and numbers.")]
            public string LicenceNumber { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync((Instructor)user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync((Instructor)user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync((Instructor)user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new Instructor account with password.");

                    // Confirm the email directly without sending a confirmation email
                    var instructorUser = (Instructor)user;
                    instructorUser.EmailConfirmed = true;
                    var confirmResult = await _userManager.UpdateAsync(instructorUser);

                    if (confirmResult.Succeeded)
                    {
                        _logger.LogInformation("User's email (Instructor) confirmed successfully.");

                        // Add the user to the Instructor role
                        var addToRoleResult = await _userManager.AddToRoleAsync(instructorUser, "Instructor");
                        if (addToRoleResult.Succeeded)
                        {
                            _logger.LogInformation("User added to Instructor role successfully.");
                        }
                        else
                        {
                            foreach (var error in addToRoleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            // If adding to role failed, return the page to display the errors
                            return Page();
                        }

                    }
                    else
                    {
                        foreach (var error in confirmResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        // If the email confirmation update failed, return the page to display the errors
                        return Page();
                    }
                    
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });

                }

                // code for sending confirmation
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("User created a new account with password.");

                //    var userId = await _userManager.GetUserIdAsync((Instructor)user);
                //    var code = await _userManager.GenerateEmailConfirmationTokenAsync((Instructor)user);
                //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //    var callbackUrl = Url.Page(
                //        "/Account/ConfirmEmail",
                //        pageHandler: null,
                //        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                //        protocol: Request.Scheme);

                //    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                //        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                //    {
                //        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                //    }
                //    else
                //    {
                //        await _signInManager.SignInAsync((Instructor)user, isPersistent: false);
                //        return LocalRedirect(returnUrl);
                //    }
                //}
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }
            }



            // If we got this far, something failed, redisplay form
            return Page();
        }

        private Instructor CreateUser()
        {
            try
            {
                var instructor = Activator.CreateInstance<Instructor>();

                // Set the additional properties
                instructor.FirstName = Input.FirstName;
                instructor.LastName = Input.LastName;
                instructor.Street = Input.Street;
                instructor.City = Input.City;
                instructor.PostCode = Input.PostCode;
                instructor.DOB = Input.DOB;
                instructor.LicenceNumber = Input.LicenceNumber;
                instructor.isBlocked = false;
                instructor.isApproved = true;
                instructor.TotalRating = 0;

                return instructor;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of 'Instructor'. Ensure that 'Instructor' is not an abstract class and has a parameterless constructor, or alternatively override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
