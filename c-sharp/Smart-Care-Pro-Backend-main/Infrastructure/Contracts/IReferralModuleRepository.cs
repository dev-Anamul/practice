using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Infrastructure.Contracts
{
    public interface IReferralModuleRepository : IRepository<ReferralModule>
    {
        /// <summary>
        /// The method is used to get a referral module by key.
        /// </summary>
        /// <param name="key">Primary key of the table referral modules.</param>
        /// <returns>Returns a referral module if the key is matched.</returns>
        public Task<ReferralModule> GetReferralModuleByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of referral modules.
        /// </summary>
        /// <returns>Returns a list of all referral modules.</returns>
        public Task<IEnumerable<ReferralModule>> GetReferralModules();

        /// <summary>
        /// The method is used to get a referral module by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a referral module if the ClientID is matched.</returns>
        public Task<IEnumerable<ReferralModule>> GetReferralModuleByClient(Guid ClientID);
        public Task<IEnumerable<ReferralModule>> GetReferralModuleByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetReferralModuleByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a referral module by OPD visit.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a referral module if the Encounter is matched.</returns>
        public Task<IEnumerable<ReferralModule>> GetReferralModuleByEncounter(Guid EncounterID);
    }
}