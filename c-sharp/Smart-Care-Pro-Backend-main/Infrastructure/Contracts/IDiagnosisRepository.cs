using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDiagnosisRepository : IRepository<Diagnosis>
    {
        /// <summary>
        /// The method is used to get a diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table Diagnosis.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public Task<Diagnosis> GetDiagnosisByKey(Guid key);

        /// <summary>
        /// The method is used to get a diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table Diagnosis.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public Task<IEnumerable<Diagnosis>> ReadDiagnosisBySurgery(Guid key);


        /// <summary>
        /// The method is used to get a diagnosis by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a diagnosis if the ClientID is matched.</returns>
        public Task<IEnumerable<Diagnosis>> GetDiagnosisByClient(Guid ClientID);
        public Task<IEnumerable<Diagnosis>> GetDiagnosisByClientLast24Hours(Guid ClientID);
        public Task<IEnumerable<Diagnosis>> GetDiagnosisByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetDiagnosisByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table Diagnosis.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public Task<IEnumerable<Diagnosis>> GetDiagnosisBySurgeryId(Guid key);

        /// <summary>
        /// The method is used to get a diagnosis by EncounterId.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public Task<IEnumerable<Diagnosis>> GetDiagnosisByOPDVisit(Guid encounterId);

        /// <summary>
        /// The method is used to get a diagnosis by EncounterId and EncounterType.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <param name="encounterType">Primary key of the table EncounterType.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public Task<IEnumerable<Diagnosis>> GetDiagnosisByEncounterType(Guid encounterId , EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a diagnosis by EncounterId and EncounterType.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <param name="encounterType">Primary key of the table EncounterType.</param>
        /// <returns>Returns a diagnosis if the key is matched.</returns>
        public Task<IEnumerable<Diagnosis>> GetDiagnosisByEncounter(Guid encounterId, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of diagnoses.
        /// </summary>
        /// <returns>Returns a list of all diagnoses.</returns>
        public Task<IEnumerable<Diagnosis>> GetDiagnoses();

        /// <summary>
        /// The method is used to get the last day of all diagnoses.
        /// </summary>
        /// <param name="ClientID">Pirmary key of Client Table</param>
        /// <returns>IEnumerable<Diagnosis></returns>
        public Task<IEnumerable<Diagnosis>> GetLastDayDiagnosisByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get the last Diganosi of Client.
        /// </summary>
        /// <param name="ClientID">Pirmary key of Client Table</param>
        /// <returns>IEnumerable<Diagnosis></returns>
        public Task<IEnumerable<Diagnosis>> GetLastDiagnosisByClient(Guid clientId);

        /// <summary>
        /// The method is used to get the last diagnosis.
        /// </summary>
        /// <param name="ClientID">Pirmary key of Client Table</param>
        /// <returns>IEnumerable<Diagnosis></returns>
        public Task<Diagnosis> GetLatestDiagnosisByClient(Guid ClientID);

        public Task<IEnumerable<Diagnosis>> GetLastEncounterDiagnosisByClient(Guid ClientID);

    }
}