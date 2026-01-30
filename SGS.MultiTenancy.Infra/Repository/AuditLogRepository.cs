using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Infra.DataContext;

namespace SGS.MultiTenancy.Infra.Repository
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        private readonly DbContextOptions<AppDbContext> _options;
        public AuditLogRepository(AppDbContext context, DbContextOptions<AppDbContext> options, ICurrentUser currentUser) : base(context, currentUser)
        {
            _options = options;
        }

        public async Task AddAuditLogAsync(AuditLog auditLog, ITenantProvider tenantProvider)
        {
            try
            {
                // Create a new independent DbContext for audit logging
                using var context = new AppDbContext(_options, tenantProvider);

                // Add audit log
                context.AuditLogs.Add(auditLog);

                // Save changes safely
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Fallback logging to console/file
                Console.WriteLine($"Audit log failed: {ex.Message}");
            }

        }
    }
}
