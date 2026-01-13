using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGS.MultiTenancy.Core.Application.DTOs.Tenants;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

namespace SGS.MultiTenancy.UI.Controllers
{
    [Authorize (Roles = "SGS_SuperHost")]
    public class TenantController : Controller
    {
        private readonly ILogger<TenantController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITenantService _tenantService;
        private readonly ILocationService _locationService;
        private readonly int _pageSizeFromConfig;

        public TenantController(ILogger<TenantController> logger, IConfiguration configuration, ITenantService tenantService, ILocationService locationService)
        {
            _logger = logger;
            _configuration = configuration;
            _tenantService = tenantService;
            _locationService = locationService;
            _pageSizeFromConfig = configuration.GetValue<int>("ApiSettings:PageSize", 10);
        }

        /// <summary>
        /// Displays the list of all tenants.
        /// </summary>
        /// <returns>A view displaying the list of tenants.</returns>
        public async Task<IActionResult> Index(int page = 1, int? pageSize = null)
        {
            int itemsPerPage = pageSize ?? _pageSizeFromConfig;

            PagedResult<Tenant> result = await _tenantService.GetPagedTenantsAsync(page, itemsPerPage);

            return View(result);
        }


        /// <summary>
        /// Retrieves the states for a given country.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country.</param>
        /// <returns>A JSON result containing the list of states.</returns>
        [HttpGet]
        public async Task<JsonResult> GetStates(Guid countryId)
        {
            IEnumerable<SelectListItem> states = await _locationService.GetStatesByCountryAsync(countryId);
            return Json(states);
        }

        /// <summary>
        /// Displays the form to add a new tenant.
        /// </summary>
        /// <returns>A view for adding a new tenant.</returns>
        [HttpGet]
        public async Task<IActionResult> AddTenant()
        {
            IEnumerable<SelectListItem> countries = await _locationService.GetCountriesAsync();

            TenantFormViewModel model = new TenantFormViewModel
            {
                Countries = countries,
                States = Enumerable.Empty<SelectListItem>()
            };

            return View(model);
        }

        /// <summary>
        /// Handles the submission of the add tenant form.
        /// </summary>
        /// <param name="model">The tenant form data.</param>
        /// <returns>Redirects to the tenant list or redisplays the form if invalid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTenant(TenantFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _tenantService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the form to update an existing tenant.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant.</param>
        /// <returns>A view for updating the tenant or NotFound if not found.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateTenant(Guid id)
        {
            TenantFormViewModel model = await _tenantService.GetEditModelAsync(id);
            if (model == null)
                return NotFound();

            return PartialView("_EditTenantPartial", model);
        }

        /// <summary>
        /// Handles the submission of the update tenant form.
        /// </summary>
        /// <param name="model">The tenant form data.</param>
        /// <returns>Redirects to the tenant list or redisplays the form if invalid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTenant(TenantFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await _locationService.GetCountriesAsync();
                model.States = await _locationService.GetStatesByCountryAsync(model.CountryID);

                return PartialView("_EditTenantPartial", model);
            }

            await _tenantService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }

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
