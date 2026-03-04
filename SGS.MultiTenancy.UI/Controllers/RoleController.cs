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
            List<RoleDto> roles = await _roleService.GetRolesByTenantAsync(tenantId);

            CreateRoleViewModel model = new CreateRoleViewModel
            {
                RolesList = roles
            };
            return View(model);
        }

        /// <summary>
        /// Handles GET requests for creating a new role.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // We need the list of permissions to show in the matrix
            List<PermissionDto>? permissions = await _roleService.GetAllPermissionsAsync();

            CreateRoleViewModel model = new CreateRoleViewModel
            {
                PermissionList = permissions // Assuming this is a List<PermissionDto>
            };
            return View(model);
        }

        /// <summary>
        /// Handles the creation of a new role for the current tenant.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            ModelState.Remove(nameof(model.PermissionList));
            ModelState.Remove(nameof(model.RolesList));
            if (!ModelState.IsValid)
            { 
                model.PermissionList = await _roleService.GetAllPermissionsAsync();
                return View(model);
            }

            var roleDto = new RoleCreateDto
            {
                Name = model.Name,
                Description = model.Description,
                SelectedPermissions = model.SelectedPermissions ?? new List<Guid>()
            };

            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            await _roleService.CreateRoleAsync(roleDto, tenantId);

            return RedirectToAction(nameof(Index));
        }
    }
}