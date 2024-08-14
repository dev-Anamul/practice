using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Labib
 * Date created : 12.09.2022
 * Modified by  : Rezwana
 * Last modified: 06.11.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ICountryRepository interface.
    /// </summary>
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a country by country name.
        /// </summary>
        /// <param name="countryName">Name of a country.</param>
        /// <returns>Returns a county if the country name is matched.</returns>
        public async Task<Country> GetCountryByName(string countryName)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Description.ToLower().Trim() == countryName.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a country by key.
        /// </summary>
        /// <param name="key">Primary key of the table Countries.</param>
        /// <returns>Returns a country if the key is matched.</returns>
        public async Task<Country> GetCountryByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of countries.
        /// </summary>
        /// <returns>Returns a list of all countries.</returns>
        public async Task<IEnumerable<Country>> GetCountries()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}