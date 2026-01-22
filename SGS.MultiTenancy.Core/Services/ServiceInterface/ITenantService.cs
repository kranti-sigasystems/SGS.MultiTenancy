using SGS.MultiTenancy.Core.Application.DTOs.Tenants;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface ITenantService
    {
        /// <summary>
        /// Retrieves all active tenants.
        /// Used to display the tenant list in the UI.
        /// </summary>
        Task<List<TenantDto>> GetAllAsync();

        /// <summary>
        /// Gets a tenant by its unique identifier.
        /// Useful for details or edit operations.
        /// </summary>
        Task<Tenant?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates a new tenant with address details.
        /// Typically called from the add tenant form.
        /// </summary>
        Task CreateAsync(TenantDto model);

        /// <summary>
        /// Gets tenant data for editing.
        /// Includes preselected country and state for dropdowns.
        /// </summary>
        Task<TenantDto> GetEditModelAsync(Guid tenantId);

        /// <summary>
        /// Updates an existing tenant and its address.
        /// Used when submitting the edit tenant form.
        /// </summary>
        Task UpdateAsync(TenantDto model);

        /// <summary>
        /// Deletes a tenant using soft delete by marking status.
        /// Ensures tenant is hidden from UI but kept in the database.
        /// </summary>
        Task<bool> DeleteAsync(Guid id);
    }
}
