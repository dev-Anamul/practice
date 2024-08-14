using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Lion
 * Date created : 25.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IBirthHistoryRepository interface.
    /// </summary>
    public class BirthHistoryRepository : Repository<BirthHistory>, IBirthHistoryRepository
    {
        public BirthHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public async Task<BirthHistory> GetBirthHistoryByKey(Guid key)
        {
            try
            {
                BirthHistory birthHistory = await context.BirthHistories.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
                if (birthHistory is not null)
                {
                    birthHistory.EncounterDate = await context.Encounters.Where(x => x.Oid == birthHistory.EncounterId).AsNoTracking().Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();
                    birthHistory.ClinicianName = await context.UserAccounts.Where(x => x.Oid == birthHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    birthHistory.FacilityName =await context.Facilities.Where(x => x.Oid == birthHistory.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }
                return birthHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get a birth history by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a birth history if the ClientID is matched.</returns>
        public async Task<IEnumerable<BirthHistory>> GetBirthHistoryByClient(Guid clientId)
        {
            try
            {
                return await context.BirthHistories.AsNoTracking()
                    .Where(x => x.ClientId == clientId && x.IsDeleted == false)
                    .Join(
                          context.Encounters.AsNoTracking(),
                          birthHistory => birthHistory.EncounterId,
                          encounter => encounter.Oid,
                          (birthHistory, encounter) => new BirthHistory
                          {
                              InteractionId = birthHistory.InteractionId,
                              BirthWeight = birthHistory.BirthWeight,
                              BirthHeight = birthHistory.BirthHeight,
                              BirthOutcome = birthHistory.BirthOutcome,
                              HeadCircumference = birthHistory.HeadCircumference,
                              ChestCircumference = birthHistory.ChestCircumference,
                              GeneralCondition = birthHistory.GeneralCondition,
                              IsBreastFeedingWell = birthHistory.IsBreastFeedingWell,
                              OtherFeedingOption = birthHistory.OtherFeedingOption,
                              DeliveryTime = birthHistory.DeliveryTime,
                              VaccinationOutside = birthHistory.VaccinationOutside,
                              TetanusAtBirth = birthHistory.TetanusAtBirth,
                              Note = birthHistory.Note,
                              ClientId = birthHistory.ClientId,
                              // Include other properties as needed

                              EncounterId = birthHistory.EncounterId,
                              EncounterType = birthHistory.EncounterType,
                              CreatedIn = birthHistory.CreatedIn,
                              DateCreated = birthHistory.DateCreated,
                              CreatedBy = birthHistory.CreatedBy,
                              ModifiedIn = birthHistory.ModifiedIn,
                              DateModified = birthHistory.DateModified,
                              ModifiedBy = birthHistory.ModifiedBy,
                              IsDeleted = birthHistory.IsDeleted,
                              IsSynced = birthHistory.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == birthHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == birthHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }
                      )
                      .Where(x => x.ClientId == clientId && x.IsDeleted == false)
                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<BirthHistory>> GetBirthHistories()
        {
            try
            {
                return await context.BirthHistories.AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                          context.Encounters.AsNoTracking(),
                          birthHistory => birthHistory.EncounterId,
                          encounter => encounter.Oid,
                          (birthHistory, encounter) => new BirthHistory
                          {
                              InteractionId = birthHistory.InteractionId,
                              BirthWeight = birthHistory.BirthWeight,
                              BirthHeight = birthHistory.BirthHeight,
                              BirthOutcome = birthHistory.BirthOutcome,
                              HeadCircumference = birthHistory.HeadCircumference,
                              ChestCircumference = birthHistory.ChestCircumference,
                              GeneralCondition = birthHistory.GeneralCondition,
                              IsBreastFeedingWell = birthHistory.IsBreastFeedingWell,
                              OtherFeedingOption = birthHistory.OtherFeedingOption,
                              DeliveryTime = birthHistory.DeliveryTime,
                              VaccinationOutside = birthHistory.VaccinationOutside,
                              TetanusAtBirth = birthHistory.TetanusAtBirth,
                              Note = birthHistory.Note,
                              ClientId = birthHistory.ClientId,
                              // Include other properties as needed

                              EncounterId = birthHistory.EncounterId,
                              EncounterType = birthHistory.EncounterType,
                              CreatedIn = birthHistory.CreatedIn,
                              DateCreated = birthHistory.DateCreated,
                              CreatedBy = birthHistory.CreatedBy,
                              ModifiedIn = birthHistory.ModifiedIn,
                              DateModified = birthHistory.DateModified,
                              ModifiedBy = birthHistory.ModifiedBy,
                              IsDeleted = birthHistory.IsDeleted,
                              IsSynced = birthHistory.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == birthHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == birthHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }
                      )

                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// he method is used to get the list of birth histories by encounter id.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BirthHistory>> GetBirthHistoryByEncounter(Guid encounterId)
        {
            try
            {
                return await context.BirthHistories.AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false)
                    .Join(
                          context.Encounters.AsNoTracking(),
                          birthHistory => birthHistory.EncounterId,
                          encounter => encounter.Oid,
                          (birthHistory, encounter) => new BirthHistory
                          {
                              InteractionId = birthHistory.InteractionId,
                              BirthWeight = birthHistory.BirthWeight,
                              BirthHeight = birthHistory.BirthHeight,
                              BirthOutcome = birthHistory.BirthOutcome,
                              HeadCircumference = birthHistory.HeadCircumference,
                              ChestCircumference = birthHistory.ChestCircumference,
                              GeneralCondition = birthHistory.GeneralCondition,
                              IsBreastFeedingWell = birthHistory.IsBreastFeedingWell,
                              OtherFeedingOption = birthHistory.OtherFeedingOption,
                              DeliveryTime = birthHistory.DeliveryTime,
                              VaccinationOutside = birthHistory.VaccinationOutside,
                              TetanusAtBirth = birthHistory.TetanusAtBirth,
                              Note = birthHistory.Note,
                              ClientId = birthHistory.ClientId,
                              // Include other properties as needed

                              EncounterId = birthHistory.EncounterId,
                              EncounterType = birthHistory.EncounterType,
                              CreatedIn = birthHistory.CreatedIn,
                              DateCreated = birthHistory.DateCreated,
                              CreatedBy = birthHistory.CreatedBy,
                              ModifiedIn = birthHistory.ModifiedIn,
                              DateModified = birthHistory.DateModified,
                              ModifiedBy = birthHistory.ModifiedBy,
                              IsDeleted = birthHistory.IsDeleted,
                              IsSynced = birthHistory.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == birthHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == birthHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }
                      )

                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}