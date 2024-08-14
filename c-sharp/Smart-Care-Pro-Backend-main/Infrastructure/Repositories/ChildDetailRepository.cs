using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ChildDetailRepository : Repository<ChildDetail>, IChildDetailRepository
    {
        public ChildDetailRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a ChildDetail if the ClientID is matched.</returns>
        public async Task<IEnumerable<ChildDetail>> GetChildDetailByClient(Guid ClientID)
        {
            try
            {
                return await context.ChildDetails.AsNoTracking().Where(x => x.ClientId == ClientID && x.IsDeleted == false).
                    Join(
                        context.Encounters.AsNoTracking(),
                        childDetails => childDetails.EncounterId,
                        encounter => encounter.Oid,
                        (childDetails, encounter) => new ChildDetail
                        {
                            // Properties from ChildDetail
                            InteractionId = childDetails.InteractionId,
                            BirthOutcome = childDetails.BirthOutcome,
                            BirthWeight = childDetails.BirthWeight,
                            ChildName = childDetails.ChildName,
                            ChildSex = childDetails.ChildSex,
                            LastTCDate = childDetails.LastTCDate,
                            LastTCResult = childDetails.LastTCResult,
                            ChildCareNumber = childDetails.ChildCareNumber,
                            DateOfChildTurns18Months = childDetails.DateOfChildTurns18Months,
                            ClientId = childDetails.ClientId,
                            Client = childDetails.Client,
                            ChildDetails = childDetails.ChildDetails,

                            // Properties from EncounterBaseModel
                            EncounterId = childDetails.EncounterId,
                            EncounterType = childDetails.EncounterType,
                            CreatedIn = childDetails.CreatedIn,
                            DateCreated = childDetails.DateCreated,
                            CreatedBy = childDetails.CreatedBy,
                            ModifiedIn = childDetails.ModifiedIn,
                            DateModified = childDetails.DateModified,
                            ModifiedBy = childDetails.ModifiedBy,
                            IsDeleted = childDetails.IsDeleted,
                            IsSynced = childDetails.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == childDetails.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == childDetails.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ChildDetail by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ChildDetail by EncounterID.</returns>
        public async Task<IEnumerable<ChildDetail>> GetChildDetailByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.ChildDetails.AsNoTracking().Where(x => x.EncounterId == EncounterID && x.IsDeleted == false).
                     Join(
                         context.Encounters.AsNoTracking(),
                         childDetails => childDetails.EncounterId,
                         encounter => encounter.Oid,
                         (childDetails, encounter) => new ChildDetail
                         {
                             // Properties from ChildDetail
                             InteractionId = childDetails.InteractionId,
                             BirthOutcome = childDetails.BirthOutcome,
                             BirthWeight = childDetails.BirthWeight,
                             ChildName = childDetails.ChildName,
                             ChildSex = childDetails.ChildSex,
                             LastTCDate = childDetails.LastTCDate,
                             LastTCResult = childDetails.LastTCResult,
                             ChildCareNumber = childDetails.ChildCareNumber,
                             DateOfChildTurns18Months = childDetails.DateOfChildTurns18Months,
                             ClientId = childDetails.ClientId,
                             Client = childDetails.Client,
                             ChildDetails = childDetails.ChildDetails,

                             // Properties from EncounterBaseModel
                             EncounterId = childDetails.EncounterId,
                             EncounterType = childDetails.EncounterType,
                             CreatedIn = childDetails.CreatedIn,
                             DateCreated = childDetails.DateCreated,
                             CreatedBy = childDetails.CreatedBy,
                             ModifiedIn = childDetails.ModifiedIn,
                             DateModified = childDetails.DateModified,
                             ModifiedBy = childDetails.ModifiedBy,
                             IsDeleted = childDetails.IsDeleted,
                             IsSynced = childDetails.IsSynced,
                             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                             ClinicianName = context.UserAccounts.Where(x => x.Oid == childDetails.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                             FacilityName = context.Facilities.Where(x => x.Oid == childDetails.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                         })
                     .OrderByDescending(x => x.EncounterDate)
                     .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ChildDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table ChildDetails.</param>
        /// <returns>Returns a ChildDetail if the key is matched.</returns>
        public async Task<ChildDetail> GetChildDetailByKey(Guid key)
        {
            try
            {
                return await context.ChildDetails.AsNoTracking().Where(x => x.InteractionId == key && x.IsDeleted == false).
                   Join(
                       context.Encounters.AsNoTracking(),
                       childDetails => childDetails.EncounterId,
                       encounter => encounter.Oid,
                       (childDetails, encounter) => new ChildDetail
                       {
                           // Properties from ChildDetail
                           InteractionId = childDetails.InteractionId,
                           BirthOutcome = childDetails.BirthOutcome,
                           BirthWeight = childDetails.BirthWeight,
                           ChildName = childDetails.ChildName,
                           ChildSex = childDetails.ChildSex,
                           LastTCDate = childDetails.LastTCDate,
                           LastTCResult = childDetails.LastTCResult,
                           ChildCareNumber = childDetails.ChildCareNumber,
                           DateOfChildTurns18Months = childDetails.DateOfChildTurns18Months,
                           ClientId = childDetails.ClientId,
                           Client = childDetails.Client,
                           ChildDetails = childDetails.ChildDetails,

                           // Properties from EncounterBaseModel
                           EncounterId = childDetails.EncounterId,
                           EncounterType = childDetails.EncounterType,
                           CreatedIn = childDetails.CreatedIn,
                           DateCreated = childDetails.DateCreated,
                           CreatedBy = childDetails.CreatedBy,
                           ModifiedIn = childDetails.ModifiedIn,
                           DateModified = childDetails.DateModified,
                           ModifiedBy = childDetails.ModifiedBy,
                           IsDeleted = childDetails.IsDeleted,
                           IsSynced = childDetails.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == childDetails.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == childDetails.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                       })
                   .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ChildDetails.
        /// </summary>
        /// <returns>Returns a list of all ChildDetail.</returns>
        public async Task<IEnumerable<ChildDetail>> GetChildDetails()
        {
            try
            {
                return await context.ChildDetails.AsNoTracking().Where(x => x.IsDeleted == false).
                   Join(
                       context.Encounters.AsNoTracking(),
                       childDetails => childDetails.EncounterId,
                       encounter => encounter.Oid,
                       (childDetails, encounter) => new ChildDetail
                       {
                           // Properties from ChildDetail
                           InteractionId = childDetails.InteractionId,
                           BirthOutcome = childDetails.BirthOutcome,
                           BirthWeight = childDetails.BirthWeight,
                           ChildName = childDetails.ChildName,
                           ChildSex = childDetails.ChildSex,
                           LastTCDate = childDetails.LastTCDate,
                           LastTCResult = childDetails.LastTCResult,
                           ChildCareNumber = childDetails.ChildCareNumber,
                           DateOfChildTurns18Months = childDetails.DateOfChildTurns18Months,
                           ClientId = childDetails.ClientId,
                           Client = childDetails.Client,
                           ChildDetails = childDetails.ChildDetails,

                           // Properties from EncounterBaseModel
                           EncounterId = childDetails.EncounterId,
                           EncounterType = childDetails.EncounterType,
                           CreatedIn = childDetails.CreatedIn,
                           DateCreated = childDetails.DateCreated,
                           CreatedBy = childDetails.CreatedBy,
                           ModifiedIn = childDetails.ModifiedIn,
                           DateModified = childDetails.DateModified,
                           ModifiedBy = childDetails.ModifiedBy,
                           IsDeleted = childDetails.IsDeleted,
                           IsSynced = childDetails.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == childDetails.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == childDetails.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                       })
                   .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}