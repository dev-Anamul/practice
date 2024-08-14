using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 25.12.2022
 * Modified by  : Shakil
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ISystemExaminationRepository interface.
    /// </summary>
    public class SystemExaminationRepository : Repository<SystemExamination>, ISystemExaminationRepository
    {
        public SystemExaminationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a system examination by key.
        /// </summary>
        /// <param name="key">Primary key of the table SystemExaminations.</param>
        /// <returns>Returns a system examination if the key is matched.</returns>
        public async Task<SystemExamination> GetSystemExaminationByKey(Guid key)
        {
            try
            {
                var systemExamination = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (systemExamination != null)
                    systemExamination.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return systemExamination;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of system examinations.
        /// </summary>
        /// <returns>Returns a list of all system examinations.</returns>
        public async Task<IEnumerable<SystemExamination>> GetSystemExaminations()
        {
            try
            {
                return await LoadListWithChildAsync<SystemExamination>(s => s.IsDeleted == false, p => p.PhysicalSystem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SystemExamination>> GetSystemReviewByEncounter(Guid EncounterID)
        {
            return await context.SystemExaminations.Include(x => x.PhysicalSystem).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
                     .Join(
                            context.Encounters.AsNoTracking(),
                            systemExamination => systemExamination.EncounterId,
                            encounter => encounter.Oid,
                            (systemExamination, encounter) => new SystemExamination
                            {
                                EncounterId = systemExamination.EncounterId,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                ClientId = systemExamination.ClientId,
                                PhysicalSystemId = systemExamination.PhysicalSystemId,
                                PhysicalSystem = systemExamination.PhysicalSystem,
                                Note = systemExamination.Note,
                                ModifiedIn = systemExamination.ModifiedIn,
                                ModifiedBy = systemExamination.ModifiedBy,
                                IsSynced = systemExamination.IsSynced,
                                CreatedBy = systemExamination.CreatedBy,
                                CreatedIn = systemExamination.CreatedIn,
                                DateCreated = systemExamination.DateCreated,
                                DateModified = systemExamination.DateModified,
                                EncounterType = systemExamination.EncounterType,
                                InteractionId = systemExamination.InteractionId,
                                IsDeleted = systemExamination.IsDeleted,
                                
                            }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        }

        public async Task<IEnumerable<SystemExamination>> GetSystemExaminationsByClientID(Guid clientID)
        {
            try
            {
                return await context.SystemExaminations.Include(x => x.PhysicalSystem).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientID)
                     .Join(
                            context.Encounters.AsNoTracking(),
                            systemExamination => systemExamination.EncounterId,
                            encounter => encounter.Oid,
                            (systemExamination, encounter) => new SystemExamination
                            {
                                EncounterId = systemExamination.EncounterId,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                ClientId = systemExamination.ClientId,
                                PhysicalSystemId = systemExamination.PhysicalSystemId,
                                PhysicalSystem = systemExamination.PhysicalSystem,
                                Note = systemExamination.Note,
                                ModifiedIn = systemExamination.ModifiedIn,
                                ModifiedBy = systemExamination.ModifiedBy,
                                IsSynced = systemExamination.IsSynced,
                                CreatedBy = systemExamination.CreatedBy,
                                CreatedIn = systemExamination.CreatedIn,
                                DateCreated = systemExamination.DateCreated,
                                DateModified = systemExamination.DateModified,
                                EncounterType = systemExamination.EncounterType,
                                InteractionId = systemExamination.InteractionId,
                                IsDeleted = systemExamination.IsDeleted,

                            }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<SystemExamination>> GetSystemExaminationsByClientID(Guid clientID, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var systemExaminationAsQuerable = context.SystemExaminations.Include(x => x.PhysicalSystem).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientID)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          systemExamination => systemExamination.EncounterId,
                          encounter => encounter.Oid,
                          (systemExamination, encounter) => new SystemExamination
                          {
                              EncounterId = systemExamination.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClientId = systemExamination.ClientId,
                              PhysicalSystemId = systemExamination.PhysicalSystemId,
                              PhysicalSystem = systemExamination.PhysicalSystem,
                              Note = systemExamination.Note,
                              ModifiedIn = systemExamination.ModifiedIn,
                              ModifiedBy = systemExamination.ModifiedBy,
                              IsSynced = systemExamination.IsSynced,
                              CreatedBy = systemExamination.CreatedBy,
                              CreatedIn = systemExamination.CreatedIn,
                              DateCreated = systemExamination.DateCreated,
                              DateModified = systemExamination.DateModified,
                              EncounterType = systemExamination.EncounterType,
                              InteractionId = systemExamination.InteractionId,
                              IsDeleted = systemExamination.IsDeleted,

                          }).AsQueryable();

                if (encounterType == null)
                    return await systemExaminationAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await systemExaminationAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetSystemExaminationsByClientIDTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.SystemExaminations.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.SystemExaminations.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}