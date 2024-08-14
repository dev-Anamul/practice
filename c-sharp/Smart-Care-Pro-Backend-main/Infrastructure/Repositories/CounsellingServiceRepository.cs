using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

/*
 * Created by   : Rezwana
 * Date created : 28.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ICounsellingServiceRepository interface.
    /// </summary>
    public class CounsellingServiceRepository : Repository<CounsellingService>, ICounsellingServiceRepository
    {
        public CounsellingServiceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a counselling service by key.
        /// </summary>
        /// <param name="key">Primary key of the table CounsellingServices.</param>
        /// <returns>Returns a counselling service if the key is matched.</returns>
        public async Task<CounsellingService> GetCounsellingServiceByKey(Guid key)
        {
            try
            {
                var counsellingService = await context.CounsellingServices.AsNoTracking().Where(c => c.InteractionId == key && c.IsDeleted == false).FirstOrDefaultAsync();

                if (counsellingService != null)
                {
                    counsellingService.ClinicianName = await context.UserAccounts.Where(x => x.Oid == counsellingService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    counsellingService.FacilityName = await context.Facilities.Where(x => x.Oid == counsellingService.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    counsellingService.EncounterDate = await context.Encounters.Where(x => x.Oid == counsellingService.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }

                return counsellingService;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of counselling services.
        /// </summary>
        /// <returns>Returns a list of all counselling services.</returns>
        public async Task<IEnumerable<CounsellingService>> GetCounsellingServices()
        {
            try
            {
                return await context.CounsellingServices
                    .AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        counsellingService => counsellingService.EncounterId,
                encounter => encounter.Oid,
                        (counsellingService, encounter) => new CounsellingService
                        {
                            InteractionId = counsellingService.InteractionId,
                            CounsellingType = counsellingService.CounsellingType,
                            OtherCounsellingType = counsellingService.OtherCounsellingType,
                            SessionNumber = counsellingService.SessionNumber,
                            SessionDate = counsellingService.SessionDate,
                            SessionNote = counsellingService.SessionNote,
                            ClientId = counsellingService.ClientId,
                            Client = counsellingService.Client,

                            // Properties from EncounterBaseModel
                            EncounterId = counsellingService.EncounterId,
                            EncounterType = counsellingService.EncounterType,
                            CreatedIn = counsellingService.CreatedIn,
                            DateCreated = counsellingService.DateCreated,
                            CreatedBy = counsellingService.CreatedBy,
                            ModifiedIn = counsellingService.ModifiedIn,
                            DateModified = counsellingService.DateModified,
                            ModifiedBy = counsellingService.ModifiedBy,
                            IsDeleted = counsellingService.IsDeleted,
                            IsSynced = counsellingService.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == counsellingService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == counsellingService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    )
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a counselling service by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a counselling service if the ClientID is matched.</returns>
        public async Task<IEnumerable<CounsellingService>> GetCounsellingServiceByClient(Guid clientId)
        {
            try
            {

                return await context.CounsellingServices
                   .AsNoTracking().Where(x => x.IsDeleted == false && x.ClientId == clientId)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       counsellingService => counsellingService.EncounterId,
               encounter => encounter.Oid,
                       (counsellingService, encounter) => new CounsellingService
                       {
                           InteractionId = counsellingService.InteractionId,
                           CounsellingType = counsellingService.CounsellingType,
                           OtherCounsellingType = counsellingService.OtherCounsellingType,
                           SessionNumber = counsellingService.SessionNumber,
                           SessionDate = counsellingService.SessionDate,
                           SessionNote = counsellingService.SessionNote,
                           ClientId = counsellingService.ClientId,
                           Client = counsellingService.Client,

                           // Properties from EncounterBaseModel
                           EncounterId = counsellingService.EncounterId,
                           EncounterType = counsellingService.EncounterType,
                           CreatedIn = counsellingService.CreatedIn,
                           DateCreated = counsellingService.DateCreated,
                           CreatedBy = counsellingService.CreatedBy,
                           ModifiedIn = counsellingService.ModifiedIn,
                           DateModified = counsellingService.DateModified,
                           ModifiedBy = counsellingService.ModifiedBy,
                           IsDeleted = counsellingService.IsDeleted,
                           IsSynced = counsellingService.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == counsellingService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == counsellingService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                       }
                   )
                   .OrderByDescending(x => x.EncounterDate)
                   .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CounsellingService>> GetCounsellingServiceByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.CounsellingServices
                           .AsNoTracking().Where(x => x.IsDeleted == false && x.ClientId == clientId && x.DateCreated >= Last24Hours)
                           .Join(
                               context.Encounters.AsNoTracking(),
                               counsellingService => counsellingService.EncounterId,
                       encounter => encounter.Oid,
                               (counsellingService, encounter) => new CounsellingService
                               {
                                   InteractionId = counsellingService.InteractionId,
                                   CounsellingType = counsellingService.CounsellingType,
                                   OtherCounsellingType = counsellingService.OtherCounsellingType,
                                   SessionNumber = counsellingService.SessionNumber,
                                   SessionDate = counsellingService.SessionDate,
                                   SessionNote = counsellingService.SessionNote,
                                   ClientId = counsellingService.ClientId,
                                   Client = counsellingService.Client,

                                   // Properties from EncounterBaseModel
                                   EncounterId = counsellingService.EncounterId,
                                   EncounterType = counsellingService.EncounterType,
                                   CreatedIn = counsellingService.CreatedIn,
                                   DateCreated = counsellingService.DateCreated,
                                   CreatedBy = counsellingService.CreatedBy,
                                   ModifiedIn = counsellingService.ModifiedIn,
                                   DateModified = counsellingService.DateModified,
                                   ModifiedBy = counsellingService.ModifiedBy,
                                   IsDeleted = counsellingService.IsDeleted,
                                   IsSynced = counsellingService.IsSynced,
                                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                   ClinicianName = context.UserAccounts.Where(x => x.Oid == counsellingService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                   FacilityName = context.Facilities.Where(x => x.Oid == counsellingService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                               }
                           )
                           .OrderByDescending(x => x.EncounterDate)
                           .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CounsellingService>> GetCounsellingServiceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var counsellingServiceAsQuerable = context.CounsellingServices
                   .AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       counsellingService => counsellingService.EncounterId,
               encounter => encounter.Oid,
                       (counsellingService, encounter) => new CounsellingService
                       {
                           InteractionId = counsellingService.InteractionId,
                           CounsellingType = counsellingService.CounsellingType,
                           OtherCounsellingType = counsellingService.OtherCounsellingType,
                           SessionNumber = counsellingService.SessionNumber,
                           SessionDate = counsellingService.SessionDate,
                           SessionNote = counsellingService.SessionNote,
                           ClientId = counsellingService.ClientId,
                           Client = counsellingService.Client,

                           // Properties from EncounterBaseModel
                           EncounterId = counsellingService.EncounterId,
                           EncounterType = counsellingService.EncounterType,
                           CreatedIn = counsellingService.CreatedIn,
                           DateCreated = counsellingService.DateCreated,
                           CreatedBy = counsellingService.CreatedBy,
                           ModifiedIn = counsellingService.ModifiedIn,
                           DateModified = counsellingService.DateModified,
                           ModifiedBy = counsellingService.ModifiedBy,
                           IsDeleted = counsellingService.IsDeleted,
                           IsSynced = counsellingService.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == counsellingService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == counsellingService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                       }
                   )
                   .OrderByDescending(x => x.EncounterDate)
                   .AsQueryable();

                if (encounterType == null)
                    return await counsellingServiceAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await counsellingServiceAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetCounsellingServiceByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.CounsellingServices.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.CounsellingServices.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get the list of counselling service by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all counselling service by EncounterID.</returns>
        public async Task<IEnumerable<CounsellingService>> GetCounsellingServiceByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.CounsellingServices
               .AsNoTracking().Where(x => x.IsDeleted == false && x.EncounterId == EncounterID)
               .Join(
                   context.Encounters.AsNoTracking(),
                   counsellingService => counsellingService.EncounterId,
           encounter => encounter.Oid,
                   (counsellingService, encounter) => new CounsellingService
                   {
                       InteractionId = counsellingService.InteractionId,
                       CounsellingType = counsellingService.CounsellingType,
                       OtherCounsellingType = counsellingService.OtherCounsellingType,
                       SessionNumber = counsellingService.SessionNumber,
                       SessionDate = counsellingService.SessionDate,
                       SessionNote = counsellingService.SessionNote,
                       ClientId = counsellingService.ClientId,
                       Client = counsellingService.Client,

                       // Properties from EncounterBaseModel
                       EncounterId = counsellingService.EncounterId,
                       EncounterType = counsellingService.EncounterType,
                       CreatedIn = counsellingService.CreatedIn,
                       DateCreated = counsellingService.DateCreated,
                       CreatedBy = counsellingService.CreatedBy,
                       ModifiedIn = counsellingService.ModifiedIn,
                       DateModified = counsellingService.DateModified,
                       ModifiedBy = counsellingService.ModifiedBy,
                       IsDeleted = counsellingService.IsDeleted,
                       IsSynced = counsellingService.IsSynced,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == counsellingService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == counsellingService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }
               )
               .OrderByDescending(x => x.EncounterDate)
               .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}