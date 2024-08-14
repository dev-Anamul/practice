using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Sayem
 * Date created : 06-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of SurgeryRepository class.
    /// </summary>
    public class SurgeryRepository : Repository<Surgery>, ISurgeryRepository
    {
        public SurgeryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Surgery   by key.
        /// </summary>
        /// <param name="key">Primary key of the table Surgerys.</param>
        /// <returns>Returns a Surgery   if the key is matched.</returns>
        public async Task<Surgery> GetSurgeryByKey(Guid key)
        {
            try
            {
                var surgery = await LoadWithChildAsync<Surgery>(s => s.InteractionId == key && s.IsDeleted == false, w => w.Ward, f => f.Ward.Firm, d => d.Ward.Firm.Department, c => c.Client);

                if (surgery != null)
                    surgery.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return surgery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Surgery   by key.
        /// </summary>
        /// <param name="clientID">Primary key of the table Client.</param>
        /// <returns>Returns a Surgery   if the key is matched.</returns>
        public async Task<IEnumerable<Surgery>> GetSurgeryByClientID(Guid clientId)
        {
            try
            {
                return await context.Surgeries.Include(w => w.Ward).ThenInclude(f => f.Firm).ThenInclude(d => d.Department).Include(c => c.Client).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                    .Join(
                           context.Encounters.AsNoTracking(),
                           surgery => surgery.EncounterId,
                           encounter => encounter.Oid,
                           (surgery, encounter) => new Surgery
                           {
                               EncounterId = surgery.EncounterId,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                               IsSynced = surgery.IsSynced,
                               IsDeleted = surgery.IsDeleted,
                               InteractionId = surgery.InteractionId,
                               BookingDate = surgery.BookingDate,
                               BookingNote = surgery.BookingNote,
                               BookingTime = surgery.BookingTime,
                               BookingTimeStr = surgery.BookingTimeStr,
                               ClientId = surgery.ClientId,
                               Client = surgery.Client,
                               CreatedBy = surgery.CreatedBy,
                               CreatedIn = surgery.CreatedIn,
                               DateCreated = surgery.DateCreated,
                               DateModified = surgery.DateModified,
                               DeviceType = surgery.DeviceType,
                               DiagnosisName = surgery.DiagnosisName,
                               EncounterType = surgery.EncounterType,
                               IsVMMCSurgery = surgery.IsVMMCSurgery,
                               ModifiedBy = surgery.ModifiedBy,
                               ModifiedIn = surgery.ModifiedIn,
                               NursingPreOpPlan = surgery.NursingPreOpPlan,
                               OperationDate = surgery.OperationDate,
                               OperationEndTime = surgery.OperationEndTime,
                               OperationEndTimeStr = surgery.OperationEndTimeStr,
                               OperationName = surgery.OperationName,
                               OperationStartTime = surgery.OperationStartTime,
                               OperationStartTimeStr = surgery.OperationStartTimeStr,
                               OperationTimeStr = surgery.OperationTimeStr,
                               OperationTime = surgery.OperationTime,
                               OperationType = surgery.OperationType,
                               Other = surgery.Other,
                               PostOpProcedure = surgery.PostOpProcedure,
                               ProcedureType = surgery.ProcedureType,
                               Surgeons = surgery.Surgeons,
                               SurgeryIndication = surgery.SurgeryIndication,
                               SurgicalCheckList = surgery.SurgicalCheckList,
                               SutureType = surgery.SutureType,
                               Team = surgery.Team,
                               TimePatientWheeledTheater = surgery.TimePatientWheeledTheater,
                               TimePatientWheeledTheaterStr = surgery.TimePatientWheeledTheaterStr,
                               TreatmentNote = surgery.TreatmentNote,
                               WardId = surgery.WardId,
                               Ward = surgery.Ward,


                           }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Surgery>> GetSurgeryByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var surgeryAsQuerable = context.Surgeries.Include(w => w.Ward).ThenInclude(f => f.Firm).ThenInclude(d => d.Department).Include(c => c.Client).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          surgery => surgery.EncounterId,
                          encounter => encounter.Oid,
                          (surgery, encounter) => new Surgery
                          {
                              EncounterId = surgery.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              IsSynced = surgery.IsSynced,
                              IsDeleted = surgery.IsDeleted,
                              InteractionId = surgery.InteractionId,
                              BookingDate = surgery.BookingDate,
                              BookingNote = surgery.BookingNote,
                              BookingTime = surgery.BookingTime,
                              BookingTimeStr = surgery.BookingTimeStr,
                              ClientId = surgery.ClientId,
                              Client = surgery.Client,
                              CreatedBy = surgery.CreatedBy,
                              CreatedIn = surgery.CreatedIn,
                              DateCreated = surgery.DateCreated,
                              DateModified = surgery.DateModified,
                              DeviceType = surgery.DeviceType,
                              DiagnosisName = surgery.DiagnosisName,
                              EncounterType = surgery.EncounterType,
                              IsVMMCSurgery = surgery.IsVMMCSurgery,
                              ModifiedBy = surgery.ModifiedBy,
                              ModifiedIn = surgery.ModifiedIn,
                              NursingPreOpPlan = surgery.NursingPreOpPlan,
                              OperationDate = surgery.OperationDate,
                              OperationEndTime = surgery.OperationEndTime,
                              OperationEndTimeStr = surgery.OperationEndTimeStr,
                              OperationName = surgery.OperationName,
                              OperationStartTime = surgery.OperationStartTime,
                              OperationStartTimeStr = surgery.OperationStartTimeStr,
                              OperationTimeStr = surgery.OperationTimeStr,
                              OperationTime = surgery.OperationTime,
                              OperationType = surgery.OperationType,
                              Other = surgery.Other,
                              PostOpProcedure = surgery.PostOpProcedure,
                              ProcedureType = surgery.ProcedureType,
                              Surgeons = surgery.Surgeons,
                              SurgeryIndication = surgery.SurgeryIndication,
                              SurgicalCheckList = surgery.SurgicalCheckList,
                              SutureType = surgery.SutureType,
                              Team = surgery.Team,
                              TimePatientWheeledTheater = surgery.TimePatientWheeledTheater,
                              TimePatientWheeledTheaterStr = surgery.TimePatientWheeledTheaterStr,
                              TreatmentNote = surgery.TreatmentNote,
                              WardId = surgery.WardId,
                              Ward = surgery.Ward,


                          }).AsQueryable();

                if (encounterType == null)
                    return await surgeryAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await surgeryAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetSurgeryByClientIDTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Surgeries.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Surgeries.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        public async Task<IEnumerable<Surgery>> GetSurgeryByEncounterID(Guid id)
        {
            try
            {
                return await context.Surgeries.Include(w => w.Ward).ThenInclude(f => f.Firm).ThenInclude(d => d.Department).Include(c => c.Client).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == id)
                    .Join(
                           context.Encounters.AsNoTracking(),
                           surgery => surgery.EncounterId,
                           encounter => encounter.Oid,
                           (surgery, encounter) => new Surgery
                           {
                               EncounterId = surgery.EncounterId,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                               IsSynced = surgery.IsSynced,
                               IsDeleted = surgery.IsDeleted,
                               InteractionId = surgery.InteractionId,
                               BookingDate = surgery.BookingDate,
                               BookingNote = surgery.BookingNote,
                               BookingTime = surgery.BookingTime,
                               BookingTimeStr = surgery.BookingTimeStr,
                               ClientId = surgery.ClientId,
                               Client = surgery.Client,
                               CreatedBy = surgery.CreatedBy,
                               CreatedIn = surgery.CreatedIn,
                               DateCreated = surgery.DateCreated,
                               DateModified = surgery.DateModified,
                               DeviceType = surgery.DeviceType,
                               DiagnosisName = surgery.DiagnosisName,
                               EncounterType = surgery.EncounterType,
                               IsVMMCSurgery = surgery.IsVMMCSurgery,
                               ModifiedBy = surgery.ModifiedBy,
                               ModifiedIn = surgery.ModifiedIn,
                               NursingPreOpPlan = surgery.NursingPreOpPlan,
                               OperationDate = surgery.OperationDate,
                               OperationEndTime = surgery.OperationEndTime,
                               OperationEndTimeStr = surgery.OperationEndTimeStr,
                               OperationName = surgery.OperationName,
                               OperationStartTime = surgery.OperationStartTime,
                               OperationStartTimeStr = surgery.OperationStartTimeStr,
                               OperationTimeStr = surgery.OperationTimeStr,
                               OperationTime = surgery.OperationTime,
                               OperationType = surgery.OperationType,
                               Other = surgery.Other,
                               PostOpProcedure = surgery.PostOpProcedure,
                               ProcedureType = surgery.ProcedureType,
                               Surgeons = surgery.Surgeons,
                               SurgeryIndication = surgery.SurgeryIndication,
                               SurgicalCheckList = surgery.SurgicalCheckList,
                               SutureType = surgery.SutureType,
                               Team = surgery.Team,
                               TimePatientWheeledTheater = surgery.TimePatientWheeledTheater,
                               TimePatientWheeledTheaterStr = surgery.TimePatientWheeledTheaterStr,
                               TreatmentNote = surgery.TreatmentNote,
                               WardId = surgery.WardId,
                               Ward = surgery.Ward,


                           }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// The method is used to get the list of Surgery  .
        /// </summary>
        /// <returns>Returns a list of all Surgery  .</returns>
        public async Task<IEnumerable<Surgery>> GetSurgerys()
        {
            try
            {
                return await LoadListWithChildAsync<Surgery>(s => s.IsDeleted == false, w => w.Ward, f => f.Ward.Firm, d => d.Ward.Firm.Department, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}