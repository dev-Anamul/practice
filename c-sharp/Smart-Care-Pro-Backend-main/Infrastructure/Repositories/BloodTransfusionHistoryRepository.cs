using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class BloodTransfusionHistoryRepository : Repository<BloodTransfusionHistory>, IBloodTransfusionHistoryRepository
    {
        public BloodTransfusionHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a BloodTransfusionHistory if the ClientID is matched.</returns>
        public async Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByClient(Guid clientId)
        {

            try
            {
                return await context.BloodTransfusionHistories.AsNoTracking().Where(x => x.ClientId == clientId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          bloodTransfusionHistory => bloodTransfusionHistory.EncounterId,
                          encounter => encounter.Oid,
                          (bloodTransfusionHistory, encounter) => new BloodTransfusionHistory
                          {
                              InteractionId = bloodTransfusionHistory.InteractionId,
                              NumberOfUnits = bloodTransfusionHistory.NumberOfUnits,
                              BloodGroup = bloodTransfusionHistory.BloodGroup,
                              KinBloodGroup = bloodTransfusionHistory.KinBloodGroup,
                              RHSensitivity = bloodTransfusionHistory.RHSensitivity,


                              // Include other properties as needed

                              EncounterId = bloodTransfusionHistory.EncounterId,
                              EncounterType = bloodTransfusionHistory.EncounterType,
                              CreatedIn = bloodTransfusionHistory.CreatedIn,
                              DateCreated = bloodTransfusionHistory.DateCreated,
                              CreatedBy = bloodTransfusionHistory.CreatedBy,
                              ModifiedIn = bloodTransfusionHistory.ModifiedIn,
                              DateModified = bloodTransfusionHistory.DateModified,
                              ModifiedBy = bloodTransfusionHistory.ModifiedBy,
                              IsDeleted = bloodTransfusionHistory.IsDeleted,
                              IsSynced = bloodTransfusionHistory.IsSynced,
                              ClientId = bloodTransfusionHistory.ClientId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == bloodTransfusionHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == bloodTransfusionHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
        public async Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByClientLast24Hours(Guid clientId)
        {

            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.BloodTransfusionHistories.AsNoTracking().Where(x => x.ClientId == clientId && x.DateCreated >= Last24Hours && x.IsDeleted == false).Join(
                                    context.Encounters.AsNoTracking(),
                                    bloodTransfusionHistory => bloodTransfusionHistory.EncounterId,
                                    encounter => encounter.Oid,
                                    (bloodTransfusionHistory, encounter) => new BloodTransfusionHistory
                                    {
                                        InteractionId = bloodTransfusionHistory.InteractionId,
                                        NumberOfUnits = bloodTransfusionHistory.NumberOfUnits,
                                        BloodGroup = bloodTransfusionHistory.BloodGroup,
                                        KinBloodGroup = bloodTransfusionHistory.KinBloodGroup,
                                        RHSensitivity = bloodTransfusionHistory.RHSensitivity,


                                        // Include other properties as needed

                                        EncounterId = bloodTransfusionHistory.EncounterId,
                                        EncounterType = bloodTransfusionHistory.EncounterType,
                                        CreatedIn = bloodTransfusionHistory.CreatedIn,
                                        DateCreated = bloodTransfusionHistory.DateCreated,
                                        CreatedBy = bloodTransfusionHistory.CreatedBy,
                                        ModifiedIn = bloodTransfusionHistory.ModifiedIn,
                                        DateModified = bloodTransfusionHistory.DateModified,
                                        ModifiedBy = bloodTransfusionHistory.ModifiedBy,
                                        IsDeleted = bloodTransfusionHistory.IsDeleted,
                                        IsSynced = bloodTransfusionHistory.IsSynced,
                                        ClientId = bloodTransfusionHistory.ClientId,
                                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                        ClinicianName = context.UserAccounts.Where(x => x.Oid == bloodTransfusionHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                        FacilityName = context.Facilities.Where(x => x.Oid == bloodTransfusionHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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

        public async Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var bloodTransfusionHistoryAsQuerable = context.BloodTransfusionHistories.AsNoTracking().Where(x => x.ClientId == clientId && x.IsDeleted == false).Join(
                              context.Encounters.AsNoTracking(),
                              bloodTransfusionHistory => bloodTransfusionHistory.EncounterId,
                              encounter => encounter.Oid,
                              (bloodTransfusionHistory, encounter) => new BloodTransfusionHistory
                              {
                                  InteractionId = bloodTransfusionHistory.InteractionId,
                                  NumberOfUnits = bloodTransfusionHistory.NumberOfUnits,
                                  BloodGroup = bloodTransfusionHistory.BloodGroup,
                                  KinBloodGroup = bloodTransfusionHistory.KinBloodGroup,
                                  RHSensitivity = bloodTransfusionHistory.RHSensitivity,


                                  // Include other properties as needed

                                  EncounterId = bloodTransfusionHistory.EncounterId,
                                  EncounterType = bloodTransfusionHistory.EncounterType,
                                  CreatedIn = bloodTransfusionHistory.CreatedIn,
                                  DateCreated = bloodTransfusionHistory.DateCreated,
                                  CreatedBy = bloodTransfusionHistory.CreatedBy,
                                  ModifiedIn = bloodTransfusionHistory.ModifiedIn,
                                  DateModified = bloodTransfusionHistory.DateModified,
                                  ModifiedBy = bloodTransfusionHistory.ModifiedBy,
                                  IsDeleted = bloodTransfusionHistory.IsDeleted,
                                  IsSynced = bloodTransfusionHistory.IsSynced,
                                  ClientId = bloodTransfusionHistory.ClientId,
                                  EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                  ClinicianName = context.UserAccounts.Where(x => x.Oid == bloodTransfusionHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                  FacilityName = context.Facilities.Where(x => x.Oid == bloodTransfusionHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                              }
                          ).AsQueryable();

                if (encounterType == null)
                    return await bloodTransfusionHistoryAsQuerable.OrderByDescending(b => b.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await bloodTransfusionHistoryAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(b => b.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetBloodTransfusionHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.BloodTransfusionHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.BloodTransfusionHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of BloodTransfusionHistory by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all BloodTransfusionHistory by EncounterID.</returns>
        public async Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistoryByEncounter(Guid encounterId)
        {
            try
            {
                return await context.BloodTransfusionHistories.AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          bloodTransfusionHistory => bloodTransfusionHistory.EncounterId,
                          encounter => encounter.Oid,
                          (bloodTransfusionHistory, encounter) => new BloodTransfusionHistory
                          {
                              InteractionId = bloodTransfusionHistory.InteractionId,
                              NumberOfUnits = bloodTransfusionHistory.NumberOfUnits,
                              BloodGroup = bloodTransfusionHistory.BloodGroup,
                              KinBloodGroup = bloodTransfusionHistory.KinBloodGroup,
                              RHSensitivity = bloodTransfusionHistory.RHSensitivity,


                              // Include other properties as needed

                              EncounterId = bloodTransfusionHistory.EncounterId,
                              ClientId = bloodTransfusionHistory.ClientId,
                              EncounterType = bloodTransfusionHistory.EncounterType,
                              CreatedIn = bloodTransfusionHistory.CreatedIn,
                              DateCreated = bloodTransfusionHistory.DateCreated,
                              CreatedBy = bloodTransfusionHistory.CreatedBy,
                              ModifiedIn = bloodTransfusionHistory.ModifiedIn,
                              DateModified = bloodTransfusionHistory.DateModified,
                              ModifiedBy = bloodTransfusionHistory.ModifiedBy,
                              IsDeleted = bloodTransfusionHistory.IsDeleted,
                              IsSynced = bloodTransfusionHistory.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == bloodTransfusionHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == bloodTransfusionHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
        /// The method is used to get a BloodTransfusionHistory by key.
        /// </summary>
        /// <param name="key">Primary key of the table BloodTransfusionHistorys.</param>
        /// <returns>Returns a BloodTransfusionHistory if the key is matched.</returns>
        public async Task<BloodTransfusionHistory> GetBloodTransfusionHistoryByKey(Guid key)
        {
            try
            {
                var bloodTransfusionHistory = await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (bloodTransfusionHistory != null)
                {
                    bloodTransfusionHistory.EncounterDate = await context.Encounters.Where(x => x.Oid == bloodTransfusionHistory.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    bloodTransfusionHistory.ClinicianName = context.UserAccounts.Where(x => x.Oid == bloodTransfusionHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    bloodTransfusionHistory.FacilityName = context.Facilities.Where(x => x.Oid == bloodTransfusionHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                }

                return bloodTransfusionHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of BloodTransfusionHistorys.
        /// </summary>
        /// <returns>Returns a list of all BloodTransfusionHistory.</returns>
        public async Task<IEnumerable<BloodTransfusionHistory>> GetBloodTransfusionHistorys()
        {
            try
            {
                return await context.BloodTransfusionHistories.AsNoTracking().Where(x => x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          bloodTransfusionHistory => bloodTransfusionHistory.EncounterId,
                          encounter => encounter.Oid,
                          (bloodTransfusionHistory, encounter) => new BloodTransfusionHistory
                          {
                              InteractionId = bloodTransfusionHistory.InteractionId,
                              NumberOfUnits = bloodTransfusionHistory.NumberOfUnits,
                              BloodGroup = bloodTransfusionHistory.BloodGroup,
                              KinBloodGroup = bloodTransfusionHistory.KinBloodGroup,
                              RHSensitivity = bloodTransfusionHistory.RHSensitivity,

                              EncounterId = bloodTransfusionHistory.EncounterId,
                              ClientId = bloodTransfusionHistory.ClientId,
                              EncounterType = bloodTransfusionHistory.EncounterType,
                              CreatedIn = bloodTransfusionHistory.CreatedIn,
                              DateCreated = bloodTransfusionHistory.DateCreated,
                              CreatedBy = bloodTransfusionHistory.CreatedBy,
                              ModifiedIn = bloodTransfusionHistory.ModifiedIn,
                              DateModified = bloodTransfusionHistory.DateModified,
                              ModifiedBy = bloodTransfusionHistory.ModifiedBy,
                              IsDeleted = bloodTransfusionHistory.IsDeleted,
                              IsSynced = bloodTransfusionHistory.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == bloodTransfusionHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == bloodTransfusionHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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