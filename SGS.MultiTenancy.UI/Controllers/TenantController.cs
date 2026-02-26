using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.DTOs.Tenants;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Attribute;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{
    [Authorize(Roles = "SGS_SuperHost")]
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly ILocationService _locationService;

        public TenantController(ITenantService tenantService, ILocationService locationService)
        {
            _tenantService = tenantService;
            _locationService = locationService;
        }

        /// <summary>
        /// Displays the list of all tenants.
        /// </summary>
        /// <returns>A view displaying the list of tenants.</returns>
        [HttpGet]
        [HasPermission(permissionId: Permissions.Tenant_View)]
        public async Task<IActionResult> Index()
        {
            List<TenantDto> list = await _tenantService.GetAllAsync();
            return View(list);
        }

        /// <summary>
        /// Displays the form to add a new tenant.
        /// </summary>
        /// <returns>A view for adding a new tenant.</returns>
        [HttpGet]
        public async Task<IActionResult> AddTenant()
        {
            TenantViewModel model = new TenantViewModel();

            IEnumerable<SelectListItem> countries = await _locationService.GetCountriesAsync();
            model.Countries = (List<SelectListItem>)countries;
            model.Tenant.UserDto = new UserDto
            {
                Addresses = new List<CreateUserAddressDto>
                 {
                     new CreateUserAddressDto()
                 }
            };
            string firstCountryId = countries.First().Value;
            model.Tenant.UserDto.Addresses[0].Country = firstCountryId;

            IEnumerable<SelectListItem> states = await _locationService.GetStatesByCountryAsync(Guid.Parse(firstCountryId));

            model.States = (List<SelectListItem>)states;
            return View(model);
        }

        /// <summary>
        /// Handles the submission of the add tenant form.
        /// </summary>
        /// <param name="model">The tenant form data.</param>
        /// <returns>Redirects to the tenant list or redisplays the form if invalid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTenant(TenantViewModel model)
        {
            if (model.Tenant.BusinessLogo != null)
            {
                if (model.Tenant.BusinessLogo.Length > Constants.MaxImageSize)
                {
                    ModelState.AddModelError(
                        "BusinessLogo",
                        Constants.ImageSizeErrorMessage
                    );
                }
                else if (!model.Tenant.BusinessLogo.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(
                        "BusinessLogo",
                        Constants.ImageFormatErrorMessage
                    );
                }
                if (model.Tenant.UserDto?.ProfileImage != null)
                {
                    if (model.Tenant.UserDto.ProfileImage.Length > Constants.MaxImageSize)
                    {
                        ModelState.AddModelError(
                            "UserDto.ProfileImage",
                            Constants.ImageSizeErrorMessage
                        );
                    }
                    else if (!model.Tenant.UserDto.ProfileImage.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(
                            "UserDto.ProfileImage",
                            Constants.ImageFormatErrorMessage
                        );
                    }
                }
            }

            if (!ModelState.IsValid)
                return View(model);
            await _tenantService.CreateAsync(model.Tenant);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the form to update an existing tenant.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant.</param>
        /// <returns>A view for updating the tenant or NotFound if not found.</returns>
        [HttpGet]
        public async Task<IActionResult> EditTenant(Guid id)
        {
            TenantDto tenant = await _tenantService.GetEditModelAsync(id);
            if (tenant == null)
                return NotFound();

            return View(tenant);
        }

        /// <summary>
        /// Handles the submission of the update tenant form.
        /// </summary>
        /// <param name="model">The tenant form data.</param>
        /// <returns>Redirects to the tenant list or redisplays the form if invalid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTenant(TenantDto model)
        {
            if (model.BusinessLogo != null)
            {
                if (model.BusinessLogo.Length > Constants.MaxImageSize)
                {
                    ModelState.AddModelError(
                        nameof(model.BusinessLogo),
                        Constants.ImageSizeErrorMessage
                    );
                }
                else if (!model.BusinessLogo.ContentType
                    .StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(
                        nameof(model.BusinessLogo),
                        Constants.ImageFormatErrorMessage
                    );
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _tenantService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Deletes a tenant by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            bool deleted = await _tenantService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}