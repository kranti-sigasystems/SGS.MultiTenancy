
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface ILocationService
    {
        /// <summary>
        /// Get all active countries for dropdowns.
        /// Used to populate country selection in tenant forms.
        /// </summary>
        Task<IEnumerable<SelectListItem>> GetCountriesAsync();

        /// <summary>
        /// Get all active states for a specific country.
        /// Used to populate state selection based on selected country.
        /// </summary>
        Task<IEnumerable<SelectListItem>> GetStatesByCountryAsync(Guid countryId);
    }
}
