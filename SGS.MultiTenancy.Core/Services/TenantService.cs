using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Application.DTOs.Tenants;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

namespace SGS.MultiTenancy.Core.Services
{
    /// <summary>
    /// Service implementation for managing tenants using generic repositories.
    /// </summary>
    public class TenantService : ITenantService
    {
        private readonly IGenericRepository<Tenant> _tenantRepo;
        private readonly IGenericRepository<UserRoles> _userRolesRepo;
        private readonly IUserService _userService;
        private readonly IFileStorageRepository _fileStorageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantService"/> class.
        /// </summary>
        public TenantService(IGenericRepository<Tenant> tenantRepo, IUserService userService, IGenericRepository<UserRoles> userRolesRepo, IFileStorageRepository fileStorageRepository)
        {
            _tenantRepo = tenantRepo;
            _userService = userService;
            _userRolesRepo = userRolesRepo;
            _fileStorageRepository = fileStorageRepository;
        }

        /// <summary>
        /// Gets list of tenant.
        /// </summary>
        /// <returns>A list of active tenants.</returns>
        public async Task<List<TenantDto>> GetAllAsync()
        {
            return await _tenantRepo.Query()
                .Select(t => new TenantDto
                {
                    ID = t.ID,
                    Name = t.Name,
                    Slug = t.Slug,
                    Domain = t.Domain,
                    Status = t.Status,
                    RegistrationNumber = t.RegistrationNumber,
                    LogoUrl = t.LogoUrl
                })
                .ToListAsync();
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
        public async Task CreateAsync(TenantDto model)
        {
            bool slugExists = await _tenantRepo.AnyAsync(t => t.Slug == model.Slug);
            if (slugExists)
                throw new Exception("Slug already exists");

            Guid tenantId = Guid.NewGuid();
            string bussinessLogoPath = await _fileStorageRepository.SaveAsync(model.BusinessLogo, tenantId.ToString());
            Tenant tenant = new Tenant
            {
                ID = tenantId,
                Name = model.Name,
                Slug = model.Slug.ToLower(),
                Domain = model.Domain,
                Status = EntityStatus.Active,
                LogoUrl = bussinessLogoPath,
                RegistrationNumber = model.RegistrationNumber,
                CreateOn = DateTime.UtcNow
            };

            Tenant tenantResult = await _tenantRepo.AddAsync(tenant);
            await _tenantRepo.CompleteAsync();
            Guid userID = Guid.NewGuid();
            model.UserDto.Status = EntityStatus.Active;
            model.UserDto.TenantId = tenant.ID;
            model.UserDto.RoleIds.Add(Guid.Parse(Constants.TenantRoleId));
            UserDto userResult = await _userService.AddUserAsync(model.UserDto);
        }

        /// <summary>
        /// Retrieves the edit model for a tenant.
        /// </summary>
        /// <param name="tenantId">The tenant ID.</param>
        /// <returns>A populated tenant form view model.</returns>
        public async Task<TenantDto> GetEditModelAsync(Guid tenantId)
        {
            TenantDto? tenant = await _tenantRepo.Query()
                .Where(t => t.ID == tenantId)
                .Select(t => new TenantDto
                {
                    ID = t.ID,
                    Name = t.Name,
                    Slug = t.Slug,
                    Domain = t.Domain,
                    Status = t.Status,
                    LogoUrl = t.LogoUrl
                })
                .FirstOrDefaultAsync();
            if (tenant == null)
                throw new Exception("Tenant not found");

            return tenant;
        }

        /// <summary>
        /// Updates an existing tenant with new form data.
        /// </summary>
        /// <param name="model">The tenant form view model.</param>
        public async Task UpdateAsync(TenantDto model)
        {
            Tenant tenant = await _tenantRepo.Query()
                .FirstOrDefaultAsync(t => t.ID == model.ID)
                ?? throw new Exception("Tenant not found");

            bool slugExists = await _tenantRepo.AnyAsync(t =>
                t.Slug == model.Slug && t.ID != model.ID);

            if (slugExists)
                throw new Exception("Slug already exists");

            if (!string.IsNullOrEmpty(model.Domain))
            {
                bool domainExists = await _tenantRepo.AnyAsync(t =>
                    t.Domain == model.Domain && t.ID != model.ID);

                if (domainExists)
                    throw new Exception("Domain already mapped to another tenant");
            }
            if (tenant.LogoUrl == null)
            {
                model.LogoUrl = await _fileStorageRepository.SaveAsync(model.BusinessLogo, model.ID.ToString());
            }
            else
            {
                bool fileDeteled = _fileStorageRepository.DeleteAsync(tenant.LogoUrl!);
                if (fileDeteled)
                    model.LogoUrl = await _fileStorageRepository.SaveAsync(model.BusinessLogo, model.ID.ToString());
            }
            tenant.Name = model.Name;
            tenant.Slug = model.Slug.ToLower();
            tenant.Domain = model.Domain;
            tenant.Status = model.Status;
            tenant.LogoUrl = model.LogoUrl;
            tenant.LastUpdateOn = DateTime.UtcNow;

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