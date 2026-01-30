using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGS.MultiTenancy.Core.Application.DTOs.Tenants;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Attribute;

namespace SGS.MultiTenancy.UI.Controllers
{
    [Authorize (Roles = "SGS_SuperHost")]
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
        [HttpGet]
        [HasPermission(permissionId:Permissions.Tenant_View)]
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
            // Simulate a SQL timeout
            throw new TimeoutException("Simulated database timeout for testing middleware.");
            return View(new TenantDto());
        }

        /// <summary>
        /// Handles the submission of the add tenant form.
        /// </summary>
        /// <param name="model">The tenant form data.</param>
        /// <returns>Redirects to the tenant list or redisplays the form if invalid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            bool deleted = await _tenantService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
