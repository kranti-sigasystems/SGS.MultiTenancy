using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs.Tenants;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Entities.Common;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System.Linq.Expressions;

namespace SGS.MultiTenancy.Core.Services
{
    /// <summary>
    /// Service implementation for managing tenants using generic repositories.
    /// </summary>
    public class TenantService : ITenantService
    {
        private readonly IGenericRepository<Tenant> _tenantRepo;
        private readonly IGenericRepository<State> _stateRepo;
        private readonly ILocationService _locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantService"/> class.
        /// </summary>
        public TenantService(
            IGenericRepository<Tenant> tenantRepo,
            IGenericRepository<State> stateRepo,
            ILocationService locationService)
        {
            _tenantRepo = tenantRepo;
            _stateRepo = stateRepo;
            _locationService = locationService;
        }

        /// <summary>
        /// Retrieves all active tenants.
        /// </summary>
        /// <returns>A list of active tenants.</returns>
        public Task<List<Tenant>> GetAllAsync()
        {
            return _tenantRepo.ListAsync(t => t.Status == EntityStatus.Active);
        }

        /// <summary>
        /// Retrieves a paginated list of active tenants.
        /// </summary>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>A paged result containing tenants and pagination metadata.</returns>
        public async Task<PagedResult<Tenant>> GetPagedTenantsAsync(int page, int pageSize)
        {
            Expression<Func<Tenant, bool>> filter = t => t.Status == EntityStatus.Active;

            var tenants = await _tenantRepo
                .GetPaged(filter, page, pageSize)
                .ToListAsync();

            var totalCount = await _tenantRepo
                .CountAsync(filter);

            return new PagedResult<Tenant>
            {
                Items = tenants,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Retrieves a tenant by its unique identifier.
        /// </summary>
        /// <param name="id">The tenant ID.</param>
        /// <returns>The tenant if found; otherwise null.</returns>
        public Task<Tenant?> GetByIdAsync(Guid id)
        {
            return _tenantRepo.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new tenant based on the provided form data.
        /// </summary>
        /// <param name="model">The tenant form view model.</param>
        public async Task CreateAsync(TenantFormViewModel model)
        {
            Tenant tenant = new Tenant
            {
                ID = Guid.NewGuid(),
                Name = model.Name,
                BussinessName = model.BussinessName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Status = EntityStatus.Active,
                CreateBy = model.ID,//temporary used tenantID as CreateBy Id
                Address = new Address
                {
                    ID = Guid.NewGuid(),
                    AddressLine = model.AddressLine,
                    PostalCode = model.PostalCode,
                    City = model.City,
                    StateID = model.StateID
                }
            };

            await _tenantRepo.AddAsync(tenant);
            await _tenantRepo.CompleteAsync();
        }

        /// <summary>
        /// Retrieves the edit model for a tenant.
        /// </summary>
        /// <param name="tenantId">The tenant ID.</param>
        /// <returns>A populated tenant form view model.</returns>
        public async Task<TenantFormViewModel> GetEditModelAsync(Guid tenantId)
        {
            Tenant? tenant = await _tenantRepo.Query()
                .Include(t => t.Address)
                .ThenInclude(a => a.State)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(t => t.ID == tenantId);

            if (tenant == null)
                throw new Exception("Tenant not found");

            if (tenant.Address == null || tenant.Address.State == null)
                throw new Exception("Tenant address or state is missing");

            var countries = await _locationService.GetCountriesAsync();
            var states = await _locationService
                .GetStatesByCountryAsync(tenant.Address.State.CountryID);

            return new TenantFormViewModel
            {
                ID = tenant.ID,
                AddressID = tenant.AddressID,

                Name = tenant.Name,
                BussinessName = tenant.BussinessName,
                Email = tenant.Email,
                PhoneNumber = tenant.PhoneNumber,
                Status = tenant.Status,

                AddressLine = tenant.Address.AddressLine,
                City = tenant.Address.City,
                PostalCode = tenant.Address.PostalCode,
                StateID = tenant.Address.StateID,
                CountryID = tenant.Address.State.CountryID,

                Countries = countries,
                States = states
            };
        }

        /// <summary>
        /// Updates an existing tenant with new form data.
        /// </summary>
        /// <param name="model">The tenant form view model.</param>
        public async Task UpdateAsync(TenantFormViewModel model)
        {
            Tenant? tenant = await _tenantRepo.Query()
                .Include(t => t.Address)
                .FirstOrDefaultAsync(t => t.ID == model.ID) ?? throw new Exception("Tenant not found");

            if (tenant.Address == null)
                throw new Exception("Address not found");

            if (!await _stateRepo.AnyAsync(s => s.ID == model.StateID))
                throw new Exception("Invalid State");

            tenant.Name = model.Name;
            tenant.BussinessName = model.BussinessName;
            tenant.Email = model.Email;
            tenant.PhoneNumber = model.PhoneNumber;
            tenant.Status = model.Status;
            tenant.UpdateBy = tenant.ID; //temporary used tenantID as UpdateBy Id
            tenant.Address.AddressLine = model.AddressLine;
            tenant.Address.PostalCode = model.PostalCode;
            tenant.Address.City = model.City;
            tenant.Address.StateID = model.StateID;

            await _tenantRepo.UpdateAsync(tenant);
            await _tenantRepo.CompleteAsync();
        }

        /// <summary>
        /// Deletes a tenant by its unique identifier.
        /// </summary>
        /// <param name="id">The tenant ID.</param>
        /// <returns>True if deleted successfully; otherwise false.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            Tenant? tenant = await _tenantRepo.GetByIdAsync(id);

            if (tenant == null) return false;

            tenant.Status = EntityStatus.Inactive;

            await _tenantRepo.UpdateAsync(tenant);
            await _tenantRepo.CompleteAsync();

            return true;
        }
    }
}
