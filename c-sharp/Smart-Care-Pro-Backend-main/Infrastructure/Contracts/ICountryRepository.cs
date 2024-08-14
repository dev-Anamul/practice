using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ICountryRepository : IRepository<Country>
    {
        /// <summary>
        /// The method is used to get a country by country name.
        /// </summary>
        /// <param name="countryName">Name of a country.</param>
        /// <returns>Returns a county if the country name is matched.</returns>
        public Task<Country> GetCountryByName(string countryName);

        /// <summary>
        /// The method is used to get a country by key.
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Returns a country if the key is matched.</returns>
        public Task<Country> GetCountryByKey(int key);

        /// <summary>
        /// The method is used to get the list of countries.
        /// </summary>
        /// <returns>Returns a list of all countries.</returns>
        public Task<IEnumerable<Country>> GetCountries();
    }
}