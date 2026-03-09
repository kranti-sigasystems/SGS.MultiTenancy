using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.DTOs.Permission;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly ITenantProvider _tenantProvider;
        public PermissionController(IPermissionService permissionService, ITenantProvider tenantProvider)
        {
            _permissionService = permissionService;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Gets a Index view with all permissions grouped by their group name.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            List<PermissionGroupDto> dto = await _permissionService.GetGroupedPermissionsAsync((Guid)_tenantProvider.TenantId!);

            List<PermissionGroupViewModel> model = dto.Select(g => new PermissionGroupViewModel
            {
                GroupName = g.GroupName,
                Permissions = g.Permissions.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    TenantId = p.TenantId
                }).ToList()
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new PermissionCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PermissionCreateDto model)
        {
            model.Code = model.Code.ToLower();
            model.TenantId = (Guid)_tenantProvider.TenantId!;
            await _permissionService.CreatePermissionAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
