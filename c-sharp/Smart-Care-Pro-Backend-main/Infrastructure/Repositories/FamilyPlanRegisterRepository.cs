using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bithy
 * Date created : 03.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IFamilyPlanRegisterRepository interface.
    /// </summary>
    public class FamilyPlanRegisterRepository : Repository<FamilyPlanRegister>, IFamilyPlanRegisterRepository
    {
        public FamilyPlanRegisterRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a FamilyPlanRegister by key.
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanRegisters.</param>
        /// <returns>Returns a FamilyPlanRegister if the key is matched.</returns>
        public async Task<FamilyPlanRegister> GetFamilyPlanRegisterByKey(Guid key)
        {
            try
            {
                var familyPlanRegister = await FirstOrDefaultAsync(c => c.InteractionId == key && c.IsDeleted == false);

                if (familyPlanRegister != null)
                {
                    familyPlanRegister.ClinicianName = await context.UserAccounts.Where(x => x.Oid == familyPlanRegister.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    familyPlanRegister.FacilityName = await context.Facilities.Where(x => x.Oid == familyPlanRegister.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    familyPlanRegister.EncounterDate = await context.Encounters.Where(x => x.Oid == familyPlanRegister.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }

                return familyPlanRegister;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of FamilyPlanRegisters.
        /// </summary>
        /// <returns>Returns a list of all FamilyPlanRegisters.</returns>
        public async Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisters()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a EncounterID by key.
        /// </summary>
        /// <param name="key">Primary key of the table Encounters.</param>
        /// <returns>Returns a Encounter if the key is matched.</returns>
        public async Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByEncounterID(Guid EncounterID)
        {
            try
            {
                return await context.FamilyPlanRegisters.Where(p => p.IsDeleted == false && p.EncounterId == EncounterID).AsNoTracking()
             .Join(
                 context.Encounters.AsNoTracking(),
               familyPlanRegister => familyPlanRegister.EncounterId,
                 encounter => encounter.Oid,
                 (familyPlanRegister, encounter) => new FamilyPlanRegister
                 {
                     EncounterId = familyPlanRegister.EncounterId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     ClientId = familyPlanRegister.ClientId,
                     ClientStaysWith = familyPlanRegister.ClientStaysWith,
                     CommunicationConsent = familyPlanRegister.CommunicationConsent,
                     CommunicationPreference = familyPlanRegister.CommunicationPreference,
                     ContactAddress = familyPlanRegister.ContactAddress,
                     ContactName = familyPlanRegister.ContactName,
                     ContactPhoneNumber = familyPlanRegister.ContactPhoneNumber,
                     CreatedBy = familyPlanRegister.CreatedBy,
                     CreatedIn = familyPlanRegister.CreatedIn,
                     DateCreated = familyPlanRegister.DateCreated,
                     DateModified = familyPlanRegister.DateModified,
                     EncounterType = familyPlanRegister.EncounterType,
                     FamilyPlanningYear = familyPlanRegister.FamilyPlanningYear,
                     InteractionId = familyPlanRegister.InteractionId,
                     IsDeleted = familyPlanRegister.IsDeleted,
                     IsSynced = familyPlanRegister.IsSynced,
                     ModifiedBy = familyPlanRegister.ModifiedBy,
                     ModifiedIn = familyPlanRegister.ModifiedIn,
                     OtherAlternativeContacts = familyPlanRegister.OtherAlternativeContacts,
                     OtherReferrals = familyPlanRegister.OtherReferrals,
                     PatientType = familyPlanRegister.PatientType,
                     ReferredBy = familyPlanRegister.ReferredBy,
                     TypeOfAlternativeContacts = familyPlanRegister.TypeOfAlternativeContacts,
                     ClinicianName = context.UserAccounts.Where(x => x.Oid == familyPlanRegister.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                     FacilityName = context.Facilities.Where(x => x.Oid == familyPlanRegister.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                 }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a FamilyPlanRegister by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a FamilyPlanRegister if the ClientID is matched.</returns>
        public async Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByClient(Guid clientId)
        {
            try
            {
                return await context.FamilyPlanRegisters.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
              .Join(
                  context.Encounters.AsNoTracking(),
                familyPlanRegister => familyPlanRegister.EncounterId,
                  encounter => encounter.Oid,
                  (familyPlanRegister, encounter) => new FamilyPlanRegister
                  {
                      EncounterId = familyPlanRegister.EncounterId,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      ClientId = familyPlanRegister.ClientId,
                      ClientStaysWith = familyPlanRegister.ClientStaysWith,
                      CommunicationConsent = familyPlanRegister.CommunicationConsent,
                      CommunicationPreference = familyPlanRegister.CommunicationPreference,
                      ContactAddress = familyPlanRegister.ContactAddress,
                      ContactName = familyPlanRegister.ContactName,
                      ContactPhoneNumber = familyPlanRegister.ContactPhoneNumber,
                      CreatedBy = familyPlanRegister.CreatedBy,
                      CreatedIn = familyPlanRegister.CreatedIn,
                      DateCreated = familyPlanRegister.DateCreated,
                      DateModified = familyPlanRegister.DateModified,
                      EncounterType = familyPlanRegister.EncounterType,
                      FamilyPlanningYear = familyPlanRegister.FamilyPlanningYear,
                      InteractionId = familyPlanRegister.InteractionId,
                      IsDeleted = familyPlanRegister.IsDeleted,
                      IsSynced = familyPlanRegister.IsSynced,
                      ModifiedBy = familyPlanRegister.ModifiedBy,
                      ModifiedIn = familyPlanRegister.ModifiedIn,
                      OtherAlternativeContacts = familyPlanRegister.OtherAlternativeContacts,
                      OtherReferrals = familyPlanRegister.OtherReferrals,
                      PatientType = familyPlanRegister.PatientType,
                      ReferredBy = familyPlanRegister.ReferredBy,
                      TypeOfAlternativeContacts = familyPlanRegister.TypeOfAlternativeContacts,
                      ClinicianName = context.UserAccounts.Where(x => x.Oid == familyPlanRegister.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                      FacilityName = context.Facilities.Where(x => x.Oid == familyPlanRegister.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                  }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.FamilyPlanRegisters.Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).AsNoTracking()
                             .Join(
                                 context.Encounters.AsNoTracking(),
                               familyPlanRegister => familyPlanRegister.EncounterId,
                                 encounter => encounter.Oid,
                                 (familyPlanRegister, encounter) => new FamilyPlanRegister
                                 {
                                     EncounterId = familyPlanRegister.EncounterId,
                                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                     ClientId = familyPlanRegister.ClientId,
                                     ClientStaysWith = familyPlanRegister.ClientStaysWith,
                                     CommunicationConsent = familyPlanRegister.CommunicationConsent,
                                     CommunicationPreference = familyPlanRegister.CommunicationPreference,
                                     ContactAddress = familyPlanRegister.ContactAddress,
                                     ContactName = familyPlanRegister.ContactName,
                                     ContactPhoneNumber = familyPlanRegister.ContactPhoneNumber,
                                     CreatedBy = familyPlanRegister.CreatedBy,
                                     CreatedIn = familyPlanRegister.CreatedIn,
                                     DateCreated = familyPlanRegister.DateCreated,
                                     DateModified = familyPlanRegister.DateModified,
                                     EncounterType = familyPlanRegister.EncounterType,
                                     FamilyPlanningYear = familyPlanRegister.FamilyPlanningYear,
                                     InteractionId = familyPlanRegister.InteractionId,
                                     IsDeleted = familyPlanRegister.IsDeleted,
                                     IsSynced = familyPlanRegister.IsSynced,
                                     ModifiedBy = familyPlanRegister.ModifiedBy,
                                     ModifiedIn = familyPlanRegister.ModifiedIn,
                                     OtherAlternativeContacts = familyPlanRegister.OtherAlternativeContacts,
                                     OtherReferrals = familyPlanRegister.OtherReferrals,
                                     PatientType = familyPlanRegister.PatientType,
                                     ReferredBy = familyPlanRegister.ReferredBy,
                                     TypeOfAlternativeContacts = familyPlanRegister.TypeOfAlternativeContacts,
                                     ClinicianName = context.UserAccounts.Where(x => x.Oid == familyPlanRegister.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                     FacilityName = context.Facilities.Where(x => x.Oid == familyPlanRegister.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                                 }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var familyPlanRegisterAsQuerable = context.FamilyPlanRegisters.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
            .Join(
                context.Encounters.AsNoTracking(),
              familyPlanRegister => familyPlanRegister.EncounterId,
                encounter => encounter.Oid,
                (familyPlanRegister, encounter) => new FamilyPlanRegister
                {
                    EncounterId = familyPlanRegister.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    ClientId = familyPlanRegister.ClientId,
                    ClientStaysWith = familyPlanRegister.ClientStaysWith,
                    CommunicationConsent = familyPlanRegister.CommunicationConsent,
                    CommunicationPreference = familyPlanRegister.CommunicationPreference,
                    ContactAddress = familyPlanRegister.ContactAddress,
                    ContactName = familyPlanRegister.ContactName,
                    ContactPhoneNumber = familyPlanRegister.ContactPhoneNumber,
                    CreatedBy = familyPlanRegister.CreatedBy,
                    CreatedIn = familyPlanRegister.CreatedIn,
                    DateCreated = familyPlanRegister.DateCreated,
                    DateModified = familyPlanRegister.DateModified,
                    EncounterType = familyPlanRegister.EncounterType,
                    FamilyPlanningYear = familyPlanRegister.FamilyPlanningYear,
                    InteractionId = familyPlanRegister.InteractionId,
                    IsDeleted = familyPlanRegister.IsDeleted,
                    IsSynced = familyPlanRegister.IsSynced,
                    ModifiedBy = familyPlanRegister.ModifiedBy,
                    ModifiedIn = familyPlanRegister.ModifiedIn,
                    OtherAlternativeContacts = familyPlanRegister.OtherAlternativeContacts,
                    OtherReferrals = familyPlanRegister.OtherReferrals,
                    PatientType = familyPlanRegister.PatientType,
                    ReferredBy = familyPlanRegister.ReferredBy,
                    TypeOfAlternativeContacts = familyPlanRegister.TypeOfAlternativeContacts,
                    ClinicianName = context.UserAccounts.Where(x => x.Oid == familyPlanRegister.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                    FacilityName = context.Facilities.Where(x => x.Oid == familyPlanRegister.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                }).AsQueryable();

                if (encounterType == null)
                    return await familyPlanRegisterAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await familyPlanRegisterAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetFamilyPlanRegisterByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.FamilyPlanRegisters.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.FamilyPlanRegisters.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}