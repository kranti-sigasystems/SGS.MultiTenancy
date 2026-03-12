using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.DTOs.Role;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Services;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITenantProvider _tenantProvider;
        private readonly ILocationService _locationService;
            private readonly IRoleService _roleService;
        public UserController(IUserService userService, ITenantProvider tenantProvider, ILocationService locationService, IRoleService roleService)
        {
            _userService = userService;
            _tenantProvider = tenantProvider;
            _locationService = locationService;
            _roleService = roleService;
        }

        /// <summary>
        /// Returns the default view for the Index page.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;

            List<UserDto> users = await _userService.GetUsersByTenantAsync(tenantId);
            UserViewModel model = new UserViewModel();


            foreach (UserDto user in users)
            {
                if (user.Addresses == null || !user.Addresses.Any())
                {
                    user.Addresses = new List<CreateUserAddressDto>
                    {
                        new CreateUserAddressDto()
                    };
                }
            }

            model.UserList = users;
            
            model.StatusOptions = Enum.GetValues<EntityStatus>()
           .Select(s => new SelectListItem
           {
               Value = ((int)s).ToString(),
               Text = s.ToString(),
               Selected = s == model.User.Status
           });
            return View(model);
        }

        /// <summary>
        /// Creates a new user form.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserViewModel model = new UserViewModel();
            List<RoleDto> roles = await _roleService.GetRolesByTenantAsync((Guid)_tenantProvider.TenantId);

            model.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.ID.ToString(),
                Text = r.Name
            }).ToList();

            IEnumerable<SelectListItem> countries = await _locationService.GetCountriesAsync();
            model.User.TenantId = (Guid)_tenantProvider.TenantId!;
            model.Countries = (List<SelectListItem>)countries;
            model.User = new UserDto
            {
                Addresses = new List<CreateUserAddressDto>
                 {
                     new CreateUserAddressDto()
                 }
            };
            string firstCountryId = countries.First().Value;
            model.User.Addresses[0].Country = firstCountryId;

            IEnumerable<SelectListItem> states = await _locationService.GetStatesByCountryAsync(Guid.Parse(firstCountryId));

            model.States = (List<SelectListItem>)states;
            return View(model);
        }

        /// <summary>
        /// Handles HTTP POST requests to create a new user.
        /// </summary>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (model?.User.ProfileImage != null)
            {
                if (model.User.ProfileImage.Length > Constants.MaxImageSize)
                {
                    ModelState.AddModelError("User.ProfileImage", Constants.ImageSizeErrorMessage);
                }
                else if (!model.User.ProfileImage.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("User.ProfileImage", Constants.ImageFormatErrorMessage);
                }
            }

            ModelState.Remove("User.ConfirmPassword");
            if (!ModelState.IsValid)
            {
                Guid tenantId = (Guid)_tenantProvider.TenantId!;

                List<RoleDto> roles = await _roleService.GetRolesByTenantAsync(tenantId);
                model.Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.ID.ToString(),
                    Text = r.Name
                }).ToList();

                IEnumerable<SelectListItem> countries = await _locationService.GetCountriesAsync();
                model.Countries = countries.ToList();

                if (model.User.Addresses != null && model.User.Addresses.Count > 0)
                {
                    IEnumerable<SelectListItem> states = await _locationService.GetStatesByCountryAsync(Guid.Parse(model.User.Addresses[0].Country));
                    model.States = states.ToList();
                }

                return View(model);
            }

            try
            {
                model.User.TenantId = (Guid)_tenantProvider.TenantId!;

                await _userService.AddUserAsync(model.User);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Softdelete user.
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            bool isDelete = await _userService.DeleteUserAsync(id, tenantId);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Retrieves a list of states with the specified country identifier.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatesByCountry(Guid countryId)
        {
            IEnumerable<SelectListItem> states = await _locationService.GetStatesByCountryAsync(countryId);
            IEnumerable<SelectListItem> result = states;
            return Json(result);
        }

        /// <summary>
        /// Retrieves a list of states with the specified country identifier.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UpdateUser(Guid id)
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            UserViewModel model = new();
            UserDto? user = await _userService.GetUserByTenantIDAndUserIDAsync(id, tenantId);

            if (user == null)
                return NotFound();

            model.User = user;
            model.StatusOptions = Enum.GetValues<EntityStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                })
                .ToList();

            List<RoleDto> roles = await _roleService.GetRolesByTenantAsync(tenantId);
            model.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.ID.ToString(),
                Text = r.Name
            }).ToList();

           
            IEnumerable<SelectListItem> countries = await _locationService.GetCountriesAsync();
            model.Countries = countries.ToList();
            string firstCountryId = countries.First().Value;
            IEnumerable<SelectListItem> states = await _locationService.GetStatesByCountryAsync(Guid.Parse(firstCountryId));
            model.States = states.ToList();

            return View(model);
        }

        /// <summary>
        /// Update user info.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel model)
        {
            model.User.TenantId = (Guid)_tenantProvider.TenantId!;
            if (model?.User.ProfileImage != null)
            {
                if (model?.User.ProfileImage.Length > Constants.MaxImageSize)
                {
                    ModelState.AddModelError(
                        "User.ProfileImage",
                        Constants.ImageSizeErrorMessage
                    );
                    if (!ModelState.IsValid)
                    {
                        return PartialView("_EditUserPartial", model);
                    }
                }
                if (!model.User.ProfileImage.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(
                        "User.ProfileImage",
                        Constants.ImageFormatErrorMessage
                    );
                    if (!ModelState.IsValid)
                    {
                        return PartialView("_EditUserPartial", model);
                    }
                }
            }
            await _userService.UpdateUserAsync(model.User);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the details of a user with the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            UserDto? user = await _userService.GetUserByTenantIDAndUserIDAsync(id, tenantId);
            return View(user);
        }
    }
}