using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class KeyPopulationDemographicRepository : Repository<KeyPopulationDemographic>, IKeyPopulationDemographicRepository
    {
        public KeyPopulationDemographicRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get KeyPopulationDemographic by key.
        /// </summary>
        /// <param name="key">Primary key of the table KeyPopulationDemographic.</param>
        /// <returns>Returns a KeyPopulationDemographic if the key is matched.</returns>
        public async Task<KeyPopulationDemographic> GetKeyPopulationDemographicByKey(Guid key)
        {
            try
            {
                var keyPopulationDemographic = await FirstOrDefaultAsync(k => k.InteractionId == key && k.IsDeleted == false);

                if(keyPopulationDemographic != null)
                 keyPopulationDemographic.EncounterDate= await context.Encounters.Select(e => e.OPDVisitDate?? e.IPDAdmissionDate?? e.DateCreated).FirstOrDefaultAsync();

                return keyPopulationDemographic;

                ///return await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a KeyPopulationDemographic by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a KeyPopulationDemographic if the ClientID is matched.</returns>
        public async Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographicByClient(Guid clientId)
        {
            try
            {
                return await context.KeyPopulationDemographics.AsNoTracking().Where(k => k.IsDeleted == false && k.ClientId == clientId).AsNoTracking()
                    .Join(
                    context.Encounters.AsNoTracking(),
                    keyPopulationDemographic => keyPopulationDemographic.EncounterId,
                    encounter => encounter.Oid,
                    (keyPopulationDemographic, encounter) => new KeyPopulationDemographic
                    {
                        ClientId=keyPopulationDemographic.ClientId,
                        CreatedBy=keyPopulationDemographic.CreatedBy,
                        CreatedIn=keyPopulationDemographic.CreatedIn,
                        DateCreated=keyPopulationDemographic.DateCreated,
                        DateModified=keyPopulationDemographic.DateModified,
                        EncounterDate= encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId=keyPopulationDemographic.EncounterId,
                        EncounterType=keyPopulationDemographic.EncounterType,
                        InteractionId=keyPopulationDemographic.InteractionId,
                        IsDeleted=keyPopulationDemographic.IsDeleted,
                        IsSynced=keyPopulationDemographic.IsSynced,
                        KeyPopulationId=keyPopulationDemographic.KeyPopulationId,
                        KeyPopulation=keyPopulationDemographic.KeyPopulation,
                        ModifiedBy=keyPopulationDemographic.ModifiedBy,
                        ModifiedIn=keyPopulationDemographic.ModifiedIn,
                    }).OrderByDescending(e => e.EncounterDate).ToListAsync();

               /// return await LoadListWithChildAsync<KeyPopulationDemographic>(b => b.IsDeleted == false && b.ClientId == clientId, p => p.KeyPopulation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of KeyPopulationDemographics.
        /// </summary>
        /// <returns>Returns a list of all KeyPopulationDemographics.</returns>
        public async Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographics()
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
        /// The method is used to get a KeyPopulationDemographic by encounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a KeyPopulationDemographic if the encounterId is matched.</returns>
        public async Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographicByEncounter(Guid encounterId)
        {
            try
            {

                return await context.KeyPopulationDemographics.AsNoTracking().Where(k => k.IsDeleted == false && k.EncounterId == encounterId).AsNoTracking()
                   .Join(
                   context.Encounters.AsNoTracking(),
                   keyPopulationDemographic => keyPopulationDemographic.EncounterId,
                   encounter => encounter.Oid,
                   (keyPopulationDemographic, encounter) => new KeyPopulationDemographic
                   {
                       ClientId = keyPopulationDemographic.ClientId,
                       CreatedBy = keyPopulationDemographic.CreatedBy,
                       CreatedIn = keyPopulationDemographic.CreatedIn,
                       DateCreated = keyPopulationDemographic.DateCreated,
                       DateModified = keyPopulationDemographic.DateModified,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       EncounterId = keyPopulationDemographic.EncounterId,
                       EncounterType = keyPopulationDemographic.EncounterType,
                       InteractionId = keyPopulationDemographic.InteractionId,
                       IsDeleted = keyPopulationDemographic.IsDeleted,
                       IsSynced = keyPopulationDemographic.IsSynced,
                       KeyPopulationId = keyPopulationDemographic.KeyPopulationId,
                       KeyPopulation = keyPopulationDemographic.KeyPopulation,
                       ModifiedBy = keyPopulationDemographic.ModifiedBy,
                       ModifiedIn = keyPopulationDemographic.ModifiedIn,
                   }).OrderByDescending(e => e.EncounterDate).ToListAsync();

                ///return await LoadListWithChildAsync<KeyPopulationDemographic>(c => c.IsDeleted == false && c.EncounterId == encounterId, p => p.KeyPopulation);
            }
            catch (Exception)
            {
                throw;
            }
        } 
        public async Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographicByEncounterIdEncounterType(Guid encounterId, EncounterType encouterType)
        {
            try
            {
                return await LoadListWithChildAsync<KeyPopulationDemographic>(c => c.IsDeleted == false && c.EncounterId == encounterId&&c.EncounterType==encouterType, p => p.KeyPopulation);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}