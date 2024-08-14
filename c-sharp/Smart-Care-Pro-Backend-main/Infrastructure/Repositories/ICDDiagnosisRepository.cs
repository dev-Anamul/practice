using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bella
 * Date created : 04.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IICDDiagnosisRepository interface.
    /// </summary>
    public class ICDDiagnosisRepository : Repository<ICDDiagnosis>, IICDDiagnosisRepository
    {
        public ICDDiagnosisRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an ICD diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table ICDDiagnoses.</param>
        /// <returns>Returns an ICD diagnosis if the key is matched.</returns>
        public async Task<ICDDiagnosis> GetICDDiagnosisByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(i => i.Oid == key && i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ICD diagnoses.
        /// </summary>
        /// <returns>Returns a list of all ICD diagnoses.</returns>
        public async Task<IEnumerable<ICDDiagnosis>> GetICDDiagnoses()
        {
            try
            {
                return await QueryAsync(i => i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ICDDiagnosis>> GetICDDiagnosesBySearchTerm(string searchTerm)
        {
            try
            {
                return await context.ICDDiagnoses.AsNoTracking().Where(x => x.IsDeleted == false && (x.ICDCode.Contains(searchTerm) || x.Description.Contains(searchTerm))).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ICD diagnoses.
        /// </summary>
        /// <returns>Returns a list of all ICD diagnoses.</returns>
        public async Task<IEnumerable<ICDDiagnosis>> GetICDDiagnosesByICPC2(int id)
        {
            try
            {
                return await QueryAsync(i => i.IsDeleted == false && i.ICPC2Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an ICDDiagnosis by ICDDiagnosis Description.
        /// </summary>
        /// <param name="iCDDiagnosis">Name of an ICDDiagnosis.</param>
        /// <returns>Returns an ICDDiagnosis if the ICDDiagnosis description is matched.</returns>
        public async Task<ICDDiagnosis> GetICDDiagnosisByName(string iCDDiagnosis)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == iCDDiagnosis.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}