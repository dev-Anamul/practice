using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class InsertionAndRemovalProcedureRepository : Repository<InsertionAndRemovalProcedure>, IInsertionAndRemovalProcedureRepository
    {
        /// <summary>
        /// Implementation of IInsertionAndRemovalProcedureRepository interface.
        /// </summary>
        public InsertionAndRemovalProcedureRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a InsertionAndRemovalProcedure by key.
        /// </summary>
        /// <param name="key">Primary key of the table InsertionAndRemovalProcedures.</param>
        /// <returns>Returns a InsertionAndRemovalProcedure if the key is matched.</returns>
        public async Task<InsertionAndRemovalProcedure> GetInsertionAndRemovalProcedureByKey(Guid key)
        {
            try
            {
                var insertionAndRemovalProcedure = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (insertionAndRemovalProcedure != null)
                {
                    insertionAndRemovalProcedure.ClinicianName = await context.UserAccounts.Where(x => x.Oid == insertionAndRemovalProcedure.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    insertionAndRemovalProcedure.FacilityName = await context.Facilities.Where(x => x.Oid == insertionAndRemovalProcedure.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    insertionAndRemovalProcedure.EncounterDate = await context.Encounters.Where(x => x.Oid == insertionAndRemovalProcedure.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return insertionAndRemovalProcedure;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of InsertionAndRemovalProcedure.
        /// </summary>
        /// <returns>Returns a list of all InsertionAndRemovalProcedures.</returns>
        public async Task<IEnumerable<InsertionAndRemovalProcedure>> GetInsertionAndRemovalProcedures()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a InsertionAndRemovalProcedure by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a InsertionAndRemovalProcedure if the ClientID is matched.</returns>
        public async Task<IEnumerable<InsertionAndRemovalProcedure>> GetInsertionAndRemovalProcedureByClient(Guid clientId)
        {
            try
            {
                return await context.InsertionAndRemovalProcedures.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       insertionAndRemovalProcedure => insertionAndRemovalProcedure.EncounterId,
       encounter => encounter.Oid,
       (insertionAndRemovalProcedure, encounter) => new InsertionAndRemovalProcedure
       {
           EncounterId = insertionAndRemovalProcedure.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           ClientId = insertionAndRemovalProcedure.ClientId,
           CreatedBy = insertionAndRemovalProcedure.CreatedBy,
           CreatedIn = insertionAndRemovalProcedure.CreatedIn,
           DateCreated = insertionAndRemovalProcedure.DateCreated,
           DateModified = insertionAndRemovalProcedure.DateModified,
           EncounterType = insertionAndRemovalProcedure.EncounterType,
           ImplantInsertion = insertionAndRemovalProcedure.ImplantInsertion,
           ImplantInsertionDate = insertionAndRemovalProcedure.ImplantInsertionDate,
           ImplantRemoval = insertionAndRemovalProcedure.ImplantRemoval,
           ImplantRemovalDate = insertionAndRemovalProcedure.ImplantRemovalDate,
           InteractionId = insertionAndRemovalProcedure.InteractionId,
           IsDeleted = insertionAndRemovalProcedure.IsDeleted,
           IsSynced = insertionAndRemovalProcedure.IsSynced,
           IUCDInsertion = insertionAndRemovalProcedure.IUCDInsertion,
           IUCDInsertionDate = insertionAndRemovalProcedure.IUCDInsertionDate,
           IUCDRemoval = insertionAndRemovalProcedure.IUCDRemoval,
           IUCDRemovalDate = insertionAndRemovalProcedure.IUCDRemovalDate,
           ModifiedBy = insertionAndRemovalProcedure.ModifiedBy,
           ModifiedIn = insertionAndRemovalProcedure.ModifiedIn,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == insertionAndRemovalProcedure.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == insertionAndRemovalProcedure.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
       }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of InsertionAndRemovalProcedure by EncounterID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a list of all InsertionAndRemovalProcedure by EncounterID.</returns>
        public async Task<IEnumerable<InsertionAndRemovalProcedure>> GetInsertionAndRemovalProcedureByEncounter(Guid encounterId)
        {
            try
            {
                return await context.InsertionAndRemovalProcedures.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       insertionAndRemovalProcedure => insertionAndRemovalProcedure.EncounterId,
       encounter => encounter.Oid,
       (insertionAndRemovalProcedure, encounter) => new InsertionAndRemovalProcedure
       {
           EncounterId = insertionAndRemovalProcedure.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           ClientId = insertionAndRemovalProcedure.ClientId,
           CreatedBy = insertionAndRemovalProcedure.CreatedBy,
           CreatedIn = insertionAndRemovalProcedure.CreatedIn,
           DateCreated = insertionAndRemovalProcedure.DateCreated,
           DateModified = insertionAndRemovalProcedure.DateModified,
           EncounterType = insertionAndRemovalProcedure.EncounterType,
           ImplantInsertion = insertionAndRemovalProcedure.ImplantInsertion,
           ImplantInsertionDate = insertionAndRemovalProcedure.ImplantInsertionDate,
           ImplantRemoval = insertionAndRemovalProcedure.ImplantRemoval,
           ImplantRemovalDate = insertionAndRemovalProcedure.ImplantRemovalDate,
           InteractionId = insertionAndRemovalProcedure.InteractionId,
           IsDeleted = insertionAndRemovalProcedure.IsDeleted,
           IsSynced = insertionAndRemovalProcedure.IsSynced,
           IUCDInsertion = insertionAndRemovalProcedure.IUCDInsertion,
           IUCDInsertionDate = insertionAndRemovalProcedure.IUCDInsertionDate,
           IUCDRemoval = insertionAndRemovalProcedure.IUCDRemoval,
           IUCDRemovalDate = insertionAndRemovalProcedure.IUCDRemovalDate,
           ModifiedBy = insertionAndRemovalProcedure.ModifiedBy,
           ModifiedIn = insertionAndRemovalProcedure.ModifiedIn,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == insertionAndRemovalProcedure.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == insertionAndRemovalProcedure.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


       }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}