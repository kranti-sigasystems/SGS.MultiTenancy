
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

        /// <summary>
        /// Gets the name of the country by its identifier.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<string> GetCountryNameByIdAsync(string countryId);

        /// <summary>
        /// Gets the name of the state by its identifier.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        Task<string> GetStateNameByIdAsync(string stateId);
    }
}
