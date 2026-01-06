using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SGS.MultiTenancy.Core.Domain.Common;
using System.Reflection;

namespace SGS.MultiTenancy.Infra.DataContext
{
    /// <summary>
    /// Extension methods for applying tenant-based query filters.
    /// </summary>
    public static class ModelBuilderTenantExtensions
    {
        /// <summary>
        /// Applies a global tenant filter to all entities containing a TenantID.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="ModelBuilder"/> used to configure the EF Core model.
        /// </param>
        /// <param name="tenantId">
        /// The current tenant identifier.
        /// </param>
        public static void ApplyTenantFilter(
            this ModelBuilder modelBuilder,
            Guid tenantId)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType == null)
                    continue;

                IMutableProperty? tenantProperty =
                    entityType.FindProperty(Constants.TenantID);

                if (tenantProperty?.ClrType == typeof(Guid))
                {
                    MethodInfo method = typeof(ModelBuilderTenantExtensions)
                        .GetMethod(nameof(SetTenantFilter),
                            BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { modelBuilder, tenantId });
                }
            }
        }

        /// <summary>
        /// Applies a tenant filter to the given entity type.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="ModelBuilder"/> used to configure the EF Core model.
        /// </param>
        /// <param name="tenantId">
        /// The current tenant identifier.
        /// </param>
        private static void SetTenantFilter<TEntity>(
            ModelBuilder modelBuilder,
            Guid tenantId)
            where TEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(e =>
                    tenantId == Guid.Empty ||
                    EF.Property<Guid>(e, Constants.TenantID) == tenantId);
        }
    }
}
