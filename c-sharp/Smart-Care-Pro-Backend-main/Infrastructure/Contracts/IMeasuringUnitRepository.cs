using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IMeasuringUnitRepository : IRepository<MeasuringUnit>
    {
        /// <summary>
        /// The method is used to get a MeasuringUnit by key.
        /// </summary>
        /// <param name="key">Primary key of the table MeasuringUnits.</param>
        /// <returns>Returns a MeasuringUnit if the key is matched.</returns>
        public Task<MeasuringUnit> GetMeasuringUnitByKey(int key);

        /// <summary>
        /// The method is used to get the list of measuringUnit.
        /// </summary>
        /// <returns>Returns a list of all MeasuringUnits.</returns>
        public Task<IEnumerable<MeasuringUnit>> GetMeasuringUnits();

        public Task<IEnumerable<MeasuringUnit>> GetMeasuringUnitByTest(int key);

        /// <summary>
        /// The method is used to get an MeasuringUnit by MeasuringUnit Description.
        /// </summary>
        /// <param name="measuringUnit">Description of an MeasuringUnit.</param>
        /// <returns>Returns an MeasuringUnit if the MeasuringUnit name is matched.</returns>
        public Task<MeasuringUnit> GetMeasuringUnitByName(string measuringUnit);
    }
}