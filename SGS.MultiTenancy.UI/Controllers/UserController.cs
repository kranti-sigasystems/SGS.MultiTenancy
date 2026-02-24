using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITenantProvider _tenantProvider;
        private readonly ILocationService _locationService;
        public UserController(IUserService userService, ITenantProvider tenantProvider, ILocationService locationService)
        {
            _userService = userService;
            _tenantProvider = tenantProvider;
            _locationService = locationService;
        }

        /// <summary>
        /// Returns the default view for the Index page.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;

            List<UserDto> users = await _userService.GetUsersByTenantAsync(tenantId);
            UserViewModel model = new UserViewModel();

            IEnumerable<SelectListItem> countries = await _locationService.GetCountriesAsync();

            model.Countries = (List<SelectListItem>)countries;

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

            string firstCountryId = countries.First().Value;
            IEnumerable<SelectListItem> states = await _locationService.GetStatesByCountryAsync(Guid.Parse(firstCountryId));
            model.UserList = users;
            model.States = (List<SelectListItem>)states;
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
        public async Task<IActionResult> Create(UserViewModel dto)
        {
            if (dto?.User.ProfileImage != null)
            {
                if (dto?.User.ProfileImage.Length > Constants.MaxImageSize)
                {
                    ModelState.AddModelError(
                        "User.ProfileImage",
                        Constants.ImageSizeErrorMessage
                    );
                }
                else if (!dto.User.ProfileImage.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(
                        "User.ProfileImage",
                        Constants.ImageFormatErrorMessage
                    );
                }
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            foreach (CreateUserAddressDto address in dto.User.Addresses)
            {
                address.Country = await _locationService.GetCountryNameByIdAsync(address.Country);
                address.State = await _locationService.GetStateNameByIdAsync(address.State);
            }
            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            dto.User.TenantId = tenantId;
            dto.User.Status = EntityStatus.Active;
            dto.User.RoleIds.Add(Guid.Parse(Constants.UserRoleId));
            await _userService.AddUserAsync(dto.User);
            return RedirectToAction(nameof(Index));
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
            UserDto? user = await _userService.GetUserByTenantIDAndUserIDAsync(tenantId, id);
            var selectedValue = ((int)user.Status).ToString();

            model.User.Status = user.Status;

            model.StatusOptions = Enum.GetValues<EntityStatus>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),  
                    Text = s.ToString()
                })
                .ToList();

            model.User = user;

            return  View( model);
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

    }
}