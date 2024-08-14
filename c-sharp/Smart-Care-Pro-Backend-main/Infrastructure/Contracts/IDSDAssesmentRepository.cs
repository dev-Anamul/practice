using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Tomas
 * Date created : 11.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDSDAssesmentRepository : IRepository<DSDAssessment>
    {
        /// <summary>
        /// The method is used to get an DSD Assesment by key.
        /// </summary>
        /// <param name="key">Primary key of the table DSDAssesments.</param>
        /// <returns>Returns an DSDAssesment if the key is matched.</returns>
        public Task<DSDAssessment> GetDSDAssesmentByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of DSDAssesments.
        /// </summary>
        /// <returns>Returns a list of all DSDAssesments.</returns>
        public Task<IEnumerable<DSDAssessment>> GetDSDAssesments();

        /// <summary>
        /// The method is used to get an DSDAssesment by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns DSDAssesment if the ClientID is matched.</returns>
        public Task<IEnumerable<DSDAssessment>> GetDSDAssesmentByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get the list of DSDAssesment by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all DSDAssesment by EncounterID.</returns>
        public Task<IEnumerable<DSDAssessment>> GetDSDAssesmentByEncounter(Guid EncounterID);
    }
}