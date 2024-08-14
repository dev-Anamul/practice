using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

/*
 * Created by   : Labib
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPriorARTExposerRepository : IRepository<PriorARTExposure>
    {
        /// <summary>
        /// The method is used to get a priorARTExposer by key.
        /// </summary>
        /// <param name="key">Primary key of the table PriorARTExposers.</param>
        /// <returns>Returns a priorARTExposer if the key is matched.</returns>
        public Task<PriorARTExposure> GetPriorARTExposerByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of priorARTExposers.
        /// </summary>
        /// <returns>Returns a list of all priorARTExposers.</returns>
        public Task<IEnumerable<PriorARTExposure>> GetPriorARTExposers();

        /// <summary>
        /// The method is used to get a priorARTExposer by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a priorARTExposer if the ClientID is matched.</returns>
        public Task<IEnumerable<PriorARTExposure>> GetPriorARTExposerByClient(Guid ClientID);
        public Task<IEnumerable<PriorARTExposure>> GetPriorARTExposerByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetPriorARTExposerByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of priorARTExposer by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all priorARTExposer by EncounterID.</returns>
        public Task<IEnumerable<PriorARTExposure>> GetPriorARTExposerByEncounter(Guid EncounterID);
    }
}