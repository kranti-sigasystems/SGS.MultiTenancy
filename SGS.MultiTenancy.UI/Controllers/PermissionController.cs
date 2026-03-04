using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs.Permission;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{

        public class PermissionController : Controller
        {
            private readonly IPermissionService _permissionService;

            public PermissionController(IPermissionService permissionService)
            {
                _permissionService = permissionService;
            }

            public async Task<IActionResult> Index()
            {
            List<PermissionGroupDto> dto = await _permissionService.GetGroupedPermissionsAsync();

            List<PermissionGroupViewModel> model = dto.Select(g => new PermissionGroupViewModel
                {
                    GroupName = g.GroupName,
                    Permissions = g.Permissions.Select(p => new PermissionItemViewModel
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name
                    }).ToList()
                }).ToList();

                return View(model);
            }
        }
    }
