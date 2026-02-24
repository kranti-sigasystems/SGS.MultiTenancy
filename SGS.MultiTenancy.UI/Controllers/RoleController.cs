using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.Web.Controllers
{
    /// <summary>
    /// MVC controller responsible for managing roles within a tenant.
    /// </summary>
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        public RoleController(IRoleService roleService, ITenantProvider tenantProvider)
        {
            _roleService = roleService;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Displays the role management page for the current tenant.
        /// </summary>        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;

            List<PermissionDto> permissions = await _roleService.GetAllPermissionsAsync();
            List<RoleDto> roles = await _roleService.GetRolesByTenantAsync(tenantId);
          
            CreateRoleViewModel model = new CreateRoleViewModel
            {
                RolesList = roles,
                PermissionList = permissions
            };
            return View(model);
        }

        /// <summary>
        /// Handles the creation of a new role for the current tenant.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                List<PermissionDto> permissions = await _roleService.GetAllPermissionsAsync();
                model.PermissionList= permissions;
                return View("Index", model);
            }
            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            await _roleService.CreateRoleAsync(model, tenantId);
            return RedirectToAction("Index");
        }
    }
}