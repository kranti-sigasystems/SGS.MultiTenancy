using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;
using System.Security.Claims;

namespace SGS.MultiTenancy.UI.Controllers
{
    /// <summary>
    /// Manages user authentication: login, logout, and password operations.
    /// </summary>
    public class AuthController : Controller
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AuthController"/>.
        /// </summary>
        /// <param name="userService">Service for user authentication.</param>
        /// <param name="jwtOptions">JWT configuration options.</param>

        private readonly IUserService _userService;
        private readonly JwtOptions _jwtOptions;
        public AuthController(IUserService userService, IOptions<JwtOptions> jwtOptions)
        {
            _userService = userService;
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Shows login page and redirects authenticated users to dashboard.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Processes login requests.
        /// Validates credentials and sets authentication cookie.
        /// </summary>
        /// <param name="loginViewModel">User login input.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            LoginRequestDto loginRequestDto = new LoginRequestDto()
            {
                Password = loginViewModel.Password,
                UserName = loginViewModel.UserName,
            };
            LoginResponseDto loginResponse = await _userService.Login(loginRequestDto);

            if (string.IsNullOrWhiteSpace(loginResponse.Token) || loginResponse.User == null)
            {
                ModelState.AddModelError(string.Empty, Constants.InvalidLogin);
                return View(loginViewModel);
            }

            await CreateMvcSessionAsync(loginResponse);
            return RedirectToAction(nameof(TenantController.Index), Utility.PrepareControllerName(nameof(TenantController)));
        }

        /// <summary>
        /// Displays forgot password page.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Processes forgot password request.
        /// Sends reset password instructions to user email.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await _userService.ForgotPasswordAsync(model.Email);

            // IMPORTANT: do NOT reveal if email exists (security best practice)
            TempData["SuccessMessage"] = Constants.PasswordResetLinkSent;

            return RedirectToAction(nameof(Login), Utility.PrepareControllerName(nameof(AuthController)));
        }


        /// <summary>
        /// Displays the change password page.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Processes change password requests.
        /// Signs out user and prompts login on success.
        /// </summary>
        /// <param name="changePasswordViewModel">New and current password input.</param>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordViewModel);
            }

            string? userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out Guid userId))
            {
                return RedirectToAction(nameof(Login), Utility.PrepareControllerName(nameof(AuthController)));
            }

            (bool success, string errorMessage) result =
            await _userService.ChangePasswordAsync(
            userId,
            changePasswordViewModel.CurrentPassword,
            changePasswordViewModel.NewPassword
            );

            bool success = result.success;
            string errorMessage = result.errorMessage;

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(changePasswordViewModel);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("SGS_AuthToken");

            TempData["SuccessMessage"] = Constants.PasswordChangedSuccess;

            return RedirectToAction(nameof(Login), Utility.PrepareControllerName(nameof(AuthController)));
        }
        /// <summary>
        /// Creates authentication session with user claims, roles, and permissions.
        /// </summary>
        /// <param name="loginResponse">Authenticated user information.</param>
        private async Task CreateMvcSessionAsync(LoginResponseDto loginResponse)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResponse.User.ID.ToString()),
                new Claim(ClaimTypes.Name, loginResponse.User.UserName),
                new Claim("tenantId", loginResponse.TenantID.ToString())
            };

            foreach (string role in loginResponse.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                ))
            );
        }
        /// <summary>
        /// Logs out the current user and clears authentication cookies.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("SGS_AuthToken");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}