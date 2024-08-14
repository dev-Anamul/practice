using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 02.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IFamilyMemberRepository interface.
    /// </summary>
    public class FamilyMemberRepository : Repository<FamilyMember>, IFamilyMemberRepository
    {
        public FamilyMemberRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get Family Member by key.
        /// </summary>
        /// <param name="key">Primary key of the table FamilyMembers.</param>
        /// <returns>Returns a family member if the key is matched.</returns>
        public async Task<FamilyMember> GetFamilyMemberByKey(Guid key)
        {
            try
            {
                var familyMember = await FirstOrDefaultAsync(f => f.InteractionId == key && f.IsDeleted == false);

                if (familyMember != null)
                {
                    familyMember.ClinicianName = await context.UserAccounts.Where(x => x.Oid == familyMember.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    familyMember.FacilityName = await context.Facilities.Where(x => x.Oid == familyMember.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    familyMember.EncounterDate = await context.Encounters.Where(x => x.Oid == familyMember.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }
                return familyMember;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of family members.
        /// </summary>
        /// <returns>Returns a list of all family members.</returns>
        public async Task<IEnumerable<FamilyMember>> GetFamilyMembers()
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

        /// <summary>
        /// The method is used to get a family member by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a family member if the clientId is matched.</returns>
        public async Task<IEnumerable<FamilyMember>> GetFamilyMemberByClientId(Guid clientId)
        {
            try
            {
                return await context.FamilyMembers.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
           .Join(
               context.Encounters.AsNoTracking(),
              familyMembers => familyMembers.EncounterId,
               encounter => encounter.Oid,
               (familyMembers, encounter) => new FamilyMember
               {
                   EncounterId = familyMembers.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   ClientId = familyMembers.ClientId,
                   DateCreated = familyMembers.DateCreated,
                   CreatedIn = familyMembers.CreatedIn,
                   CreatedBy = familyMembers.CreatedBy,
                   DateModified = familyMembers.DateModified,
                   Age = familyMembers.Age,
                   EncounterType = familyMembers.EncounterType,
                   FamilyMemberType = familyMembers.FamilyMemberType,
                   FirstName = familyMembers.FirstName,
                   HIVResult = familyMembers.HIVResult,
                   HIVTested = familyMembers.HIVTested,
                   InteractionId = familyMembers.InteractionId,
                   IsDeleted = familyMembers.IsDeleted,
                   IsSynced = familyMembers.IsSynced,
                   ModifiedBy = familyMembers.ModifiedBy,
                   ModifiedIn = familyMembers.ModifiedIn,
                   OnART = familyMembers.OnART,
                   OtherFamilyMember = familyMembers.OtherFamilyMember,
                   Surname = familyMembers.Surname,
                   ClinicianName = context.UserAccounts.Where(x => x.Oid == familyMembers.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                   FacilityName = context.Facilities.Where(x => x.Oid == familyMembers.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

               }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<FamilyMember>> GetFamilyMemberByClientIdLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);
 
                return await context.FamilyMembers.Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).AsNoTracking()
                      .Join(
                          context.Encounters.AsNoTracking(),
                         familyMembers => familyMembers.EncounterId,
                          encounter => encounter.Oid,
                          (familyMembers, encounter) => new FamilyMember
                          {
                              EncounterId = familyMembers.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClientId = familyMembers.ClientId,
                              DateCreated = familyMembers.DateCreated,
                              CreatedIn = familyMembers.CreatedIn,
                              CreatedBy = familyMembers.CreatedBy,
                              DateModified = familyMembers.DateModified,
                              Age = familyMembers.Age,
                              EncounterType = familyMembers.EncounterType,
                              FamilyMemberType = familyMembers.FamilyMemberType,
                              FirstName = familyMembers.FirstName,
                              HIVResult = familyMembers.HIVResult,
                              HIVTested = familyMembers.HIVTested,
                              InteractionId = familyMembers.InteractionId,
                              IsDeleted = familyMembers.IsDeleted,
                              IsSynced = familyMembers.IsSynced,
                              ModifiedBy = familyMembers.ModifiedBy,
                              ModifiedIn = familyMembers.ModifiedIn,
                              OnART = familyMembers.OnART,
                              OtherFamilyMember = familyMembers.OtherFamilyMember,
                              Surname = familyMembers.Surname,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == familyMembers.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == familyMembers.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<FamilyMember>> GetFamilyMemberByClientId(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var familyMembersAsQuerable = context.FamilyMembers.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
          .Join(
              context.Encounters.AsNoTracking(),
             familyMembers => familyMembers.EncounterId,
              encounter => encounter.Oid,
              (familyMembers, encounter) => new FamilyMember
              {
                  EncounterId = familyMembers.EncounterId,
                  EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                  ClientId = familyMembers.ClientId,
                  DateCreated = familyMembers.DateCreated,
                  CreatedIn = familyMembers.CreatedIn,
                  CreatedBy = familyMembers.CreatedBy,
                  DateModified = familyMembers.DateModified,
                  Age = familyMembers.Age,
                  EncounterType = familyMembers.EncounterType,
                  FamilyMemberType = familyMembers.FamilyMemberType,
                  FirstName = familyMembers.FirstName,
                  HIVResult = familyMembers.HIVResult,
                  HIVTested = familyMembers.HIVTested,
                  InteractionId = familyMembers.InteractionId,
                  IsDeleted = familyMembers.IsDeleted,
                  IsSynced = familyMembers.IsSynced,
                  ModifiedBy = familyMembers.ModifiedBy,
                  ModifiedIn = familyMembers.ModifiedIn,
                  OnART = familyMembers.OnART,
                  OtherFamilyMember = familyMembers.OtherFamilyMember,
                  Surname = familyMembers.Surname,
                  ClinicianName = context.UserAccounts.Where(x => x.Oid == familyMembers.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                  FacilityName = context.Facilities.Where(x => x.Oid == familyMembers.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

              }).AsQueryable();

                if (encounterType == null)
                    return await familyMembersAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await familyMembersAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetFamilyMemberByClientIdTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.FamilyMembers.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.FamilyMembers.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a encounterId by key.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Returns a encounterId if the key is matched.</returns>
        public async Task<IEnumerable<FamilyMember>> GetFamilyMemberByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.FamilyMembers.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
             .Join(
                 context.Encounters.AsNoTracking(),
                familyMembers => familyMembers.EncounterId,
                 encounter => encounter.Oid,
                 (familyMembers, encounter) => new FamilyMember
                 {
                     EncounterId = familyMembers.EncounterId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     ClientId = familyMembers.ClientId,
                     DateCreated = familyMembers.DateCreated,
                     CreatedIn = familyMembers.CreatedIn,
                     CreatedBy = familyMembers.CreatedBy,
                     DateModified = familyMembers.DateModified,
                     Age = familyMembers.Age,
                     EncounterType = familyMembers.EncounterType,
                     FamilyMemberType = familyMembers.FamilyMemberType,
                     FirstName = familyMembers.FirstName,
                     HIVResult = familyMembers.HIVResult,
                     HIVTested = familyMembers.HIVTested,
                     InteractionId = familyMembers.InteractionId,
                     IsDeleted = familyMembers.IsDeleted,
                     IsSynced = familyMembers.IsSynced,
                     ModifiedBy = familyMembers.ModifiedBy,
                     ModifiedIn = familyMembers.ModifiedIn,
                     OnART = familyMembers.OnART,
                     OtherFamilyMember = familyMembers.OtherFamilyMember,
                     Surname = familyMembers.Surname,
                     ClinicianName = context.UserAccounts.Where(x => x.Oid == familyMembers.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                     FacilityName = context.Facilities.Where(x => x.Oid == familyMembers.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                 }).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}