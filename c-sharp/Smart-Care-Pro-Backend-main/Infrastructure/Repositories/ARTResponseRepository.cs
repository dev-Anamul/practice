using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Dto;
using System.Diagnostics.Metrics;

/*
 * Created by   : Bella
 * Date created : 29.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ARTResponseRepository : Repository<ARTResponse>, IARTResponseRepository
    {
        public ARTResponseRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a ART Response by key.
        /// </summary>
        /// <param name="key">Primary key of the table ART Responses.</param>
        /// <returns>Returns a ART Response if the key is matched.</returns>
        public async Task<ARTResponse> GetARTResponseByKey(Guid key)
        {
            try
            {
                ARTResponse aRTResponse = await context.ARTResponses.AsNoTracking().FirstOrDefaultAsync(c => c.InteractionId == key && c.IsDeleted == false);

                if (aRTResponse is not null)
                {
                    aRTResponse.EncounterDate = await context.Encounters.Where(x => x.Oid == aRTResponse.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    aRTResponse.ClinicianName = await context.UserAccounts.Where(x => x.Oid == aRTResponse.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    aRTResponse.FacilityName = await context.Facilities.Where(x => x.Oid == aRTResponse.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return aRTResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ARTResponses.
        /// </summary>
        /// <returns>Returns a list of all ART Responses.</returns>
        public async Task<IEnumerable<ARTResponse>> GetARTResponses()
        {
            try
            {
                return await context.ARTResponses.AsNoTracking().Where(artService => artService.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        artResponse => artResponse.EncounterId,
                        encounter => encounter.Oid,
                        (artResponse, encounter) => new ARTResponse
                        {
                            InteractionId = artResponse.InteractionId,
                            OnARTMoreThanSixMonths = artResponse.OnARTMoreThanSixMonths,
                            ClinicalMonitoring = artResponse.ClinicalMonitoring,
                            ImmunologicMonitoring = artResponse.ImmunologicMonitoring,
                            VirologicMonitoring = artResponse.VirologicMonitoring,
                            StableOnCareStatus = artResponse.StableOnCareStatus,
                            ClientId = artResponse.ClientId,
                            Client = artResponse.Client,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            CreatedBy = artResponse.CreatedBy,
                            DateCreated = artResponse.DateCreated,
                            EncounterId = artResponse.EncounterId,
                            EncounterType = artResponse.EncounterType,
                            DateModified = artResponse.DateModified,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == artResponse.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == artResponse.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
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
        /// <returns>Returns a OPDVisit if the key is matched.</returns>

        public async Task<IEnumerable<ARTResponse>> GetARTResponseByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.ARTResponses.AsNoTracking().Where(artResponse => artResponse.IsDeleted == false && artResponse.EncounterId == encounterId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        artResponse => artResponse.EncounterId,
                        encounter => encounter.Oid,
                        (artResponse, encounter) => new ARTResponse
                        {
                            InteractionId = artResponse.InteractionId,
                            OnARTMoreThanSixMonths = artResponse.OnARTMoreThanSixMonths,
                            ClinicalMonitoring = artResponse.ClinicalMonitoring,
                            ImmunologicMonitoring = artResponse.ImmunologicMonitoring,
                            VirologicMonitoring = artResponse.VirologicMonitoring,
                            StableOnCareStatus = artResponse.StableOnCareStatus,
                            ClientId = artResponse.ClientId,
                            // Include properties from Client model
                            Client = new Client
                            {
                                // Include client properties you need
                                Oid = artResponse.Client.Oid,
                                FirstName = artResponse.Client.FirstName,
                                Surname = artResponse.Client.Surname,
                                // Add other client properties as needed
                            },
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = artResponse.EncounterId,
                            EncounterType = artResponse.EncounterType,
                            CreatedIn = artResponse.CreatedIn,
                            DateCreated = artResponse.DateCreated,
                            CreatedBy = artResponse.CreatedBy,
                            ModifiedIn = artResponse.ModifiedIn,
                            DateModified = artResponse.DateModified,
                            ModifiedBy = artResponse.ModifiedBy,
                            IsDeleted = artResponse.IsDeleted,
                            IsSynced = artResponse.IsSynced,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == artResponse.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == artResponse.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })

                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ART Response by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ART Response if the ClientID is matched.</returns>
        public async Task<IEnumerable<ARTResponse>> GetARTResponseByClient(Guid clientId)
        {
            try
            {
                return await context.ARTResponses.AsNoTracking().Where(artResponse => artResponse.IsDeleted == false && artResponse.ClientId == clientId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        artResponse => artResponse.EncounterId,
                        encounter => encounter.Oid,
                        (artResponse, encounter) => new ARTResponse
                        {
                            InteractionId = artResponse.InteractionId,
                            OnARTMoreThanSixMonths = artResponse.OnARTMoreThanSixMonths,
                            ClinicalMonitoring = artResponse.ClinicalMonitoring,
                            ImmunologicMonitoring = artResponse.ImmunologicMonitoring,
                            VirologicMonitoring = artResponse.VirologicMonitoring,
                            StableOnCareStatus = artResponse.StableOnCareStatus,
                            ClientId = artResponse.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = artResponse.EncounterId,
                            EncounterType = artResponse.EncounterType,
                            CreatedIn = artResponse.CreatedIn,
                            DateCreated = artResponse.DateCreated,
                            CreatedBy = artResponse.CreatedBy,
                            ModifiedIn = artResponse.ModifiedIn,
                            DateModified = artResponse.DateModified,
                            ModifiedBy = artResponse.ModifiedBy,
                            IsDeleted = artResponse.IsDeleted,
                            IsSynced = artResponse.IsSynced,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == artResponse.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == artResponse.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })

                    .OrderByDescending(a => a.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to Get dot by EncounterID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a dots by EncounterID.</returns>
        public async Task<IEnumerable<ARTResponse>> GetDotByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ARTResponses.AsNoTracking().Where(dot => dot.IsDeleted == false && dot.EncounterId == encounterId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        dot => dot.EncounterId,
                        encounter => encounter.Oid,
                        (dot, encounter) => new ARTResponse
                        {
                            InteractionId = dot.InteractionId,
                            OnARTMoreThanSixMonths = dot.OnARTMoreThanSixMonths,
                            ClinicalMonitoring = dot.ClinicalMonitoring,
                            ImmunologicMonitoring = dot.ImmunologicMonitoring,
                            VirologicMonitoring = dot.VirologicMonitoring,
                            StableOnCareStatus = dot.StableOnCareStatus,
                            ClientId = dot.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = dot.EncounterId,
                            EncounterType = dot.EncounterType,
                            CreatedIn = dot.CreatedIn,
                            DateCreated = dot.DateCreated,
                            CreatedBy = dot.CreatedBy,
                            ModifiedIn = dot.ModifiedIn,
                            DateModified = dot.DateModified,
                            ModifiedBy = dot.ModifiedBy,
                            IsDeleted = dot.IsDeleted,
                            IsSynced = dot.IsSynced,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == dot.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == dot.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })

                    .OrderByDescending(dot => dot.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}