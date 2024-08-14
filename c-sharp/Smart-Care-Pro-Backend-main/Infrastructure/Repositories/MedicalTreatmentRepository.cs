using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class MedicalTreatmentRepository : Repository<MedicalTreatment>, IMedicalTreatmentRepository
    {
        public MedicalTreatmentRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get Medical treatment by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicalTreatments.</param>
        /// <returns>Returns a MedicalTreatment if the key is matched.</returns>
        public async Task<MedicalTreatment> GetMedicalTreatmentByKey(Guid key)
        {
            try
            {
                var medicalTreatment = await FirstOrDefaultAsync(m => m.InteractionId == key && m.IsDeleted == false);

                if (medicalTreatment != null)
                    medicalTreatment.EncounterDate = await context.Encounters.Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();

                return medicalTreatment;

                /// return await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Medical treatments.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<MedicalTreatment>> GetMedicalTreatments()
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
        /// The method is used to get the list of MedicalTreatment by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all MedicalTreatment by EncounterID.</returns>
        public async Task<IEnumerable<MedicalTreatment>> GetMedicalTreatmentByEncounter(Guid encounterId)
        {
            try
            {
                var medicalTreatment = context.MedicalTreatments.Where(m => m.IsDeleted == false && m.EncounterId == encounterId).AsNoTracking()
                 .Join(context.Encounters.AsNoTracking(),
                 medicalTreatment => medicalTreatment.EncounterId,
                 encounter => encounter.Oid,
                 (medicalTreatment, encounter) => new MedicalTreatment
                 {
                     CreatedBy = medicalTreatment.CreatedBy,
                     CreatedIn = medicalTreatment.CreatedIn,
                     DateCreated = medicalTreatment.DateCreated,
                     DateModified = medicalTreatment.DateModified,
                     DeliveryId = medicalTreatment.DeliveryId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     EncounterId = medicalTreatment.EncounterId,
                     EncounterType = medicalTreatment.EncounterType,
                     InteractionId = medicalTreatment.InteractionId,
                     IsDeleted = medicalTreatment.IsDeleted,
                     IsSynced = medicalTreatment.IsSynced,
                     ModifiedBy = medicalTreatment.ModifiedBy,
                     ModifiedIn = medicalTreatment.ModifiedIn,
                     Other = medicalTreatment.Other,
                     Treatments = medicalTreatment.Treatments,

                 }).AsQueryable();

                return await medicalTreatment.OrderByDescending(x => x.EncounterDate).ToListAsync();

                /// return await QueryAsync(b => b.IsDeleted == false && b.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of MedicalTreatment by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all MedicalTreatment by DeliveryId.</returns>
        public async Task<IEnumerable<MedicalTreatment>> GetMedicalTreatmentByDelivery(Guid deliveryId)
        {
            try
            {
                var medicalTreatment = context.MedicalTreatments.Where(m => m.IsDeleted == false && m.DeliveryId == deliveryId).AsNoTracking()
                 .Join(context.Encounters.AsNoTracking(),
                 medicalTreatment => medicalTreatment.EncounterId,
                 encounter => encounter.Oid,
                 (medicalTreatment, encounter) => new MedicalTreatment
                 {
                     CreatedBy = medicalTreatment.CreatedBy,
                     CreatedIn = medicalTreatment.CreatedIn,
                     DateCreated = medicalTreatment.DateCreated,
                     DateModified = medicalTreatment.DateModified,
                     DeliveryId = medicalTreatment.DeliveryId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     EncounterId = medicalTreatment.EncounterId,
                     EncounterType = medicalTreatment.EncounterType,
                     InteractionId = medicalTreatment.InteractionId,
                     IsDeleted = medicalTreatment.IsDeleted,
                     IsSynced = medicalTreatment.IsSynced,
                     ModifiedBy = medicalTreatment.ModifiedBy,
                     ModifiedIn = medicalTreatment.ModifiedIn,
                     Other = medicalTreatment.Other,
                     Treatments = medicalTreatment.Treatments,
                 }).AsQueryable();

                return await medicalTreatment.OrderByDescending(x => x.EncounterDate).ToListAsync();
                /// return await QueryAsync(n => n.IsDeleted == false && n.DeliveryId == deliveryId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}