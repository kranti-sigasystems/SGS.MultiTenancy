using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.DTOs.Role;
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
            List<PermissionDto>? permissions = await _roleService.GetAllPermissionsAsync();

            CreateRoleViewModel model = new CreateRoleViewModel
            {
                PermissionList = permissions
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

            RoleCreateDto roleDto = new RoleCreateDto
            {
                Name = model.Name,
                Description = model.Description,
                SelectedPermissions = model.SelectedPermissions ?? new List<Guid>()
            };

            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            await _roleService.CreateRoleAsync(roleDto, tenantId);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Updates a role form.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;

            RoleDto roleDto = await _roleService.GetRolesByIdandTenantIdAsync(id, tenantId);
            List<PermissionDto>? permissions = await _roleService.GetAllPermissionsAsync();

            HashSet<Guid>? selectedPermissionIds = roleDto.RolePermissions.Select(rp => rp.PermissionID).ToHashSet() ?? new HashSet<Guid>();

            foreach (PermissionDto? permission in permissions)
            {
                permission.IsSelected = selectedPermissionIds.Contains(permission.ID);
            }

            CreateRoleViewModel model = new CreateRoleViewModel
            {
                Name = roleDto.Name,
                Description = roleDto.Description,
                SelectedPermissions = selectedPermissionIds.ToList(),
                PermissionList = permissions
            };
            return View(model);
        }

        /// <summary>
        /// Update Actioon method for updating a role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateRoleViewModel model)
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;
            ModelState.Remove(nameof(model.PermissionList));
            ModelState.Remove(nameof(model.RolesList));
            if (!ModelState.IsValid)
            {
                model.PermissionList = await _roleService.GetAllPermissionsAsync();
                HashSet<Guid>? selectedPermissionIds = model.SelectedPermissions.Select(sp => sp).ToHashSet() ?? new HashSet<Guid>();

                foreach (PermissionDto? permission in model.PermissionList)
                {
                    permission.IsSelected = selectedPermissionIds.Contains(permission.ID);
                }
                return View(model);
            }

            UpdateRoleDto roleDto = new UpdateRoleDto
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                SelectedPermissions = model.SelectedPermissions ?? new List<Guid>()
            };
            await _roleService.UpdateRoleAsync(roleDto, tenantId);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete method to delete role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Guid tenantId = _tenantProvider.TenantId.Value;
            await _roleService.DeleteAsync(id, tenantId);
            return RedirectToAction(nameof(Index));
        }
    }
}