using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.DTOs.Permission;
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

            PermissionResponseDto permissions = await _permissionService.GetPermissionsAsync();
            PermissionPageViewModel vm = new PermissionPageViewModel
            {
                Permissions = permissions.Permissions?
                 .Select(p => new PermissionsViewModel
                 {
                     Id = p.Id,
                     Code = p.Code,
                     Description = p.Description
                 })
                 .ToList()
                 ?? new List<PermissionsViewModel>(),
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

            PermissionRequestDto permissionsRequestDto = new PermissionRequestDto
            {
                Code = model.Form.Code,
                Description = model.Form.Description
            };
            PermissionResponseDto responce = await _permissionService.AddPermissionAsync(permissionsRequestDto);
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

            PermissionRequestDto permissionsRequestDto = new PermissionRequestDto
            {
                Code = model.Form.Code,
                Description = model.Form.Description,
                UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            };
            PermissionResponseDto responce = await _permissionService.UpdatePermissionAsync((Guid)model.Form.Id!, permissionsRequestDto);
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
                PermissionResponseDto responce = await _permissionService.DeletePermissionAsync((Guid)model.Form.Id!);
            }
            return RedirectToAction("Index");
        }
    }
}