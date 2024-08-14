using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by    : Bithy
 * Date created  : 18.02.2023
 * Modified by   : Biplob Roy
 * Last modified : 30.07.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    public class CovidSymptomRepository : Repository<CovidSymptom>, ICovidSymptomRepository
    {
        public CovidSymptomRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of CovidSymptom.
        /// </summary>
        /// <returns>Returns a list of all covid CovidSymptoms.</returns>
        public async Task<IEnumerable<CovidSymptom>> GetCovidSymptoms()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a CovidSymptom by key.
        /// </summary>
        /// <param name="key">Primary key of the table CovidSymptoms.</param>
        /// <returns>Returns a CovidSymptom  if the key is matched.</returns>
        public async Task<CovidSymptom> GetCovidSymptomByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an CovidSymptom by CovidSymptom Description.
        /// </summary>
        /// <param name="description">Name of an CovidSymptom.</param>
        /// <returns>Returns an CovidSymptom if the CovidSymptom description is matched.</returns>
        public async Task<CovidSymptom> GetCovidSymptomByName(string covidSymptom)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == covidSymptom.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}