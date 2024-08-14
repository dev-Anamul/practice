using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by: Sphinx(2)
 * Date created: 12.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IRiskAssessmentRepository interface.
    /// </summary>
    public class RiskAssessmentRepository : Repository<RiskAssessment>, IRiskAssessmentRepository
    {
        public RiskAssessmentRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get risk assessment by HTSID.
        /// </summary>
        /// <param name="HTSID">HTSID of risk assessment.</param>
        /// <returns>Returns risk assessment if the HTSID is matched.</returns>
        public async Task<IEnumerable<RiskAssessment>> GetRiskAssessmentByHTS(Guid HTSID)
        {
            try
            {
                return await QueryAsync(r => r.IsDeleted == false && r.HTSId == HTSID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get risk assessment by key.
        /// </summary>
        /// <param name="key">Primary key of the table RiskAssessments.</param>
        /// <returns>Returns risk assessment if the key is matched.</returns>
        public async Task<RiskAssessment> GetRiskAssessmentByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(r => r.Oid == key && r.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of risk assessments.
        /// </summary>
        /// <returns>Returns a list of all risk assessments.</returns>
        public async Task<IEnumerable<RiskAssessment>> GetRiskAssessments()
        {
            try
            {
                return await QueryAsync(r => r.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}