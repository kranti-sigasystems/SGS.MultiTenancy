using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.DTOs.Permission;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Application.Pagination;
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

        public async Task<IActionResult> Index(
               string? searchTerm,
               string? groupName,
               int pageNumber = 1,
               int pageSize = 10)
        {
            Guid tenantId = (Guid)_tenantProvider.TenantId!;

            PaginationParams paginationParams = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            PagedResult<PermissionListDto> result =
                await _permissionService.GetPermissionsPagedAsync(
                    tenantId,
                    paginationParams,
                    searchTerm,
                    groupName);

            PagedListViewModel<PermissionListDto> vm = new()
            {
                Data = result,
                SearchTerm = searchTerm,
            };

            return View(vm);
        }

        /// <summary>
        /// Create page form.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new PermissionCreateDto());
        }

        /// <summary>
        /// Create permission.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(PermissionCreateDto model)
        {
            model.Code = model.Code.ToLower();
            model.TenantId = (Guid)_tenantProvider.TenantId!;
            await _permissionService.CreatePermissionAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
            PermissionItemDto permission = await _permissionService.GetPermissionById(Id);
            string[] parts = permission.Code!.Split('.', 2);
            PermissionUpdateDto model = new PermissionUpdateDto
            {
                Code = permission.Code,
                Description = permission.Description,
                Group = parts[0],
                Action = parts[1],
                ID = permission.Id,
                TenantId = permission.TenantId
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(PermissionUpdateDto updateDto)
        {
            await _permissionService.UpdatePermissionAsync(updateDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _permissionService.DeletePermissionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
