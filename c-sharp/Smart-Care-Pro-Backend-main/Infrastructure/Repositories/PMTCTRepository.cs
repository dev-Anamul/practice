using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 11.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PMTCTRepository : Repository<PMTCT>, IPMTCTRepository
    {
        public PMTCTRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PMTCT if the ClientID is matched.</returns>
        public async Task<IEnumerable<PMTCT>> GetPMTCTByClient(Guid ClientID)
        {
            try
            {
                return await context.PMTCTs.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
            .Join(
                context.Encounters.AsNoTracking(),
                pMTCT => pMTCT.EncounterId,
                encounter => encounter.Oid,
                (pMTCT, encounter) => new PMTCT
                {
                    EncounterId = pMTCT.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    CreatedIn = pMTCT.CreatedIn,
                    EncounterType = pMTCT.EncounterType,
                    DateModified = pMTCT.DateModified,
                    DateCreated = pMTCT.DateCreated,
                    CreatedBy = pMTCT.CreatedBy,
                    InteractionId = pMTCT.InteractionId,
                    IsDeleted = pMTCT.IsDeleted,
                    ARVEndDateForChild = pMTCT.ARVEndDateForChild,
                    ARVEndDateForMother = pMTCT.ARVEndDateForMother,
                    ARVStartDateForChild = pMTCT.ARVStartDateForChild,
                    ARVStartDateForMother = pMTCT.ARVStartDateForMother,
                    ClientId = pMTCT.ClientId,
                    DurationUnitForMother = pMTCT.DurationUnitForMother,
                    HasChildTakenARV = pMTCT.HasChildTakenARV,
                    HasMotherTakenARV = pMTCT.HasChildTakenARV,
                    HowLongChildTakenARV = pMTCT.HowLongChildTakenARV,
                    HowLongMotherTakenARV = pMTCT.HowLongMotherTakenARV,
                    IsSynced = pMTCT.IsSynced,
                    ModifiedBy = pMTCT.ModifiedBy,
                    ModifiedIn = pMTCT.ModifiedIn,
                    OtherARVForChild = pMTCT.OtherARVForChild,
                    OtherARVForMother = pMTCT.OtherARVForMother,
                    WhatARVChildWasTaken = pMTCT.WhatARVChildWasTaken,
                    WhatARVMotherWasTaken = pMTCT.WhatARVChildWasTaken,
                    WhenMotherTakenARV = pMTCT.WhenMotherTakenARV,


                }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PMTCT by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PMTCT by EncounterID.</returns>
        public async Task<IEnumerable<PMTCT>> GetPMTCTByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PMTCTs.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
            .Join(
                context.Encounters.AsNoTracking(),
                pMTCT => pMTCT.EncounterId,
                encounter => encounter.Oid,
                (pMTCT, encounter) => new PMTCT
                {
                    EncounterId = pMTCT.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    CreatedIn = pMTCT.CreatedIn,
                    EncounterType = pMTCT.EncounterType,
                    DateModified = pMTCT.DateModified,
                    DateCreated = pMTCT.DateCreated,
                    CreatedBy = pMTCT.CreatedBy,
                    InteractionId = pMTCT.InteractionId,
                    IsDeleted = pMTCT.IsDeleted,
                    ARVEndDateForChild = pMTCT.ARVEndDateForChild,
                    ARVEndDateForMother = pMTCT.ARVEndDateForMother,
                    ARVStartDateForChild = pMTCT.ARVStartDateForChild,
                    ARVStartDateForMother = pMTCT.ARVStartDateForMother,
                    ClientId = pMTCT.ClientId,
                    DurationUnitForMother = pMTCT.DurationUnitForMother,
                    HasChildTakenARV = pMTCT.HasChildTakenARV,
                    HasMotherTakenARV = pMTCT.HasChildTakenARV,
                    HowLongChildTakenARV = pMTCT.HowLongChildTakenARV,
                    HowLongMotherTakenARV = pMTCT.HowLongMotherTakenARV,
                    IsSynced = pMTCT.IsSynced,
                    ModifiedBy = pMTCT.ModifiedBy,
                    ModifiedIn = pMTCT.ModifiedIn,
                    OtherARVForChild = pMTCT.OtherARVForChild,
                    OtherARVForMother = pMTCT.OtherARVForMother,
                    WhatARVChildWasTaken = pMTCT.WhatARVChildWasTaken,
                    WhatARVMotherWasTaken = pMTCT.WhatARVChildWasTaken,
                    WhenMotherTakenARV = pMTCT.WhenMotherTakenARV,


                }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PMTCT by key.
        /// </summary>
        /// <param name="key">Primary key of the table PMTCTs.</param>
        /// <returns>Returns a PMTCT if the key is matched.</returns>
        public async Task<PMTCT> GetPMTCTByKey(Guid key)
        {
            try
            {
                var pMTCT = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (pMTCT != null)
                    pMTCT.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return pMTCT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PMTCTs.
        /// </summary>
        /// <returns>Returns a list of all PMTCT.</returns>
        public async Task<IEnumerable<PMTCT>> GetPMTCTs()
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
    }
}