using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ITenantProvider _tenantProvider;

        public RoleController(IRoleService roleService , ITenantProvider tenantProvider)
        {
            _roleService = roleService;
            _tenantProvider = tenantProvider;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;

            List<Permission> permissions = await _roleService.GetAllPermissionsAsync();
            List<Role> roles = await _roleService.GetRolesByTenantAsync(tenantId);

            ViewBag.Permissions = permissions;
            ViewBag.Roles = roles;

            return View(new CreateRoleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                List<Permission> permissions = await _roleService.GetAllPermissionsAsync();
                ViewBag.Permissions = permissions;
                return View("Index", model);
            }

            Guid tenantId = (Guid)_tenantProvider.TenantId!;

            await _roleService.CreateRoleAsync(model, tenantId);

            return RedirectToAction("Index");
        }
    }
}