using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Labib
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IClientsDisabilityRepository interface.
    /// </summary>
    public class ClientsDisabilityRepository : Repository<ClientsDisability>, IClientsDisabilityRepository
    {
        public ClientsDisabilityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a clients disabilities by key.
        /// </summary>
        /// <param name="key">Primary key of the table ClientsDisabilities.</param>
        /// <returns>Returns a clients disabilities if the key is matched.</returns>
        public async Task<ClientsDisability> GetClientsDisabilityByKey(Guid key)
        {
            try
            {
                var clientsDisability = await context.ClientsDisabilities.AsNoTracking().FirstOrDefaultAsync(c => c.EncounterId == key && c.IsDeleted == false);
               
                if (clientsDisability != null)
                {
                    clientsDisability.ClinicianName = await context.UserAccounts.Where(x => x.Oid == clientsDisability.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    clientsDisability.FacilityName = await context.Facilities.Where(x => x.Oid == clientsDisability.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    clientsDisability.EncounterDate = await context.Encounters.Where(x => x.Oid == clientsDisability.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return await context.ClientsDisabilities.AsNoTracking().FirstOrDefaultAsync(c => c.EncounterId == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of clients disabilities.
        /// </summary>
        /// <returns>Returns a list of all clients disabilities.</returns>
        public async Task<IEnumerable<ClientsDisability>> GetClientsDisabilities()
        {
            try
            {
                return await context.ClientsDisabilities.AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                       clientsDisabilities => clientsDisabilities.EncounterId,
                encounter => encounter.Oid,
                       (clientsDisabilities, encounter) => new ClientsDisability
                       {
                           // Properties from ClientsDisability
                           InteractionId = clientsDisabilities.InteractionId,
                           OtherDisability = clientsDisabilities.OtherDisability,
                           DisabilityId = clientsDisabilities.DisabilityId,
                           Disability = clientsDisabilities.Disability,
                           ClientId = clientsDisabilities.ClientId,
                           Client = clientsDisabilities.Client,
                           DisabilityList = clientsDisabilities.DisabilityList,

                           // Properties from EncounterBaseModel
                           EncounterId = clientsDisabilities.EncounterId,
                           EncounterType = clientsDisabilities.EncounterType,
                           CreatedIn = clientsDisabilities.CreatedIn,
                           DateCreated = clientsDisabilities.DateCreated,
                           CreatedBy = clientsDisabilities.CreatedBy,
                           ModifiedIn = clientsDisabilities.ModifiedIn,
                           DateModified = clientsDisabilities.DateModified,
                           ModifiedBy = clientsDisabilities.ModifiedBy,
                           IsDeleted = clientsDisabilities.IsDeleted,
                           IsSynced = clientsDisabilities.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == clientsDisabilities.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == clientsDisabilities.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        /// The method is used to get a Encounter by key.
        /// </summary>
        /// <param name="EncounterID">Primary key of the table Encounter.</param>
        /// <returns>Returns a Encounter if the key is matched.</returns>
        public async Task<IEnumerable<ClientsDisability>> GetClientsDisabilityByEncounter(Guid EncounterID)
        {
            try
            {

                return await context.ClientsDisabilities.AsNoTracking().Where(x => x.IsDeleted == false && x.EncounterId == EncounterID)
                    .Join(
                        context.Encounters.AsNoTracking(),
                       clientsDisabilities => clientsDisabilities.EncounterId,
                encounter => encounter.Oid,
                       (clientsDisabilities, encounter) => new ClientsDisability
                       {
                           // Properties from ClientsDisability
                           InteractionId = clientsDisabilities.InteractionId,
                           OtherDisability = clientsDisabilities.OtherDisability,
                           DisabilityId = clientsDisabilities.DisabilityId,
                           Disability = clientsDisabilities.Disability,
                           ClientId = clientsDisabilities.ClientId,
                           Client = clientsDisabilities.Client,
                           DisabilityList = clientsDisabilities.DisabilityList,

                           // Properties from EncounterBaseModel
                           EncounterId = clientsDisabilities.EncounterId,
                           EncounterType = clientsDisabilities.EncounterType,
                           CreatedIn = clientsDisabilities.CreatedIn,
                           DateCreated = clientsDisabilities.DateCreated,
                           CreatedBy = clientsDisabilities.CreatedBy,
                           ModifiedIn = clientsDisabilities.ModifiedIn,
                           DateModified = clientsDisabilities.DateModified,
                           ModifiedBy = clientsDisabilities.ModifiedBy,
                           IsDeleted = clientsDisabilities.IsDeleted,
                           IsSynced = clientsDisabilities.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == clientsDisabilities.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == clientsDisabilities.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="clientID">Primary key of the table Client.</param>
        /// <returns>Returns a Client if the key is matched.</returns>

        public async Task<IEnumerable<ClientsDisability>> GetClientsDisabilityByClient(Guid clientId)
        {
            try
            {
                return await context.ClientsDisabilities.AsNoTracking().Where(x => x.IsDeleted == false && x.ClientId == clientId)
                   .Join(
                       context.Encounters.AsNoTracking(),
                      clientsDisabilities => clientsDisabilities.EncounterId,
               encounter => encounter.Oid,
                      (clientsDisabilities, encounter) => new ClientsDisability
                      {
                          // Properties from ClientsDisability
                          InteractionId = clientsDisabilities.InteractionId,
                          OtherDisability = clientsDisabilities.OtherDisability,
                          DisabilityId = clientsDisabilities.DisabilityId,
                          Disability = clientsDisabilities.Disability,
                          ClientId = clientsDisabilities.ClientId,
                          Client = clientsDisabilities.Client,
                          DisabilityList = clientsDisabilities.DisabilityList,

                          // Properties from EncounterBaseModel
                          EncounterId = clientsDisabilities.EncounterId,
                          EncounterType = clientsDisabilities.EncounterType,
                          CreatedIn = clientsDisabilities.CreatedIn,
                          DateCreated = clientsDisabilities.DateCreated,
                          CreatedBy = clientsDisabilities.CreatedBy,
                          ModifiedIn = clientsDisabilities.ModifiedIn,
                          DateModified = clientsDisabilities.DateModified,
                          ModifiedBy = clientsDisabilities.ModifiedBy,
                          IsDeleted = clientsDisabilities.IsDeleted,
                          IsSynced = clientsDisabilities.IsSynced,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClinicianName = context.UserAccounts.Where(x => x.Oid == clientsDisabilities.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                          FacilityName = context.Facilities.Where(x => x.Oid == clientsDisabilities.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                      })
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