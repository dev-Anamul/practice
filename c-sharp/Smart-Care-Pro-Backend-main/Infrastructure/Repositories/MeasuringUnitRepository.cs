using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class MeasuringUnitRepository : Repository<MeasuringUnit>, IMeasuringUnitRepository
    {
        public MeasuringUnitRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a MeasuringUnit by key.
        /// </summary>
        /// <param name="key">Primary key of the table MeasuringUnits.</param>
        /// <returns>Returns a MeasuringUnit  if the key is matched.</returns>
        public async Task<MeasuringUnit> GetMeasuringUnitByKey(int key)
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
        /// The method is used to get a MeasuringUnit by key.
        /// </summary>
        /// <param name="key">Primary key of the table MeasuringUnits.</param>
        /// <returns>Returns a MeasuringUnit  if the key is matched.</returns>
        public async Task<IEnumerable<MeasuringUnit>> GetMeasuringUnitByTest(int key)
        {
            try
            {
                return await QueryAsync(d => d.TestId == key && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of MeasuringUnit.
        /// </summary>
        /// <returns>Returns a list of all covid MeasuringUnits.</returns>
        public async Task<IEnumerable<MeasuringUnit>> GetMeasuringUnits()
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
        /// The method is used to get an MeasuringUnit by MeasuringUnit Description.
        /// </summary>
        /// <param name="measuringUnit">Name of an MeasuringUnit.</param>
        /// <returns>Returns an MeasuringUnit if the MeasuringUnit description is matched.</returns>
        public async Task<MeasuringUnit> GetMeasuringUnitByName(string measuringUnit)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == measuringUnit.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}