using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IAuditLogService
    {
        /// <summary>
        /// Retrieves all active AuditLogs.
        /// Used to display the AuditLog list in the UI.
        /// </summary>
        Task<List<AuditLog>> GetAllAsync();

        /// <summary>
        /// Gets a AuditLog by its unique identifier.
        /// Useful for details or edit operations.
        /// </summary>
        Task<AuditLog?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates a new AuditLog with address details.
        /// Typically called from the add AuditLog form.
        /// </summary>
        Task CreateAsync(AuditLog model);

        /// <summary>
        /// Gets AuditLog data for editing.
        /// Includes preselected country and state for dropdowns.
        /// </summary>
        Task<AuditLog> GetEditModelAsync(Guid AuditLogId);

        /// <summary>
        /// Updates an existing AuditLog and its address.
        /// Used when submitting the edit AuditLog form.
        /// </summary>
        Task UpdateAsync(AuditLog model);

        /// <summary>
        /// Deletes a AuditLog using soft delete by marking status.
        /// Ensures AuditLog is hidden from UI but kept in the database.
        /// </summary>
        Task<bool> DeleteAsync(Guid id);
    }
}
