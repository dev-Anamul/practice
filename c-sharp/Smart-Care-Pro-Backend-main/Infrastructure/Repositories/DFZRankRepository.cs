using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Stephan
 * Date created : 02.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DFZRankRepository : Repository<DFZRank>, IDFZRankRepository
    {
        public DFZRankRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of DFZ patient type.
        /// </summary>
        /// <param name="patientTypeId">Primary key of the table DFZ patient type.</param>
        /// <returns>Returns a list of all DFZ patient type.</returns>
        public async Task<IEnumerable<DFZRank>> GetDFZRankByPatientType(int patientTypeId)
        {
            try
            {
                var DFZRank = await QueryAsync(d => d.IsDeleted == false && d.PatientTypeId == patientTypeId, p => p.DFZPatientType);

                return DFZRank.OrderBy(d => d.Description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a DFZRank by key.
        /// </summary>
        /// <param name="key">Primary key of the table DFZRank.</param>
        /// <returns>Returns a DFZRank if the key is matched.</returns>
        public async Task<DFZRank> GetDFZRankByKey(int key)
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
        /// The method is used to get a DFZRank by DFZRank name.
        /// </summary>
        /// <param name="DFZRankName">Name of a DFZRank.</param>
        /// <returns>Returns a DFZRank if the DFZRank name is matched.</returns>
        public async Task<DFZRank> GetDFZRankByName(string DFZRankName)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Description.ToLower().Trim() == DFZRankName.ToLower().Trim() && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of DFZRank.
        /// </summary>
        /// <returns>Returns a list of all DFZRank.</returns>
        public async Task<IEnumerable<DFZRank>> GetDFZRanks()
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false, p => p.DFZPatientType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}