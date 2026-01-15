using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;
using System.Security.Claims;
namespace SGS.MultiTenancy.UI.Controllers
{

    /// <summary>
    /// Provides endpoints for managing permissions
    /// </summary>
 
    [Authorize]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Displays the list of all permissions.
        /// </summary>
        /// <returns>A view displaying the list of permissions.</returns>
        public async Task<IActionResult> Index()
        {
          
            List<Permission> permissions = await _permissionService.GetPermissionsAsync();
            PermissionPageViewModel vm = new PermissionPageViewModel
            {
                Permissions = new PagedResult<PermissionsViewModel>
                {
                    Items = permissions.Select(p => new PermissionsViewModel
                    {
                        Id = p.ID,
                        Code = p.Code,
                        Description = p.Description
                    }).ToList(),
                },
                Form = new PermissionsViewModel()
            };
            return View(vm);
        }

        /// <summary>
        /// Handles the submission of the add permission form.
        /// </summary>
        /// <param name="model">The permission form data.</param>
        /// <returns>Redirects to the permission list or redisplays the form if invalid.</returns>
        public async Task<IActionResult> Create(PermissionPageViewModel model)
        {

            if (model.Form.Code == null || model.Form.Description == null)
            {
                return RedirectToAction("Index");
            }

            GenericRequestDto<Permission> permissionsRequestDto = new GenericRequestDto<Permission>
            {
                Data = new Permission
                {
                    Code = model.Form.Code,
                    Description = model.Form.Description,
                    CreateBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                }
            };
            GenericResponseDto<Permission> responce = await _permissionService.AddPermissionAsync(permissionsRequestDto);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Handles the submission of the update permission form.
        /// </summary>
        /// <param name="model">The permission form data.</param>
        /// <returns>Redirects to the permission list or redisplays the form if invalid.</returns>
        [HttpPost]
        public async Task<IActionResult> Update(PermissionPageViewModel model)
        {
            if (model.Form.Code == null || model.Form.Description == null)
            {
                return RedirectToAction("Index");
            }

            GenericRequestDto<Permission> permissionsRequestDto = new GenericRequestDto<Permission>
            {
                Data = new Permission
                {
                    Code = model.Form.Code!,
                    Description = model.Form.Description!,
                    UpdateBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    UpdateOn = DateTime.Now
                }
            };
            GenericResponseDto<Permission> response = await _permissionService.UpdatePermissionAsync((Guid)model.Form.Id!, permissionsRequestDto);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Handles Delete a permission based on the data provided id.
        /// </summary>
        /// <param name="model">The view model containing the permission information to be deleted. The model's Form.Id property must not be null.</param>
        /// <returns>A redirect to the Index action after attempting to delete the specified permission.</returns>
        /// 
        [HttpPost]
        public async Task<IActionResult> Delete(PermissionPageViewModel model)
        {

            if (model.Form.Id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                GenericResponseDto<Permission> response = await _permissionService.DeletePermissionAsync((Guid)model.Form.Id!);
            }
            return RedirectToAction("Index");
        }


    }
}