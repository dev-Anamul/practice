using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

/*
 * Created by   : Lion
 * Date created : 15-08-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ANCScreeningRepository : Repository<ANCScreening>, IANCScreeningRepository
    {
        public ANCScreeningRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ANCScreening if the ClientID is matched.</returns>

        public async Task<IEnumerable<ANCScreening>> GetANCScreeningByClient(Guid clientId)
        {
            try
            {
                return await context.ANCScreenings.AsNoTracking().Where(x => x.ClientId == clientId && x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        ancScreening => ancScreening.EncounterId,
                        encounter => encounter.Oid,
                        (ancScreening, encounter) => new ANCScreening
                        {
                            InteractionId = ancScreening.InteractionId,
                            HistoryofBleeding = ancScreening.HistoryofBleeding,
                            Draining = ancScreening.Draining,
                            PVMucus = ancScreening.PVMucus,
                            Contraction = ancScreening.Contraction,
                            PVBleeding = ancScreening.PVBleeding,
                            FetalMovements = ancScreening.FetalMovements,
                            IsSyphilisDone = ancScreening.IsSyphilisDone,
                            SyphilisTestDate = ancScreening.SyphilisTestDate,
                            SyphilisTestType = ancScreening.SyphilisTestType,
                            SyphilisResult = ancScreening.SyphilisResult,
                            IsHepatitisDone = ancScreening.IsHepatitisDone,
                            HepatitisTestDate = ancScreening.HepatitisTestDate,
                            HepatitisTestType = ancScreening.HepatitisTestType,
                            HepatitisResult = ancScreening.HepatitisResult,
                            ClientId = ancScreening.ClientId,
                            Client = ancScreening.Client,
                            // Add other properties as needed
                            EncounterId = ancScreening.EncounterId,
                            EncounterType = ancScreening.EncounterType,
                            CreatedBy = ancScreening.CreatedBy,
                            CreatedIn = ancScreening.CreatedIn,
                            DateCreated = ancScreening.DateCreated,
                            DateModified = ancScreening.DateModified,
                            ModifiedBy = ancScreening.ModifiedBy,
                            ModifiedIn = ancScreening.ModifiedIn,
                            IsDeleted = ancScreening.IsDeleted,
                            IsSynced = ancScreening.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == ancScreening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == ancScreening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                        }).OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ANCScreening>> GetANCScreeningByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.ANCScreenings.AsNoTracking().Where(x => x.ClientId == clientId && x.DateCreated >= Last24Hours && x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        ancScreening => ancScreening.EncounterId,
                        encounter => encounter.Oid,
                        (ancScreening, encounter) => new ANCScreening
                        {
                            InteractionId = ancScreening.InteractionId,
                            HistoryofBleeding = ancScreening.HistoryofBleeding,
                            Draining = ancScreening.Draining,
                            PVMucus = ancScreening.PVMucus,
                            Contraction = ancScreening.Contraction,
                            PVBleeding = ancScreening.PVBleeding,
                            FetalMovements = ancScreening.FetalMovements,
                            IsSyphilisDone = ancScreening.IsSyphilisDone,
                            SyphilisTestDate = ancScreening.SyphilisTestDate,
                            SyphilisTestType = ancScreening.SyphilisTestType,
                            SyphilisResult = ancScreening.SyphilisResult,
                            IsHepatitisDone = ancScreening.IsHepatitisDone,
                            HepatitisTestDate = ancScreening.HepatitisTestDate,
                            HepatitisTestType = ancScreening.HepatitisTestType,
                            HepatitisResult = ancScreening.HepatitisResult,
                            ClientId = ancScreening.ClientId,
                            Client = ancScreening.Client,
                            // Add other properties as needed
                            EncounterId = ancScreening.EncounterId,
                            EncounterType = ancScreening.EncounterType,
                            CreatedBy = ancScreening.CreatedBy,
                            CreatedIn = ancScreening.CreatedIn,
                            DateCreated = ancScreening.DateCreated,
                            DateModified = ancScreening.DateModified,
                            ModifiedBy = ancScreening.ModifiedBy,
                            ModifiedIn = ancScreening.ModifiedIn,
                            IsDeleted = ancScreening.IsDeleted,
                            IsSynced = ancScreening.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == ancScreening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == ancScreening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                        }).OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ANCScreening by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ANCScreening by EncounterID.</returns>
        public async Task<IEnumerable<ANCScreening>> GetANCScreeningByEncounter(Guid encounterId)
        {
            try
            {
                var ancScreening = context.ANCScreenings.Where(a => a.IsDeleted == false && a.EncounterId == encounterId).AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    ancScreening => ancScreening.EncounterId,
                    encounter => encounter.Oid,
                    (ancScreening, encounter) => new ANCScreening
                    {
                        Client = ancScreening.Client,
                        ClientId = ancScreening.ClientId,
                        Contraction = ancScreening.Contraction,
                        CreatedBy = ancScreening.CreatedBy,
                        CreatedIn = ancScreening.CreatedIn,
                        DateCreated = ancScreening.DateCreated,
                        DateModified = ancScreening.DateModified,
                        Draining = ancScreening.Draining,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = ancScreening.EncounterId,
                        EncounterType = ancScreening.EncounterType,
                        FetalMovements = ancScreening.FetalMovements,
                        HepatitisResult = ancScreening.HepatitisResult,
                        HepatitisTestDate = ancScreening.HepatitisTestDate,
                        HepatitisTestType = ancScreening.HepatitisTestType,
                        IsHepatitisDone = ancScreening.IsHepatitisDone,
                        HistoryofBleeding = ancScreening.HistoryofBleeding,
                        InteractionId = ancScreening.InteractionId,
                        IsDeleted = ancScreening.IsDeleted,
                        IsSynced = ancScreening.IsSynced,
                        IsSyphilisDone = ancScreening.IsSyphilisDone,
                        ModifiedBy = ancScreening.ModifiedBy,
                        ModifiedIn = ancScreening.ModifiedIn,
                        PVBleeding = ancScreening.PVBleeding,
                        PVMucus = ancScreening.PVMucus,
                        SyphilisResult = ancScreening.SyphilisResult,
                        SyphilisTestDate = ancScreening.SyphilisTestDate,
                        SyphilisTestType = ancScreening.SyphilisTestType,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == ancScreening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == ancScreening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                    })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();

                return await ancScreening;
                /// return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ANCScreening by key.
        /// </summary>
        /// <param name="key">Primary key of the table ANCScreenings.</param>
        /// <returns>Returns a ANCScreening if the key is matched.</returns>
        public async Task<ANCScreening> GetANCScreeningByKey(Guid key)
        {
            try
            {
                ANCScreening aNCScreening = await context.ANCScreenings.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
             
                if (aNCScreening is not null)
                {
                    aNCScreening.EncounterDate = await context.Encounters.Where(x => x.Oid == aNCScreening.EncounterId).Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();
                    aNCScreening.ClinicianName = context.UserAccounts.Where(x => x.Oid == aNCScreening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    aNCScreening.FacilityName = context.Facilities.Where(x => x.Oid == aNCScreening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                }

                return aNCScreening;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ANCScreenings.
        /// </summary>
        /// <returns>Returns a list of all ANCScreening.</returns>
        public async Task<IEnumerable<ANCScreening>> GetANCScreenings()
        {
            try
            {
                return await context.ANCScreenings.AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    ancScreening => ancScreening.EncounterId,
                     encounter => encounter.Oid,
                    (ancScreening, encounter) => new ANCScreening
                    {
                        Client = ancScreening.Client,
                        ClientId = ancScreening.ClientId,
                        Contraction = ancScreening.Contraction,
                        CreatedBy = ancScreening.CreatedBy,
                        CreatedIn = ancScreening.CreatedIn,
                        DateCreated = ancScreening.DateCreated,
                        DateModified = ancScreening.DateModified,
                        Draining = ancScreening.Draining,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = ancScreening.EncounterId,
                        EncounterType = ancScreening.EncounterType,
                        FetalMovements = ancScreening.FetalMovements,
                        HepatitisResult = ancScreening.HepatitisResult,
                        HepatitisTestDate = ancScreening.HepatitisTestDate,
                        HepatitisTestType = ancScreening.HepatitisTestType,
                        IsHepatitisDone = ancScreening.IsHepatitisDone,
                        HistoryofBleeding = ancScreening.HistoryofBleeding,
                        InteractionId = ancScreening.InteractionId,
                        IsDeleted = ancScreening.IsDeleted,
                        IsSynced = ancScreening.IsSynced,
                        IsSyphilisDone = ancScreening.IsSyphilisDone,
                        ModifiedBy = ancScreening.ModifiedBy,
                        ModifiedIn = ancScreening.ModifiedIn,
                        PVBleeding = ancScreening.PVBleeding,
                        PVMucus = ancScreening.PVMucus,
                        SyphilisResult = ancScreening.SyphilisResult,
                        SyphilisTestDate = ancScreening.SyphilisTestDate,
                        SyphilisTestType = ancScreening.SyphilisTestType,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == ancScreening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == ancScreening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();

                ///return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}