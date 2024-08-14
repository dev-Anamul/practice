using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/*
 * Created by    : Bithy
 * Date created  : 18.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    public class CovidRepository : Repository<Covid>, ICovidRepository
    {
        public CovidRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a covid by key.
        /// </summary>
        /// <param name="key">Primary key of the table Covids.</param>
        /// <returns>Returns a covid if the key is matched.</returns>
        public async Task<Covid> GetCovidByKey(Guid key)
        {
            try
            {
                var covid = await context.Covids.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (covid != null)
                {
                    covid.ClinicianName = await context.UserAccounts.Where(x => x.Oid == covid.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    covid.FacilityName = await context.Facilities.Where(x => x.Oid == covid.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    covid.EncounterDate = await context.Encounters.Where(x => x.Oid == covid.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return covid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of covids.
        /// </summary>
        /// <returns>Returns a list of all covids.</returns>
        public async Task<IEnumerable<Covid>> GetCovids()
        {
            try
            {
                return await context.Covids.Include(x => x.CovidComorbidities).Include(r => r.ExposureRisks).AsNoTracking().Where(b => b.IsDeleted == false)
                  .Join(
                  context.Encounters.AsNoTracking(),
                  covids => covids.EncounterId,
                  encounter => encounter.Oid,
                  (covids, encounter) => new Covid
                  {
                      // Properties from EncounterBaseModel
                      EncounterId = covids.EncounterId,
                      EncounterType = covids.EncounterType,
                      CreatedIn = encounter.CreatedIn,
                      DateCreated = encounter.DateCreated,
                      CreatedBy = encounter.CreatedBy,
                      ModifiedIn = encounter.ModifiedIn,
                      DateModified = encounter.DateModified,
                      ModifiedBy = encounter.ModifiedBy,
                      IsDeleted = encounter.IsDeleted,
                      IsSynced = encounter.IsSynced,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      // Properties from Covid entity
                      InteractionId = covids.InteractionId,
                      SourceOfAlert = covids.SourceOfAlert,
                      NotificationDate = covids.NotificationDate,
                      OtherCovidSymptom = covids.OtherCovidSymptom,
                      OtherExposureRisk = covids.OtherExposureRisk,
                      IsICUAdmitted = covids.IsICUAdmitted,
                      ICUAdmissionDate = covids.ICUAdmissionDate,
                      IsOnOxygen = covids.IsOnOxygen,
                      OxygenSaturation = covids.OxygenSaturation,
                      ReceivedBPSupport = covids.ReceivedBPSupport,
                      ReceivedVentilatorySupport = covids.ReceivedVentilatorySupport,
                      DateFirstPositive = covids.DateFirstPositive,
                      AnyInternationalTravel = covids.AnyInternationalTravel,
                      TravelDestination = covids.TravelDestination,
                      IsClientHealthCareWorker = covids.IsClientHealthCareWorker,
                      HadCovidExposure = covids.HadCovidExposure,
                      MentalStatusOnAdmission = covids.MentalStatusOnAdmission,
                      HasPneumonia = covids.HasPneumonia,
                      IsPatientHospitalized = covids.IsPatientHospitalized,
                      DateHospitalized = covids.DateHospitalized,
                      IsARDS = covids.IsARDS,
                      OtherComorbiditiesConditions = covids.OtherComorbiditiesConditions,
                      OtherRespiratoryIllness = covids.OtherRespiratoryIllness,
                      ClientId = covids.ClientId,
                      Client = covids.Client,  // Navigation property  
                      CovidComorbidities = covids.CovidComorbidities.ToList(),  // Navigation property
                      ExposureRisks = covids.ExposureRisks.ToList(),  // Navigation property 
                      ClinicianName = context.UserAccounts.Where(x => x.Oid == covids.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                      FacilityName = context.Facilities.Where(x => x.Oid == covids.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
        /// The method is used to get a covid by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a covid if the ClientID is matched.</returns>
        public async Task<IEnumerable<Covid>> GetCovidByClient(Guid clientId)
        {
            try
            {
                return await context.Covids.Include(x => x.CovidComorbidities).Include(r => r.ExposureRisks).AsNoTracking().Where(b => b.IsDeleted == false && b.ClientId == clientId)
                    .Join(
                    context.Encounters.AsNoTracking(),
                    covids => covids.EncounterId,
                    encounter => encounter.Oid,
                    (covids, encounter) => new Covid
                    {
                        // Properties from EncounterBaseModel
                        EncounterId = covids.EncounterId,
                        EncounterType = covids.EncounterType,
                        CreatedIn = encounter.CreatedIn,
                        DateCreated = encounter.DateCreated,
                        CreatedBy = encounter.CreatedBy,
                        ModifiedIn = encounter.ModifiedIn,
                        DateModified = encounter.DateModified,
                        ModifiedBy = encounter.ModifiedBy,
                        IsDeleted = encounter.IsDeleted,
                        IsSynced = encounter.IsSynced,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        // Properties from Covid entity
                        InteractionId = covids.InteractionId,
                        SourceOfAlert = covids.SourceOfAlert,
                        NotificationDate = covids.NotificationDate,
                        OtherCovidSymptom = covids.OtherCovidSymptom,
                        OtherExposureRisk = covids.OtherExposureRisk,
                        IsICUAdmitted = covids.IsICUAdmitted,
                        ICUAdmissionDate = covids.ICUAdmissionDate,
                        IsOnOxygen = covids.IsOnOxygen,
                        OxygenSaturation = covids.OxygenSaturation,
                        ReceivedBPSupport = covids.ReceivedBPSupport,
                        ReceivedVentilatorySupport = covids.ReceivedVentilatorySupport,
                        DateFirstPositive = covids.DateFirstPositive,
                        AnyInternationalTravel = covids.AnyInternationalTravel,
                        TravelDestination = covids.TravelDestination,
                        IsClientHealthCareWorker = covids.IsClientHealthCareWorker,
                        HadCovidExposure = covids.HadCovidExposure,
                        MentalStatusOnAdmission = covids.MentalStatusOnAdmission,
                        HasPneumonia = covids.HasPneumonia,
                        IsPatientHospitalized = covids.IsPatientHospitalized,
                        DateHospitalized = covids.DateHospitalized,
                        IsARDS = covids.IsARDS,
                        OtherComorbiditiesConditions = covids.OtherComorbiditiesConditions,
                        OtherRespiratoryIllness = covids.OtherRespiratoryIllness,
                        ClientId = covids.ClientId,
                        Client = covids.Client,  // Navigation property
                        CovidComorbidities = covids.CovidComorbidities.ToList(),  // Navigation property
                        ExposureRisks = covids.ExposureRisks.ToList(),  // Navigation property
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == covids.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == covids.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Covid>> GetCovidByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);
 
                return await context.Covids.Include(x => x.CovidComorbidities).Include(r => r.ExposureRisks).AsNoTracking().Where(b => b.IsDeleted == false && b.ClientId == clientId && b.DateCreated >= Last24Hours)
                                  .Join(
                                  context.Encounters.AsNoTracking(),
                                  covids => covids.EncounterId,
                                  encounter => encounter.Oid,
                                  (covids, encounter) => new Covid
                                  {
                                      // Properties from EncounterBaseModel
                                      EncounterId = covids.EncounterId,
                                      EncounterType = covids.EncounterType,
                                      CreatedIn = encounter.CreatedIn,
                                      DateCreated = encounter.DateCreated,
                                      CreatedBy = encounter.CreatedBy,
                                      ModifiedIn = encounter.ModifiedIn,
                                      DateModified = encounter.DateModified,
                                      ModifiedBy = encounter.ModifiedBy,
                                      IsDeleted = encounter.IsDeleted,
                                      IsSynced = encounter.IsSynced,
                                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                      // Properties from Covid entity
                                      InteractionId = covids.InteractionId,
                                      SourceOfAlert = covids.SourceOfAlert,
                                      NotificationDate = covids.NotificationDate,
                                      OtherCovidSymptom = covids.OtherCovidSymptom,
                                      OtherExposureRisk = covids.OtherExposureRisk,
                                      IsICUAdmitted = covids.IsICUAdmitted,
                                      ICUAdmissionDate = covids.ICUAdmissionDate,
                                      IsOnOxygen = covids.IsOnOxygen,
                                      OxygenSaturation = covids.OxygenSaturation,
                                      ReceivedBPSupport = covids.ReceivedBPSupport,
                                      ReceivedVentilatorySupport = covids.ReceivedVentilatorySupport,
                                      DateFirstPositive = covids.DateFirstPositive,
                                      AnyInternationalTravel = covids.AnyInternationalTravel,
                                      TravelDestination = covids.TravelDestination,
                                      IsClientHealthCareWorker = covids.IsClientHealthCareWorker,
                                      HadCovidExposure = covids.HadCovidExposure,
                                      MentalStatusOnAdmission = covids.MentalStatusOnAdmission,
                                      HasPneumonia = covids.HasPneumonia,
                                      IsPatientHospitalized = covids.IsPatientHospitalized,
                                      DateHospitalized = covids.DateHospitalized,
                                      IsARDS = covids.IsARDS,
                                      OtherComorbiditiesConditions = covids.OtherComorbiditiesConditions,
                                      OtherRespiratoryIllness = covids.OtherRespiratoryIllness,
                                      ClientId = covids.ClientId,
                                      Client = covids.Client,  // Navigation property
                                      CovidComorbidities = covids.CovidComorbidities.ToList(),  // Navigation property
                                      ExposureRisks = covids.ExposureRisks.ToList(),  // Navigation property
                                      ClinicianName = context.UserAccounts.Where(x => x.Oid == covids.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                      FacilityName = context.Facilities.Where(x => x.Oid == covids.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                                  })
                                  .OrderByDescending(x => x.EncounterDate)
                                  .ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Covid>> GetCovidByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var covidAsQueryable = context.Covids.Include(x => x.CovidComorbidities).Include(r => r.ExposureRisks).AsNoTracking().Where(b => b.IsDeleted == false && b.ClientId == clientId)
                        .Join(
                        context.Encounters.AsNoTracking(),
                        covids => covids.EncounterId,
                        encounter => encounter.Oid,
                        (covids, encounter) => new Covid
                        {
                            // Properties from EncounterBaseModel
                            EncounterId = covids.EncounterId,
                            EncounterType = covids.EncounterType,
                            CreatedIn = encounter.CreatedIn,
                            DateCreated = encounter.DateCreated,
                            CreatedBy = encounter.CreatedBy,
                            ModifiedIn = encounter.ModifiedIn,
                            DateModified = encounter.DateModified,
                            ModifiedBy = encounter.ModifiedBy,
                            IsDeleted = encounter.IsDeleted,
                            IsSynced = encounter.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            // Properties from Covid entity
                            InteractionId = covids.InteractionId,
                            SourceOfAlert = covids.SourceOfAlert,
                            NotificationDate = covids.NotificationDate,
                            OtherCovidSymptom = covids.OtherCovidSymptom,
                            OtherExposureRisk = covids.OtherExposureRisk,
                            IsICUAdmitted = covids.IsICUAdmitted,
                            ICUAdmissionDate = covids.ICUAdmissionDate,
                            IsOnOxygen = covids.IsOnOxygen,
                            OxygenSaturation = covids.OxygenSaturation,
                            ReceivedBPSupport = covids.ReceivedBPSupport,
                            ReceivedVentilatorySupport = covids.ReceivedVentilatorySupport,
                            DateFirstPositive = covids.DateFirstPositive,
                            AnyInternationalTravel = covids.AnyInternationalTravel,
                            TravelDestination = covids.TravelDestination,
                            IsClientHealthCareWorker = covids.IsClientHealthCareWorker,
                            HadCovidExposure = covids.HadCovidExposure,
                            MentalStatusOnAdmission = covids.MentalStatusOnAdmission,
                            HasPneumonia = covids.HasPneumonia,
                            IsPatientHospitalized = covids.IsPatientHospitalized,
                            DateHospitalized = covids.DateHospitalized,
                            IsARDS = covids.IsARDS,
                            OtherComorbiditiesConditions = covids.OtherComorbiditiesConditions,
                            OtherRespiratoryIllness = covids.OtherRespiratoryIllness,
                            ClientId = covids.ClientId,
                            Client = covids.Client,  // Navigation property
                            CovidComorbidities = covids.CovidComorbidities.ToList(),  // Navigation property
                            ExposureRisks = covids.ExposureRisks.ToList(),  // Navigation property
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == covids.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == covids.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                        })
                        .OrderByDescending(x => x.EncounterDate)
                        .AsQueryable();

                if (encounterType == null)
                    return await covidAsQueryable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await covidAsQueryable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetCovidByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Covids.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Covids.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a covid by Encounter.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a covid if the OPD EncounterID is matched.</returns>
        public async Task<IEnumerable<Covid>> GetCovidByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.Covids.Include(x => x.CovidComorbidities).Include(r => r.ExposureRisks).AsNoTracking().Where(b => b.IsDeleted == false && b.EncounterId == EncounterID)
                  .Join(
                  context.Encounters.AsNoTracking(),
                  covids => covids.EncounterId,
                  encounter => encounter.Oid,
                  (covids, encounter) => new Covid
                  {
                      // Properties from EncounterBaseModel
                      EncounterId = covids.EncounterId,
                      EncounterType = covids.EncounterType,
                      CreatedIn = encounter.CreatedIn,
                      DateCreated = encounter.DateCreated,
                      CreatedBy = encounter.CreatedBy,
                      ModifiedIn = encounter.ModifiedIn,
                      DateModified = encounter.DateModified,
                      ModifiedBy = encounter.ModifiedBy,
                      IsDeleted = encounter.IsDeleted,
                      IsSynced = encounter.IsSynced,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      // Properties from Covid entity
                      InteractionId = covids.InteractionId,
                      SourceOfAlert = covids.SourceOfAlert,
                      NotificationDate = covids.NotificationDate,
                      OtherCovidSymptom = covids.OtherCovidSymptom,
                      OtherExposureRisk = covids.OtherExposureRisk,
                      IsICUAdmitted = covids.IsICUAdmitted,
                      ICUAdmissionDate = covids.ICUAdmissionDate,
                      IsOnOxygen = covids.IsOnOxygen,
                      OxygenSaturation = covids.OxygenSaturation,
                      ReceivedBPSupport = covids.ReceivedBPSupport,
                      ReceivedVentilatorySupport = covids.ReceivedVentilatorySupport,
                      DateFirstPositive = covids.DateFirstPositive,
                      AnyInternationalTravel = covids.AnyInternationalTravel,
                      TravelDestination = covids.TravelDestination,
                      IsClientHealthCareWorker = covids.IsClientHealthCareWorker,
                      HadCovidExposure = covids.HadCovidExposure,
                      MentalStatusOnAdmission = covids.MentalStatusOnAdmission,
                      HasPneumonia = covids.HasPneumonia,
                      IsPatientHospitalized = covids.IsPatientHospitalized,
                      DateHospitalized = covids.DateHospitalized,
                      IsARDS = covids.IsARDS,
                      OtherComorbiditiesConditions = covids.OtherComorbiditiesConditions,
                      OtherRespiratoryIllness = covids.OtherRespiratoryIllness,
                      ClientId = covids.ClientId,
                      Client = covids.Client,  // Navigation property 
                      CovidComorbidities = covids.CovidComorbidities,  // Navigation property
                      ExposureRisks = covids.ExposureRisks.ToList(),  // Navigation property 
                      ClinicianName = context.UserAccounts.Where(x => x.Oid == covids.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                      FacilityName = context.Facilities.Where(x => x.Oid == covids.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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