using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 15-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class AttachedFacilityRepository : Repository<AttachedFacility>, IAttachedFacilityRepository
    {
        public AttachedFacilityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a AttachedFacility by key.
        /// </summary>
        /// <param name="key">Primary key of the table AttachedFacility.</param>
        /// <returns>Returns a AttachedFacility if the key is matched.</returns>
        public async Task<AttachedFacility> GetAttachedFacilityByKey(Guid key)
        {
            try
            {
                var attachedFacility = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (attachedFacility != null)
                {
                    attachedFacility.EncounterDate = await context.Encounters.Where(x => x.Oid == attachedFacility.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    attachedFacility.ClinicianName = await context.UserAccounts.Where(x => x.Oid == attachedFacility.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    attachedFacility.FacilityName = await context.Facilities.Where(x => x.Oid == attachedFacility.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return attachedFacility;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of AttachedFacility  .
        /// </summary>
        /// <returns>Returns a list of all AttachedFacility.</returns>        
        public async Task<IEnumerable<AttachedFacility>> GetAttachedFacility()
        {
            try
            {
                return await context.AttachedFacilities.AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                       context.Encounters.AsNoTracking(),
                        attachedFacility => attachedFacility.EncounterId,
                        encounter => encounter.Oid,
                        (attachedFacility, encounter) => new AttachedFacility
                        {

                            InteractionId = attachedFacility.InteractionId,
                            TypeOfEntry = attachedFacility.TypeOfEntry,
                            DateAttached = attachedFacility.DateAttached,
                            AttachedFacilityId = attachedFacility.AttachedFacilityId,
                            SourceFacilityId = attachedFacility.SourceFacilityId,
                            SourceFacilityName = attachedFacility.SourceFacilityName,
                            TokenNumber = attachedFacility.TokenNumber,
                            ClientId = attachedFacility.ClientId,
                            Client = attachedFacility.Client,
                            Facility = attachedFacility.Facility,

                            // Include other properties as needed

                            EncounterId = encounter.Oid,
                            EncounterType = attachedFacility.EncounterType,
                            CreatedIn = encounter.CreatedIn,
                            DateCreated = encounter.DateCreated,
                            CreatedBy = encounter.CreatedBy,
                            ModifiedIn = encounter.ModifiedIn,
                            DateModified = encounter.DateModified,
                            ModifiedBy = encounter.ModifiedBy,
                            IsDeleted = encounter.IsDeleted,
                            IsSynced = encounter.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == attachedFacility.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == attachedFacility.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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

        public async Task<IEnumerable<AttachedFacility>> GetAttachedFacilityByClient(Guid clientId)
        {
            try
            {
                return await context.AttachedFacilities.AsNoTracking()
                    .Where(x => x.ClientId == clientId && x.IsDeleted == false).Include(x => x.Facility)
                    .Join(
                       context.Encounters.AsNoTracking(),
                        attachedFacility => attachedFacility.EncounterId,
                        encounter => encounter.Oid,
                        (attachedFacility, encounter) => new AttachedFacility
                        {

                            InteractionId = attachedFacility.InteractionId,
                            TypeOfEntry = attachedFacility.TypeOfEntry,
                            DateAttached = attachedFacility.DateAttached,
                            AttachedFacilityId = attachedFacility.AttachedFacilityId,
                            SourceFacilityId = attachedFacility.SourceFacilityId,
                            SourceFacilityName = attachedFacility.SourceFacilityName,
                            TokenNumber = attachedFacility.TokenNumber,
                            ClientId = attachedFacility.ClientId,
                            //Client = attachedFacility.Client,
                            Facility = attachedFacility.Facility,

                            EncounterId = encounter.Oid,
                            EncounterType = attachedFacility.EncounterType,
                            CreatedIn = attachedFacility.CreatedIn,
                            DateCreated = attachedFacility.DateCreated,
                            CreatedBy = attachedFacility.CreatedBy,
                            ModifiedIn = attachedFacility.ModifiedIn,
                            DateModified = attachedFacility.DateModified,
                            ModifiedBy = attachedFacility.ModifiedBy,
                            IsDeleted = attachedFacility.IsDeleted,
                            IsSynced = attachedFacility.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == attachedFacility.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == attachedFacility.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
        /// The method is used to Get AttachedFacility by EncounterID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a AttachedFacilities by EncounterID.</returns>
        public async Task<IEnumerable<AttachedFacility>> GetAttachedFacilityByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.AttachedFacilities.AsNoTracking()
                    .Where(x => x.EncounterId == encounterId && x.IsDeleted == false)
                    .Join(
                       context.Encounters.AsNoTracking(),
                        attachedFacility => attachedFacility.EncounterId,
                        encounter => encounter.Oid,
                        (attachedFacility, encounter) => new AttachedFacility
                        {

                            InteractionId = attachedFacility.InteractionId,
                            TypeOfEntry = attachedFacility.TypeOfEntry,
                            DateAttached = attachedFacility.DateAttached,
                            AttachedFacilityId = attachedFacility.AttachedFacilityId,
                            SourceFacilityId = attachedFacility.SourceFacilityId,
                            SourceFacilityName = attachedFacility.SourceFacilityName,
                            TokenNumber = attachedFacility.TokenNumber,
                            ClientId = attachedFacility.ClientId,
                            Client = attachedFacility.Client,
                            Facility = attachedFacility.Facility,

                            EncounterId = attachedFacility.EncounterId,
                            EncounterType = attachedFacility.EncounterType,
                            CreatedIn = attachedFacility.CreatedIn,
                            DateCreated = attachedFacility.DateCreated,
                            CreatedBy = attachedFacility.CreatedBy,
                            ModifiedIn = attachedFacility.ModifiedIn,
                            DateModified = attachedFacility.DateModified,
                            ModifiedBy = attachedFacility.ModifiedBy,
                            IsDeleted = attachedFacility.IsDeleted,
                            IsSynced = attachedFacility.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == attachedFacility.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == attachedFacility.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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