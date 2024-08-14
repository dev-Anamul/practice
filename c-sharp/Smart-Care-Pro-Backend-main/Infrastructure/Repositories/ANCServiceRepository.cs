using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Stephan
 * Last modified: 05.12.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ANCServiceRepository : Repository<ANCService>, IANCServiceRepository
    {
        public ANCServiceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a ANC service by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ANCService if the ClientID is matched.</returns>
        public async Task<IEnumerable<ANCService>> GetANCServiceByClient(Guid clientId)
        {
            try
            {
                var ancService = context.ANCServices.Where(a => a.ClientId == clientId && a.IsDeleted == false).AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    ancService => ancService.EncounterId,
                encounter => encounter.Oid,
                    (ancService, encounter) => new ANCService
                    {
                        Client = ancService.Client,
                        ClientId = ancService.ClientId,
                        CreatedBy = ancService.CreatedBy,
                        CreatedIn = ancService.CreatedIn,
                        DateCreated = ancService.DateCreated,
                        DateModified = ancService.DateModified,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = ancService.EncounterId,
                        EncounterType = ancService.EncounterType,
                        Gravida = ancService.Gravida,
                        HasCurrentPregnancyConcluded = ancService.HasCurrentPregnancyConcluded,
                        InteractionId = ancService.InteractionId,
                        IsDeleted = ancService.IsDeleted,
                        IsSynced = ancService.IsSynced,
                        ModifiedBy = ancService.ModifiedBy,
                        ModifiedIn = ancService.ModifiedIn,
                        MotherhoodNumber = ancService.MotherhoodNumber,
                        OtherReason = ancService.OtherReason,
                        Parity = ancService.Parity,
                        PregnancyConcludedReason = ancService.PregnancyConcludedReason,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == ancService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == ancService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                    }).AsQueryable();

                return await ancService.OrderByDescending(a => a.EncounterDate).ToListAsync();

                //return await QueryAsync(b => b.IsDeleted == false && b.ClientId == clientId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ANCService>> GetANCServiceByClientLast24Hour(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                var ancService = context.ANCServices.Where(a => a.ClientId == clientId && a.DateCreated >= Last24Hours && a.IsDeleted == false).AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    ancService => ancService.EncounterId,
                encounter => encounter.Oid,
                    (ancService, encounter) => new ANCService
                    {
                        Client = ancService.Client,
                        ClientId = ancService.ClientId,
                        CreatedBy = ancService.CreatedBy,
                        CreatedIn = ancService.CreatedIn,
                        DateCreated = ancService.DateCreated,
                        DateModified = ancService.DateModified,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = ancService.EncounterId,
                        EncounterType = ancService.EncounterType,
                        Gravida = ancService.Gravida,
                        HasCurrentPregnancyConcluded = ancService.HasCurrentPregnancyConcluded,
                        InteractionId = ancService.InteractionId,
                        IsDeleted = ancService.IsDeleted,
                        IsSynced = ancService.IsSynced,
                        ModifiedBy = ancService.ModifiedBy,
                        ModifiedIn = ancService.ModifiedIn,
                        MotherhoodNumber = ancService.MotherhoodNumber,
                        OtherReason = ancService.OtherReason,
                        Parity = ancService.Parity,
                        PregnancyConcludedReason = ancService.PregnancyConcludedReason,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == ancService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == ancService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).AsQueryable();

                return await ancService.OrderByDescending(a => a.EncounterDate).ToListAsync();

                //return await QueryAsync(b => b.IsDeleted == false && b.ClientId == clientId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to a ANC service by ClientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<ANCService> GetLatestANCServiceByClient(Guid clientId)
        {
            try
            {
                return await context.ANCServices.AsNoTracking().Where(a => a.ClientId == clientId && a.IsDeleted == false).Include(c => c.Client)
                    .Join(context.Encounters.AsNoTracking(),
                    ancService => ancService.EncounterId,
                encounter => encounter.Oid,
                    (ancService, encounter) => new ANCService
                    {
                        Client = ancService.Client,
                        ClientId = ancService.ClientId,
                        CreatedBy = ancService.CreatedBy,
                        CreatedIn = ancService.CreatedIn,
                        DateCreated = ancService.DateCreated,
                        DateModified = ancService.DateModified,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = ancService.EncounterId,
                        EncounterType = ancService.EncounterType,
                        Gravida = ancService.Gravida,
                        HasCurrentPregnancyConcluded = ancService.HasCurrentPregnancyConcluded,
                        InteractionId = ancService.InteractionId,
                        IsDeleted = ancService.IsDeleted,
                        IsSynced = ancService.IsSynced,
                        ModifiedBy = ancService.ModifiedBy,
                        ModifiedIn = ancService.ModifiedIn,
                        MotherhoodNumber = ancService.MotherhoodNumber,
                        OtherReason = ancService.OtherReason,
                        Parity = ancService.Parity,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == ancService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == ancService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).FirstOrDefaultAsync();


                //  return await LoadWithChildWithOrderByAsync<ANCService>(c => c.ClientId == clientId && c.IsDeleted == false, orderBy: d => d.OrderByDescending(y => y.DateCreated));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ANCService by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ANCService by EncounterID.</returns>
        public async Task<IEnumerable<ANCService>> GetANCServiceByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ANCServices.AsNoTracking().Where(x => x.IsDeleted == false && x.EncounterId == encounterId).Join(
                    context.Encounters.AsNoTracking(),
                    ancService => ancService.EncounterId,
                encounter => encounter.Oid,
                    (ancService, encounter) => new ANCService
                    {
                        Client = ancService.Client,
                        ClientId = ancService.ClientId,
                        CreatedBy = ancService.CreatedBy,
                        CreatedIn = ancService.CreatedIn,
                        DateCreated = ancService.DateCreated,
                        DateModified = ancService.DateModified,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = ancService.EncounterId,
                        EncounterType = ancService.EncounterType,
                        Gravida = ancService.Gravida,
                        HasCurrentPregnancyConcluded = ancService.HasCurrentPregnancyConcluded,
                        InteractionId = ancService.InteractionId,
                        IsDeleted = ancService.IsDeleted,
                        IsSynced = ancService.IsSynced,
                        ModifiedBy = ancService.ModifiedBy,
                        ModifiedIn = ancService.ModifiedIn,
                        MotherhoodNumber = ancService.MotherhoodNumber,
                        OtherReason = ancService.OtherReason,
                        Parity = ancService.Parity,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == ancService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == ancService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();


                /// return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ANCService by key.
        /// </summary>
        /// <param name="key">Primary key of the table ANCServices.</param>
        /// <returns>Returns a ANCService if the key is matched.</returns>
        public async Task<ANCService> GetANCServiceByKey(Guid key)
        {
            try
            {
                ANCService aNCService = await context.ANCServices.AsNoTracking().FirstOrDefaultAsync(x => x.InteractionId == key && x.IsDeleted == false);

                if (aNCService is not null)
                {
                    aNCService.EncounterDate = await context.Encounters.Where(x => x.Oid == aNCService.EncounterId).AsNoTracking().Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();
                    aNCService.ClinicianName = context.UserAccounts.Where(x => x.Oid == aNCService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    aNCService.FacilityName = context.Facilities.Where(x => x.Oid == aNCService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                }

                return aNCService;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ANCServices.
        /// </summary>
        /// <returns>Returns a list of all ANCService.</returns>
        public async Task<IEnumerable<ANCService>> GetANCServices()
        {
            try
            {
                return await context.ANCServices.AsNoTracking()
                    .Join(
                    context.Encounters.AsNoTracking(),
                    ancService => ancService.EncounterId,
                    encounter => encounter.Oid,
                    (ancService, encounter) => new ANCService
                    {
                        Client = ancService.Client,
                        ClientId = ancService.ClientId,
                        CreatedBy = ancService.CreatedBy,
                        CreatedIn = ancService.CreatedIn,
                        DateCreated = ancService.DateCreated,
                        DateModified = ancService.DateModified,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = ancService.EncounterId,
                        EncounterType = ancService.EncounterType,
                        Gravida = ancService.Gravida,
                        HasCurrentPregnancyConcluded = ancService.HasCurrentPregnancyConcluded,
                        InteractionId = ancService.InteractionId,
                        IsDeleted = ancService.IsDeleted,
                        IsSynced = ancService.IsSynced,
                        ModifiedBy = ancService.ModifiedBy,
                        ModifiedIn = ancService.ModifiedIn,
                        MotherhoodNumber = ancService.MotherhoodNumber,
                        OtherReason = ancService.OtherReason,
                        Parity = ancService.Parity,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == ancService.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == ancService.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();

                /// return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}