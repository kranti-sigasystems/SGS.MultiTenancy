using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        /// <summary>
        /// Displays the list of all tenants.
        /// </summary>
        /// <returns>A view displaying the list of tenants.</returns>
        [Authorize(Roles = "SGS_SuperHost")]
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
        [Authorize(Roles = "SGS_SuperHost")]
        public async Task<IActionResult> AddTenant()
        {
            return View(new TenantDto());
        }

        /// <summary>
        /// Handles the submission of the add tenant form.
        /// </summary>
        /// <param name="model">The tenant form data.</param>
        /// <returns>Redirects to the tenant list or redisplays the form if invalid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SGS_SuperHost")]
        public async Task<IActionResult> AddTenant(TenantDto model)
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
        [Authorize(Roles = "SGS_SuperHost")]
        public async Task<IActionResult> UpdateTenant(Guid id)
        {
            TenantDto tenant = await _tenantService.GetEditModelAsync(id);
            if (tenant == null)
                return NotFound();

            return PartialView("_EditTenantPartial", tenant);
        }
        /// <summary>
        /// Handles the submission of the update tenant form.
        /// </summary>
        /// <param name="model">The tenant form data.</param>
        /// <returns>Redirects to the tenant list or redisplays the form if invalid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SGS_SuperHost")]
        public async Task<IActionResult> UpdateTenant(TenantDto model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditTenantPartial", model);
            }

            await _tenantService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SGS_SuperHost")]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            bool deleted = await _tenantService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// serving the form for the tenant identity
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Discovery()
        {
            return View();
        }
        /// <summary>
        /// serving the form for the tenant identity
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Discovery(TenantDiscoveryViewModel model)
        {
            string NoramalizedTenantName = model.TenantName.Trim();
            TenantDto? tenant = await _tenantService.GetTenantByNameAsync(NoramalizedTenantName);
            if (tenant == null)
            {
                // Generic message, no leakage
                ModelState.AddModelError("", "Unable to find workspace");
                return View();
            }
            var scheme = Request.Scheme;
            var host = Request.Host.Host;
            var port = Request.Host.Port;
            var loginPath = "/Auth/Login";

            var tenantHost = $"{tenant.Slug}.{host}";

            var loginUrl = port.HasValue
                ? $"{scheme}://{tenantHost}:{port}{loginPath}"
                : $"{scheme}://{tenantHost}{loginPath}";

            return Redirect(loginUrl);
        }
    }
}