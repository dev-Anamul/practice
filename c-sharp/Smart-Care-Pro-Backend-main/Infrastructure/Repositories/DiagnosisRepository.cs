using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 04.01.23
 * Modified by  : Brian
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IDiagnosisRepository interface.
    /// </summary>
    public class DiagnosisRepository : Repository<Diagnosis>, IDiagnosisRepository
    {
        public DiagnosisRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table Diagnosis.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public async Task<Diagnosis> GetDiagnosisByKey(Guid key)
        {
            try
            {
                var diagnosis = await LoadWithChildAsync<Diagnosis>(i => i.InteractionId == key && i.IsDeleted == false, p => p.ICDDiagnosis, p => p.NTGLevelThreeDiagnosis);

                if (diagnosis != null)
                {
                    diagnosis.ClinicianName = await context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    diagnosis.FacilityName = await context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    diagnosis.EncounterDate = await context.Encounters.Where(x => x.Oid == diagnosis.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return diagnosis;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table Diagnosis.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public async Task<IEnumerable<Diagnosis>> GetDiagnosisBySurgeryId(Guid key)
        {
            try
            {
                return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(i => i.SurgeryId == key && i.IsDeleted == false && i.IsSelectedForSurgery == true)
          .Join(
              context.Encounters.AsNoTracking(),
              diagnosis => diagnosis.EncounterId,
              encounter => encounter.Oid,
              (diagnosis, encounter) => new Diagnosis
              {
                  EncounterId = diagnosis.EncounterId,
                  EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                  DateCreated = diagnosis.DateCreated,
                  CreatedBy = encounter.CreatedBy,
                  ClientId = diagnosis.ClientId,
                  Client = diagnosis.Client,
                  CreatedIn = diagnosis.CreatedIn,
                  DateModified = diagnosis.DateModified,
                  DiagnosisType = diagnosis.DiagnosisType,
                  EncounterType = diagnosis.EncounterType,
                  ICDDiagnosis = diagnosis.ICDDiagnosis,
                  ICDId = diagnosis.ICDId,
                  InteractionId = diagnosis.InteractionId,
                  IsDeleted = diagnosis.IsDeleted,
                  IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
                  IsSynced = diagnosis.IsSynced,
                  NTGId = diagnosis.NTGId,
                  NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
                  ModifiedBy = diagnosis.ModifiedBy,
                  ModifiedIn = diagnosis.ModifiedIn,
                  Surgery = diagnosis.Surgery,
                  SurgeryId = diagnosis.SurgeryId,
                  ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                  FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

              }).OrderByDescending(x => x.EncounterDate).ToListAsync();


            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key">Primary key of the table Diagnosis.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public async Task<IEnumerable<Diagnosis>> ReadDiagnosisBySurgery(Guid key)
        {
            try
            {
                return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(i => i.SurgeryId == key && i.IsDeleted == false)
         .Join(
             context.Encounters.AsNoTracking(),
             diagnosis => diagnosis.EncounterId,
             encounter => encounter.Oid,
             (diagnosis, encounter) => new Diagnosis
             {
                 EncounterId = diagnosis.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 DateCreated = diagnosis.DateCreated,
                 CreatedBy = encounter.CreatedBy,
                 ClientId = diagnosis.ClientId,
                 Client = diagnosis.Client,
                 CreatedIn = diagnosis.CreatedIn,
                 DateModified = diagnosis.DateModified,
                 DiagnosisType = diagnosis.DiagnosisType,
                 EncounterType = diagnosis.EncounterType,
                 ICDDiagnosis = diagnosis.ICDDiagnosis,
                 ICDId = diagnosis.ICDId,
                 InteractionId = diagnosis.InteractionId,
                 IsDeleted = diagnosis.IsDeleted,
                 IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
                 IsSynced = diagnosis.IsSynced,
                 NTGId = diagnosis.NTGId,
                 NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
                 ModifiedBy = diagnosis.ModifiedBy,
                 ModifiedIn = diagnosis.ModifiedIn,
                 Surgery = diagnosis.Surgery,
                 SurgeryId = diagnosis.SurgeryId,
                 ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                 FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

             }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a diagnosis by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a diagnosis if the ClientID is matched.</returns>
        public async Task<IEnumerable<Diagnosis>> GetDiagnosisByClient(Guid ClientID)
        {
            try
            {
                return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
     .Join(
         context.Encounters.AsNoTracking(),
         diagnosis => diagnosis.EncounterId,
         encounter => encounter.Oid,
         (diagnosis, encounter) => new Diagnosis
         {
             EncounterId = diagnosis.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             DateCreated = diagnosis.DateCreated,
             CreatedBy = encounter.CreatedBy,
             ClientId = ClientID,
             Client = diagnosis.Client,
             CreatedIn = diagnosis.CreatedIn,
             DateModified = diagnosis.DateModified,
             DiagnosisType = diagnosis.DiagnosisType,
             EncounterType = diagnosis.EncounterType,
             ICDDiagnosis = diagnosis.ICDDiagnosis,
             ICDId = diagnosis.ICDId,
             InteractionId = diagnosis.InteractionId,
             IsDeleted = diagnosis.IsDeleted,
             IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
             IsSynced = diagnosis.IsSynced,
             NTGId = diagnosis.NTGId,
             NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
             ModifiedBy = diagnosis.ModifiedBy,
             ModifiedIn = diagnosis.ModifiedIn,
             Surgery = diagnosis.Surgery,
             SurgeryId = diagnosis.SurgeryId,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Diagnosis>> GetDiagnosisByClientLast24Hours(Guid ClientID)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.DateCreated >= Last24Hours && p.ClientId == ClientID)
              .Join(
                  context.Encounters.AsNoTracking(),
                  diagnosis => diagnosis.EncounterId,
                  encounter => encounter.Oid,
                  (diagnosis, encounter) => new Diagnosis
                  {
                      EncounterId = diagnosis.EncounterId,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      DateCreated = diagnosis.DateCreated,
                      CreatedBy = encounter.CreatedBy,
                      ClientId = ClientID,
                      Client = diagnosis.Client,
                      CreatedIn = diagnosis.CreatedIn,
                      DateModified = diagnosis.DateModified,
                      DiagnosisType = diagnosis.DiagnosisType,
                      EncounterType = diagnosis.EncounterType,
                      ICDDiagnosis = diagnosis.ICDDiagnosis,
                      ICDId = diagnosis.ICDId,
                      InteractionId = diagnosis.InteractionId,
                      IsDeleted = diagnosis.IsDeleted,
                      IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
                      IsSynced = diagnosis.IsSynced,
                      NTGId = diagnosis.NTGId,
                      NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
                      ModifiedBy = diagnosis.ModifiedBy,
                      ModifiedIn = diagnosis.ModifiedIn,
                      Surgery = diagnosis.Surgery,
                      SurgeryId = diagnosis.SurgeryId,
                      ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                      FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                  }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Diagnosis>> GetDiagnosisByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var diagnosisAsQuerable = context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
  .Join(
      context.Encounters.AsNoTracking(),
      diagnosis => diagnosis.EncounterId,
      encounter => encounter.Oid,
      (diagnosis, encounter) => new Diagnosis
      {
          EncounterId = diagnosis.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          DateCreated = diagnosis.DateCreated,
          CreatedBy = encounter.CreatedBy,
          ClientId = clientId,
          Client = diagnosis.Client,
          CreatedIn = diagnosis.CreatedIn,
          DateModified = diagnosis.DateModified,
          DiagnosisType = diagnosis.DiagnosisType,
          EncounterType = diagnosis.EncounterType,
          ICDDiagnosis = diagnosis.ICDDiagnosis,
          ICDId = diagnosis.ICDId,
          InteractionId = diagnosis.InteractionId,
          IsDeleted = diagnosis.IsDeleted,
          IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
          IsSynced = diagnosis.IsSynced,
          NTGId = diagnosis.NTGId,
          NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
          ModifiedBy = diagnosis.ModifiedBy,
          ModifiedIn = diagnosis.ModifiedIn,
          Surgery = diagnosis.Surgery,
          SurgeryId = diagnosis.SurgeryId,
          ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
          FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


      }).AsQueryable();

                if (encounterType == null)
                    return await diagnosisAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await diagnosisAsQuerable.Where(p => p.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetDiagnosisByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Diagnoses.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Diagnoses.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the last day of all diagnoses.
        /// </summary>
        /// <param name="ClientID">Pirmary key of Client Table</param>
        /// <returns>IEnumerable<Diagnosis></returns>
        public async Task<IEnumerable<Diagnosis>> GetLastDayDiagnosisByClient(Guid ClientID)
        {
            try
            {

                Encounter lastEncounter = await context.Encounters.Where(c => c.ClientId == ClientID).OrderBy(c => c.OPDVisitDate).LastOrDefaultAsync();
                //return context.Diagnoses.Where(i => i.IsDeleted == false && i.ClientId == ClientID && i.EncounterId == lastEncounter.Oid).Include(x => x.ICDDiagnosis)
                //   .Include(x => x.NTGLevelThreeDiagnosis);
                return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == lastEncounter.Oid && p.ClientId == ClientID)
        .Join(
            context.Encounters.AsNoTracking(),
            diagnosis => diagnosis.EncounterId,
            encounter => encounter.Oid,
            (diagnosis, encounter) => new Diagnosis
            {
                EncounterId = diagnosis.EncounterId,
                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                DateCreated = diagnosis.DateCreated,
                CreatedBy = encounter.CreatedBy,
                ClientId = ClientID,
                Client = diagnosis.Client,
                CreatedIn = diagnosis.CreatedIn,
                DateModified = diagnosis.DateModified,
                DiagnosisType = diagnosis.DiagnosisType,
                EncounterType = diagnosis.EncounterType,
                ICDDiagnosis = diagnosis.ICDDiagnosis,
                ICDId = diagnosis.ICDId,
                InteractionId = diagnosis.InteractionId,
                IsDeleted = diagnosis.IsDeleted,
                IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
                IsSynced = diagnosis.IsSynced,
                NTGId = diagnosis.NTGId,
                NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
                ModifiedBy = diagnosis.ModifiedBy,
                ModifiedIn = diagnosis.ModifiedIn,
                Surgery = diagnosis.Surgery,
                SurgeryId = diagnosis.SurgeryId,
                ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


            }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public async Task<IEnumerable<Diagnosis>> GetLastDiagnosisByClient(Guid clientId)
        //{
        //    try
        //    {

        //        Diagnosis lastEncounter = await context.Diagnoses.Where(c => c.ClientId == clientId).OrderBy(c => c.DateCreated).LastOrDefaultAsync();

        //        return context.Diagnoses.Where(i => i.IsDeleted == false && i.ClientId == clientId && i.EncounterId == lastEncounter.EncounterId).Include(x => x.ICDDiagnosis)
        //           .Include(x => x.NTGLevelThreeDiagnosis);

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<Diagnosis>> GetLastDiagnosisByClient(Guid clientId)
        {
            try
            {
                var lastEncounter = await context.Diagnoses.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
.Join(
context.Encounters.AsNoTracking(),
diagnosis => diagnosis.EncounterId,
encounter => encounter.Oid,
(diagnosis, encounter) => new Diagnosis
{
    EncounterId = diagnosis.EncounterId,
    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

}).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

                if (lastEncounter == null)
                {
                    // Handle the case where no last encounter was found.
                    return Enumerable.Empty<Diagnosis>();
                }

                var lastEncounterId = lastEncounter.EncounterId;

                return await
                context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId && p.EncounterId == lastEncounterId)
 .Join(
     context.Encounters.AsNoTracking(),
     diagnosis => diagnosis.EncounterId,
     encounter => encounter.Oid,
     (diagnosis, encounter) => new Diagnosis
     {
         EncounterId = diagnosis.EncounterId,
         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
         DateCreated = diagnosis.DateCreated,
         CreatedBy = encounter.CreatedBy,
         ClientId = clientId,
         Client = diagnosis.Client,
         CreatedIn = diagnosis.CreatedIn,
         DateModified = diagnosis.DateModified,
         DiagnosisType = diagnosis.DiagnosisType,
         EncounterType = diagnosis.EncounterType,
         ICDDiagnosis = diagnosis.ICDDiagnosis,
         ICDId = diagnosis.ICDId,
         InteractionId = diagnosis.InteractionId,
         IsDeleted = diagnosis.IsDeleted,
         IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
         IsSynced = diagnosis.IsSynced,
         NTGId = diagnosis.NTGId,
         NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
         ModifiedBy = diagnosis.ModifiedBy,
         ModifiedIn = diagnosis.ModifiedIn,
         Surgery = diagnosis.Surgery,
         SurgeryId = diagnosis.SurgeryId,
         ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
         FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

     }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Diagnosis>> GetLastEncounterDiagnosisByClient(Guid ClientID)
        {
            try
            {
                var lastEncounter = await context.Diagnoses.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
                                    .Join(
                                     context.Encounters.AsNoTracking(),
                                     diagnosis => diagnosis.EncounterId,
                                     encounter => encounter.Oid,
                                     (diagnosis, encounter) => new Diagnosis
                                     {
                                         EncounterId = diagnosis.EncounterId,
                                         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                                     }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

                // var lastEncounter = await LoadWithChildWithOrderByAsync<Diagnosis>(c => c.ClientId == ClientID && c.IsDeleted == false, orderBy: d => d.OrderByDescending(y => y.DateCreated));
                if (lastEncounter == null)
                    return new List<Diagnosis>();
                else
                    return await context.Diagnoses?.Where(i => i.IsDeleted == false && i.ClientId == ClientID && i.EncounterId == lastEncounter.EncounterId).Include(x => x.ICDDiagnosis)
                       .Include(x => x.NTGLevelThreeDiagnosis).Join(
     context.Encounters.AsNoTracking(),
     diagnosis => diagnosis.EncounterId,
     encounter => encounter.Oid,
     (diagnosis, encounter) => new Diagnosis
     {
         EncounterId = diagnosis.EncounterId,
         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
         DateCreated = diagnosis.DateCreated,
         CreatedBy = encounter.CreatedBy,
         ClientId = ClientID,
         Client = diagnosis.Client,
         CreatedIn = diagnosis.CreatedIn,
         DateModified = diagnosis.DateModified,
         DiagnosisType = diagnosis.DiagnosisType,
         EncounterType = diagnosis.EncounterType,
         ICDDiagnosis = diagnosis.ICDDiagnosis,
         ICDId = diagnosis.ICDId,
         InteractionId = diagnosis.InteractionId,
         IsDeleted = diagnosis.IsDeleted,
         IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
         IsSynced = diagnosis.IsSynced,
         NTGId = diagnosis.NTGId,
         NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
         ModifiedBy = diagnosis.ModifiedBy,
         ModifiedIn = diagnosis.ModifiedIn,
         Surgery = diagnosis.Surgery,
         SurgeryId = diagnosis.SurgeryId,
         ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
         FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

     }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a OPDVisit by key.
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisit.</param>
        /// <param name="encounterType">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPDVisit if the key is matched.</returns>
        public async Task<IEnumerable<Diagnosis>> GetDiagnosisByEncounterType(Guid encounterId, EncounterType? encounterType)
        {
            return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId && p.EncounterType == encounterType)
     .Join(
         context.Encounters.AsNoTracking(),
         diagnosis => diagnosis.EncounterId,
         encounter => encounter.Oid,
         (diagnosis, encounter) => new Diagnosis
         {
             EncounterId = diagnosis.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             DateCreated = diagnosis.DateCreated,
             CreatedBy = encounter.CreatedBy,
             ClientId = diagnosis.ClientId,
             Client = diagnosis.Client,
             CreatedIn = diagnosis.CreatedIn,
             DateModified = diagnosis.DateModified,
             DiagnosisType = diagnosis.DiagnosisType,
             EncounterType = diagnosis.EncounterType,
             ICDDiagnosis = diagnosis.ICDDiagnosis,
             ICDId = diagnosis.ICDId,
             InteractionId = diagnosis.InteractionId,
             IsDeleted = diagnosis.IsDeleted,
             IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
             IsSynced = diagnosis.IsSynced,
             NTGId = diagnosis.NTGId,
             NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
             ModifiedBy = diagnosis.ModifiedBy,
             ModifiedIn = diagnosis.ModifiedIn,
             Surgery = diagnosis.Surgery,
             SurgeryId = diagnosis.SurgeryId,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        }

        /// <summary>
        /// The method is used to get a OPDVisit by key.
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPDVisit if the key is matched.</returns>
        public async Task<IEnumerable<Diagnosis>> GetDiagnosisByOPDVisit(Guid encounterId)
        {
            return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
     .Join(
         context.Encounters.AsNoTracking(),
         diagnosis => diagnosis.EncounterId,
         encounter => encounter.Oid,
         (diagnosis, encounter) => new Diagnosis
         {
             EncounterId = diagnosis.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             DateCreated = diagnosis.DateCreated,
             CreatedBy = encounter.CreatedBy,
             ClientId = diagnosis.ClientId,
             Client = diagnosis.Client,
             CreatedIn = diagnosis.CreatedIn,
             DateModified = diagnosis.DateModified,
             DiagnosisType = diagnosis.DiagnosisType,
             EncounterType = diagnosis.EncounterType,
             ICDDiagnosis = diagnosis.ICDDiagnosis,
             ICDId = diagnosis.ICDId,
             InteractionId = diagnosis.InteractionId,
             IsDeleted = diagnosis.IsDeleted,
             IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
             IsSynced = diagnosis.IsSynced,
             NTGId = diagnosis.NTGId,
             NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
             ModifiedBy = diagnosis.ModifiedBy,
             ModifiedIn = diagnosis.ModifiedIn,
             Surgery = diagnosis.Surgery,
             SurgeryId = diagnosis.SurgeryId,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        }

        public async Task<IEnumerable<Diagnosis>> GetDiagnosisByEncounter(Guid encounterId, EncounterType? encounterType)
        {
            try
            {
                return await LoadListWithChildAsync<Diagnosis>(u => u.EncounterId == encounterId && u.EncounterType==encounterType, p => p.ICDDiagnosis, p => p.NTGLevelThreeDiagnosis);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of diagnoses.
        /// </summary>
        /// <returns>Returns a list of all diagnoses.</returns>
        public async Task<IEnumerable<Diagnosis>> GetDiagnoses()
        {
            try
            {
                return await LoadListWithChildAsync<Diagnosis>(u => u.IsDeleted == false, p => p.ICDDiagnosis, p => p.NTGLevelThreeDiagnosis);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public async Task<Diagnosis> GetLatestDiagnosisByClient(Guid ClientID)
        {
            return await context.Diagnoses.Include(i => i.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
      .Join(
          context.Encounters.AsNoTracking(),
          diagnosis => diagnosis.EncounterId,
          encounter => encounter.Oid,
          (diagnosis, encounter) => new Diagnosis
          {
              EncounterId = diagnosis.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              DateCreated = diagnosis.DateCreated,
              CreatedBy = encounter.CreatedBy,
              ClientId = diagnosis.ClientId,
              Client = diagnosis.Client,
              CreatedIn = diagnosis.CreatedIn,
              DateModified = diagnosis.DateModified,
              DiagnosisType = diagnosis.DiagnosisType,
              EncounterType = diagnosis.EncounterType,
              ICDDiagnosis = diagnosis.ICDDiagnosis,
              ICDId = diagnosis.ICDId,
              InteractionId = diagnosis.InteractionId,
              IsDeleted = diagnosis.IsDeleted,
              IsSelectedForSurgery = diagnosis.IsSelectedForSurgery,
              IsSynced = diagnosis.IsSynced,
              NTGId = diagnosis.NTGId,
              NTGLevelThreeDiagnosis = diagnosis.NTGLevelThreeDiagnosis,
              ModifiedBy = diagnosis.ModifiedBy,
              ModifiedIn = diagnosis.ModifiedIn,
              Surgery = diagnosis.Surgery,
              SurgeryId = diagnosis.SurgeryId,
              ClinicianName = context.UserAccounts.Where(x => x.Oid == diagnosis.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
              FacilityName = context.Facilities.Where(x => x.Oid == diagnosis.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

          }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

        }


    }
}