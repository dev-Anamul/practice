using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IRiskAssessmentRepository : IRepository<RiskAssessment>
    {
        /// <summary>
        /// The method is used to get risk assessment by HTSID.
        /// </summary>
        /// <param name="HTSID">HTSID of risk assessment.</param>
        /// <returns>Returns risk assessment if the HTSID is matched.</returns>
        public Task<IEnumerable<RiskAssessment>> GetRiskAssessmentByHTS(Guid HTSID);

        /// <summary>
        /// The method is used to get risk assessment by key.
        /// </summary>
        /// <param name="key">Primary key of the table RiskAssessments.</param>
        /// <returns>Returns risk assessment if the key is matched.</returns>
        public Task<RiskAssessment> GetRiskAssessmentByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of risk assessments.
        /// </summary>
        /// <returns>Returns a list of all risk assessments.</returns>
        public Task<IEnumerable<RiskAssessment>> GetRiskAssessments(); 
    }
}