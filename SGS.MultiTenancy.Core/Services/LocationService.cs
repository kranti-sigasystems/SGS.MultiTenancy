using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SGS.MultiTenancy.Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly IGenericRepository<Country> _countryRepo;
        private readonly IGenericRepository<State> _stateRepo;

        public LocationService(
            IGenericRepository<Country> countryRepo,
            IGenericRepository<State> stateRepo)
        {
            _countryRepo = countryRepo;
            _stateRepo = stateRepo;
        }

        public async Task<IEnumerable<SelectListItem>> GetCountriesAsync()
        {
            return await _countryRepo.Query()
                .Where(c => c.Status == EntityStatus.Active)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.ID.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }
        
        public async Task<IEnumerable<SelectListItem>> GetStatesByCountryAsync(Guid countryId)
        {
            return await _stateRepo.Query()
                .Where(s => s.CountryID == countryId)
                .OrderBy(s => s.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.Name
                })
                .ToListAsync()
                .ConfigureAwait(false);
        }

    }
}
