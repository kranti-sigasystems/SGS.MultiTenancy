using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

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

        /// <summary>
        /// Retrieves a list of active countries formatted for use in selection controls.
        /// </summary>
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

        /// <summary>
        /// Retrieves a list of active states based on the country identifier formatted for use in selection controls.
        /// </summary>
        /// <param name="countryId"></param>
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

        /// <summary>
        /// Retrieves the name of a country based on its unique identifier.
        /// </summary>
        /// <param name="countryId"></param>
        public async Task<string> GetCountryNameByIdAsync(string countryId)
        {
            return await _countryRepo.Query().Where(c => c.ID.ToString() == countryId).Select(c => c.Name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves the name of a country based on its unique identifier.
        /// </summary>
        public async Task<string> GetStateNameByIdAsync(string stateId)
        {
            return await _stateRepo.Query().Where(s => s.ID.ToString() == stateId).Select(s => s.Name).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}