using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtOptions _jwtOptions;
        public AuthController(IUserService userService, IOptions<JwtOptions> jwtOptions)
        {
            _userService = userService;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto()
            {
                Password = loginViewModel.Password,
                UserName = loginViewModel.UserName,
            };
            LoginResponseDto loginResponse = await _userService.Login(loginRequestDto);

            if (string.IsNullOrWhiteSpace(loginResponse.Token) || loginResponse.User == null)
            {
                ModelState.AddModelError("", Constants.InvalidLogin);
                return View(loginViewModel);
            }

            Response.Cookies.Append("SGS_AuthToken", loginResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.Now.AddMinutes(_jwtOptions.ExpiryMinutes)
            });

            await CreateMvcSessionAsync(loginResponse);
            return RedirectToAction(nameof(DashBoardController.Index), Utility.PrepareControllerName(nameof(DashBoardController)));
        }

        private async Task CreateMvcSessionAsync(LoginResponseDto loginResponse)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResponse.User.ID.ToString()),
                new Claim(ClaimTypes.Name, loginResponse.User.UserName),
            };

            foreach (string role in loginResponse.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            foreach (string permission in loginResponse.Permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                ))
            );
        }
    }
}