using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class MedicationRepository : Repository<Medication>, IMedicationRepository
    {
        public MedicationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a GeneralMedication by key.
        /// </summary>
        /// <param name="key">Primary key of the table GeneralMedication.</param>
        /// <returns>Returns a GeneralMedication if the key is matched.</returns>
        public async Task<Medication> GetGeneralMedicationByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.InteractionId == key && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MedicationListDto> GetGeneralDispenseDetailByKey(Guid key)
        {
            try
            {
                var medicationInDb = await LoadWithChildAsync<Medication>(d => d.IsDeleted == false && d.InteractionId == key && d.Prescription.DispensationDate != null, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute, dr => dr.DrugRoute);
                //return await FirstOrDefaultAsync(m => m.InteractionId == key && m.IsDeleted == false);

                var clinician = new UserAccount();

                var clinicianFacility = new Facility();

                if (medicationInDb != null)
                {
                    if (medicationInDb.CreatedBy != null && medicationInDb.CreatedBy != Guid.Empty)
                    {
                        clinician = context.UserAccounts.Where(x => x.Oid == medicationInDb.CreatedBy.Value).FirstOrDefault();
                    }
                    if (medicationInDb.CreatedIn != null)
                    {
                        clinicianFacility = context.Facilities.Where(x => x.Oid == medicationInDb.CreatedIn.Value).FirstOrDefault();
                    }
                }

                return new MedicationListDto()
                {
                    AdditionalDrugFormulation = medicationInDb.AdditionalDrugFormulation,
                    AdditionalDrugTitle = medicationInDb.AdditionalDrugTitle,
                    GenericDrug = medicationInDb.GeneralDrugDefinition != null ? medicationInDb.GeneralDrugDefinition.GenericDrug.Description : "",
                    GenericDrugsDescription = medicationInDb.GeneralDrugDefinition != null ? medicationInDb.GeneralDrugDefinition.Description : "",
                    GenericDrugsFormulationsDescription = medicationInDb.GeneralDrugDefinition != null ? medicationInDb.GeneralDrugDefinition.DrugFormulation.Description : "",
                    GenericDrugsDrugDosageUnitesDescription = medicationInDb.GeneralDrugDefinition != null ? medicationInDb.GeneralDrugDefinition.DrugDosageUnit.Description : "",
                    SpecialDrugDescription = medicationInDb.SpecialDrug != null ? medicationInDb.SpecialDrug.Description : "",
                    SpecialDrugFormulations = medicationInDb.SpecialDrug != null ? medicationInDb.SpecialDrug.DrugFormulation.Description : "",
                    SpecialDrugDrugDosageUnitsDescription = medicationInDb.SpecialDrug != null ? medicationInDb.SpecialDrug.DrugDosageUnit.Description : "",
                    FrequencyIntervalTimeInterval = medicationInDb.FrequencyInterval != null ? medicationInDb.FrequencyInterval.Description : "",
                    PrescribedDosage = medicationInDb.PrescribedDosage,
                    IsPasserBy = medicationInDb.IsPasserBy,
                    IsDispencedPasserBy = medicationInDb.IsDispencedPasserBy,
                    DrugRoute_Route = medicationInDb.DrugRoute.Description,
                    DispenseDrugRouteRoute = medicationInDb.DispensedDrugsRouteId == null ? "" : context.DrugRoutes.Where(z => z.Oid == medicationInDb.DispensedDrugsRouteId.Value).FirstOrDefault().Description,
                    Duration = medicationInDb.Duration,
                    TimeUnit = medicationInDb.TimeUnit,
                    Frequency = medicationInDb.Frequency,
                    DispensedDrugsFrequency = medicationInDb.DispensedDrugsFrequency,
                    DispensedDrugsQuantity = medicationInDb.DispensedDrugsQuantity,
                    DispansedDrugsDosage = medicationInDb.DispensedDrugsDosage,
                    DispansedDrugTitle = medicationInDb.DispensedDrugTitle,
                    DispensedDrugDuration = medicationInDb.DispensedDrugDuration,
                    DispensedDrugsBrand = medicationInDb.DispensedDrugsBrand,
                    DispensedDrugsEndDate = medicationInDb.DispensedDrugsEndDate,
                    DispensedDrugsFormulation = medicationInDb.DispensedDrugsFormulation,
                    DispensedDrugsFrequencyIntervalId = medicationInDb.DispensedDrugsFrequencyIntervalId,
                    DispensedDrugsItemPerDose = medicationInDb.DispensedDrugsItemPerDose,
                    DispensedDrugsRuteId = medicationInDb.DispensedDrugsRouteId,
                    DispensedDrugsStartDate = medicationInDb.DispensedDrugsStartDate,
                    DispensedDrugsTimeUnit = medicationInDb.DispensedDrugsTimeUnit,
                    DosageUnit = medicationInDb.DosageUnit,
                    EndDate = medicationInDb.EndDate,
                    FrequencyIntervalId = medicationInDb.FrequencyIntervalId,
                    StartDate = medicationInDb.StartDate,
                    ItemPerDose = medicationInDb.ItemPerDose,
                    PrescribedQuentity = medicationInDb.PrescribedQuantity,
                    ReasonForReplacement = medicationInDb.ReasonForReplacement,
                    Note = medicationInDb.Note,
                    MedicationInteractionId = medicationInDb.InteractionId,
                    PrescriptionDate = medicationInDb.Prescription.PrescriptionDate,
                    DispensationDate = medicationInDb.Prescription.DispensationDate,
                    CreatedBy = medicationInDb.CreatedBy,
                    ClinicianName = clinician != null ? clinician.FirstName + " " + clinician.Surname : "",
                    FacilityName = clinicianFacility != null ? clinicianFacility.Description : "",
                    CreatedIn = medicationInDb.CreatedIn
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of GeneralMedication.
        /// </summary>
        /// <returns>Returns a list of all GeneralMedication.</returns>        
        public async Task<IEnumerable<Medication>> GetGeneralMedication(Guid clientId)
        {
            try
            {
                //return await QueryAsync(d => d.IsDeleted == false);
                return await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false && d.Prescription.ClientId == clientId, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute);
                // return await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false, dd => dd.GeneralDrugDefinition, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, dr => dr.DrugRoute);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<MedicationDto>> GetGeneralMedicationByClient(Guid clientId)
        {
            try
            {
                var medicationInDb = await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false && d.Prescription.ClientId == clientId, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute, dr => dr.DrugRoute);

                return medicationInDb.GroupBy(c => new
                {
                    c.Prescription.EncounterId,
                    c.Prescription.InteractionId,

                }).Select(x => new MedicationDto()
                {
                    PrescriptionId = x.Key.InteractionId,
                    EncounterId = x.Key.EncounterId,

                    PrescriptionDate = x.Select(x => x.Prescription.PrescriptionDate).FirstOrDefault(),
                    DispensationDate = x.Select(x => x.Prescription.DispensationDate).FirstOrDefault(),
                    MedicationsList = x.Select(x => new MedicationListDto()
                    {
                        AdditionalDrugFormulation = x.AdditionalDrugFormulation,
                        AdditionalDrugTitle = x.AdditionalDrugTitle,
                        GenericDrug = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.GenericDrug.Description : "",
                        GenericDrugsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.Description : "",
                        GenericDrugsFormulationsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugFormulation.Description : "",
                        GenericDrugsDrugDosageUnitesDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugDosageUnit.Description : "",
                        SpecialDrugDescription = x.SpecialDrug != null ? x.SpecialDrug.Description : "",
                        SpecialDrugFormulations = x.SpecialDrug != null ? x.SpecialDrug.DrugFormulation.Description : "",
                        SpecialDrugDrugDosageUnitsDescription = x.SpecialDrug != null ? x.SpecialDrug.DrugDosageUnit.Description : "",
                        FrequencyIntervalTimeInterval = x.FrequencyInterval != null ? x.FrequencyInterval.Description : "",
                        PrescribedDosage = x.PrescribedDosage,
                        DrugRoute_Route = x.DrugRoute.Description,
                        DispenseDrugRouteRoute = x.DispensedDrugsRouteId == null ? "" : context.DrugRoutes.Where(z => z.Oid == x.DispensedDrugsRouteId.Value).FirstOrDefault().Description,
                        Duration = x.Duration,
                        TimeUnit = x.TimeUnit,
                        Frequency = x.Frequency,
                        IsPasserBy = x.IsPasserBy,
                        IsDispencedPasserBy = x.IsDispencedPasserBy,
                        DispensedDrugsFrequency = x.DispensedDrugsFrequency,
                        DispensedDrugsQuantity = x.DispensedDrugsQuantity,
                        DispansedDrugsDosage = x.DispensedDrugsDosage,
                        DispansedDrugTitle = x.DispensedDrugTitle,
                        DispensedDrugDuration = x.DispensedDrugDuration,
                        DispensedDrugsBrand = x.DispensedDrugsBrand,
                        DispensedDrugsEndDate = x.DispensedDrugsEndDate,
                        DispensedDrugsFormulation = x.DispensedDrugsFormulation,
                        DispensedDrugsFrequencyIntervalId = x.DispensedDrugsFrequencyIntervalId,
                        DispensedDrugsItemPerDose = x.DispensedDrugsItemPerDose,
                        DispensedDrugsRuteId = x.DispensedDrugsRouteId,
                        DispensedDrugsStartDate = x.DispensedDrugsStartDate,
                        DispensedDrugsTimeUnit = x.DispensedDrugsTimeUnit,
                        DosageUnit = x.DosageUnit,
                        EndDate = x.EndDate,
                        FrequencyIntervalId = x.FrequencyIntervalId,
                        StartDate = x.StartDate,
                        ItemPerDose = x.ItemPerDose,
                        PrescribedQuentity = x.PrescribedQuantity,
                        ReasonForReplacement = x.ReasonForReplacement,
                        Note = x.Note,
                        MedicationInteractionId = x.InteractionId,
                        PrescriptionDate = x.Prescription.PrescriptionDate,
                        DispensationDate = x.Prescription.DispensationDate,
                        CreatedBy = x.CreatedBy,
                        CreatedIn = x.CreatedIn

                    }).ToList(),

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<MedicationDto>> GetGeneralMedicationByClient(Guid clientId, int page, int pageSize)
        {
            try
            {
                //  var medicationInDb = await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false && d.Prescription.ClientId == clientId, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute, dr => dr.DrugRoute);

                var medicationInDb = await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false && d.Prescription.ClientId == clientId, page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute, dr => dr.DrugRoute);

                return medicationInDb.GroupBy(c => new
                {
                    c.Prescription.EncounterId,
                    c.Prescription.InteractionId,

                }).Select(x => new MedicationDto()
                {
                    PrescriptionId = x.Key.InteractionId,
                    EncounterId = x.Key.EncounterId,

                    PrescriptionDate = x.Select(x => x.Prescription.PrescriptionDate).FirstOrDefault(),
                    DispensationDate = x.Select(x => x.Prescription.DispensationDate).FirstOrDefault(),
                    MedicationsList = x.Select(x => new MedicationListDto()
                    {
                        AdditionalDrugFormulation = x.AdditionalDrugFormulation,
                        AdditionalDrugTitle = x.AdditionalDrugTitle,
                        GenericDrug = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.GenericDrug.Description : "",
                        GenericDrugsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.Description : "",
                        GenericDrugsFormulationsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugFormulation.Description : "",
                        GenericDrugsDrugDosageUnitesDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugDosageUnit.Description : "",
                        SpecialDrugDescription = x.SpecialDrug != null ? x.SpecialDrug.Description : "",
                        SpecialDrugFormulations = x.SpecialDrug != null ? x.SpecialDrug.DrugFormulation.Description : "",
                        SpecialDrugDrugDosageUnitsDescription = x.SpecialDrug != null ? x.SpecialDrug.DrugDosageUnit.Description : "",
                        FrequencyIntervalTimeInterval = x.FrequencyInterval != null ? x.FrequencyInterval.Description : "",
                        PrescribedDosage = x.PrescribedDosage,
                        DrugRoute_Route = x.DrugRoute.Description,
                        DispenseDrugRouteRoute = x.DispensedDrugsRouteId == null ? "" : context.DrugRoutes.Where(z => z.Oid == x.DispensedDrugsRouteId.Value).FirstOrDefault().Description,
                        Duration = x.Duration,
                        TimeUnit = x.TimeUnit,
                        Frequency = x.Frequency,
                        IsPasserBy = x.IsPasserBy,
                        IsDispencedPasserBy = x.IsDispencedPasserBy,
                        DispensedDrugsFrequency = x.DispensedDrugsFrequency,
                        DispensedDrugsQuantity = x.DispensedDrugsQuantity,
                        DispansedDrugsDosage = x.DispensedDrugsDosage,
                        DispansedDrugTitle = x.DispensedDrugTitle,
                        DispensedDrugDuration = x.DispensedDrugDuration,
                        DispensedDrugsBrand = x.DispensedDrugsBrand,
                        DispensedDrugsEndDate = x.DispensedDrugsEndDate,
                        DispensedDrugsFormulation = x.DispensedDrugsFormulation,
                        DispensedDrugsFrequencyIntervalId = x.DispensedDrugsFrequencyIntervalId,
                        DispensedDrugsItemPerDose = x.DispensedDrugsItemPerDose,
                        DispensedDrugsRuteId = x.DispensedDrugsRouteId,
                        DispensedDrugsStartDate = x.DispensedDrugsStartDate,
                        DispensedDrugsTimeUnit = x.DispensedDrugsTimeUnit,
                        DosageUnit = x.DosageUnit,
                        EndDate = x.EndDate,
                        FrequencyIntervalId = x.FrequencyIntervalId,
                        StartDate = x.StartDate,
                        ItemPerDose = x.ItemPerDose,
                        PrescribedQuentity = x.PrescribedQuantity,
                        ReasonForReplacement = x.ReasonForReplacement,
                        Note = x.Note,
                        MedicationInteractionId = x.InteractionId,
                        PrescriptionDate = x.Prescription.PrescriptionDate,
                        DispensationDate = x.Prescription.DispensationDate,
                        CreatedBy = x.CreatedBy,
                        CreatedIn = x.CreatedIn

                    }).ToList(),

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetGeneralMedicationByClientTotalCount(Guid clientID)
        {
            return context.Medications.Include(y => y.Prescription).Where(x => x.IsDeleted == false && x.Prescription.ClientId == clientID).Count();
        }
        public async Task<IEnumerable<MedicationDto>> GetGeneralMedicationByPrescription(Guid prescriptionId)
        {
            try
            {
                var medicationInDb = await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false && d.Prescription.InteractionId == prescriptionId, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute, dr => dr.DrugRoute);
                var clinician = new UserAccount();
                var clinicianFacility = new Facility();
                if (medicationInDb != null)
                {
                    var createdBy = medicationInDb.Select(x => x.Prescription.CreatedBy).FirstOrDefault();
                    var createdIn = medicationInDb.Select(x => x.Prescription.CreatedIn).FirstOrDefault();
                    if (createdBy != null && createdBy != Guid.Empty)
                    {
                        clinician = context.UserAccounts.Where(x => x.Oid == createdBy.Value).FirstOrDefault();
                    }
                    if (createdIn != null)
                    {
                        clinicianFacility = context.Facilities.Where(x => x.Oid == createdIn.Value).FirstOrDefault();
                    }
                }

                return medicationInDb.GroupBy(c => new
                {
                    c.Prescription.EncounterId,
                    c.Prescription.InteractionId,

                }).Select(x => new MedicationDto()
                {
                    PrescriptionId = x.Key.InteractionId,
                    EncounterId = x.Key.EncounterId,

                    PrescriptionDate = x.Select(x => x.Prescription.PrescriptionDate).FirstOrDefault(),
                    DispensationDate = x.Select(x => x.Prescription.DispensationDate).FirstOrDefault(),
                    MedicationsList = x.Select(x => new MedicationListDto()
                    {
                        AdditionalDrugFormulation = x.AdditionalDrugFormulation,
                        AdditionalDrugTitle = x.AdditionalDrugTitle,
                        GenericDrug = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.GenericDrug.Description : "",
                        GenericDrugsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.Description : "",
                        GenericDrugsFormulationsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugFormulation.Description : "",
                        GenericDrugsDrugDosageUnitesDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugDosageUnit.Description : "",
                        SpecialDrugDescription = x.SpecialDrug != null ? x.SpecialDrug.Description : "",
                        SpecialDrugFormulations = x.SpecialDrug != null ? x.SpecialDrug.DrugFormulation.Description : "",
                        SpecialDrugDrugDosageUnitsDescription = x.SpecialDrug != null ? x.SpecialDrug.DrugDosageUnit.Description : "",
                        FrequencyIntervalTimeInterval = x.FrequencyInterval != null ? x.FrequencyInterval.Description : "",
                        PrescribedDosage = x.PrescribedDosage,
                        DrugRoute_Route = x.DrugRoute.Description,
                        DispenseDrugRouteRoute = x.DispensedDrugsRouteId == null ? "" : context.DrugRoutes.Where(z => z.Oid == x.DispensedDrugsRouteId.Value).FirstOrDefault().Description,
                        Duration = x.Duration,
                        TimeUnit = x.TimeUnit,
                        Frequency = x.Frequency,
                        IsPasserBy = x.IsPasserBy,
                        IsDispencedPasserBy = x.IsDispencedPasserBy,
                        DispensedDrugsFrequency = x.DispensedDrugsFrequency,
                        DispensedDrugsQuantity = x.DispensedDrugsQuantity,
                        DispansedDrugsDosage = x.DispensedDrugsDosage,
                        DispansedDrugTitle = x.DispensedDrugTitle,
                        DispensedDrugDuration = x.DispensedDrugDuration,
                        DispensedDrugsBrand = x.DispensedDrugsBrand,
                        DispensedDrugsEndDate = x.DispensedDrugsEndDate,
                        DispensedDrugsFormulation = x.DispensedDrugsFormulation,
                        DispensedDrugsFrequencyIntervalId = x.DispensedDrugsFrequencyIntervalId,
                        DispensedDrugsItemPerDose = x.DispensedDrugsItemPerDose,
                        DispensedDrugsRuteId = x.DispensedDrugsRouteId,
                        DispensedDrugsStartDate = x.DispensedDrugsStartDate,
                        DispensedDrugsTimeUnit = x.DispensedDrugsTimeUnit,
                        DosageUnit = x.DosageUnit,
                        EndDate = x.EndDate,
                        FrequencyIntervalId = x.FrequencyIntervalId,
                        StartDate = x.StartDate,
                        ItemPerDose = x.ItemPerDose,
                        PrescribedQuentity = x.PrescribedQuantity,
                        ReasonForReplacement = x.ReasonForReplacement,
                        Note = x.Note,
                        MedicationInteractionId = x.InteractionId,
                        PrescriptionDate = x.Prescription.PrescriptionDate,
                        DispensationDate = x.Prescription.DispensationDate,
                        CreatedBy = x.CreatedBy,
                        ClinicianName = clinician != null ? clinician.FirstName + " " + clinician.Surname : "",
                        FacilityName = clinicianFacility != null ? clinicianFacility.Description : "",

                    }).ToList(),

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<MedicationDto> GetLatestGeneralMedicationByClient(Guid clientId)
        {
            try
            {
                MedicationDto medicationDto = new MedicationDto();

                var precription = context.Prescriptions.OrderByDescending(x => x.PrescriptionDate).Where(x => x.ClientId == clientId).FirstOrDefault();

                if (precription != null)
                {
                    var medicationInDb = await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false && d.PrescriptionId == precription.InteractionId && d.Prescription.ClientId == clientId, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute, dr => dr.DrugRoute);
                    return medicationDto = new MedicationDto()
                    {
                        PrescriptionId = precription.InteractionId,
                        EncounterId = precription.EncounterId,

                        PrescriptionDate = precription.PrescriptionDate,
                        DispensationDate = precription.DispensationDate,
                        MedicationsList = medicationInDb.Select(x => new MedicationListDto()
                        {
                            AdditionalDrugFormulation = x.AdditionalDrugFormulation,
                            AdditionalDrugTitle = x.AdditionalDrugTitle,
                            GenericDrug = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.GenericDrug.Description : "",
                            GenericDrugsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.Description : "",
                            GenericDrugsFormulationsDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugFormulation.Description : "",
                            GenericDrugsDrugDosageUnitesDescription = x.GeneralDrugDefinition != null ? x.GeneralDrugDefinition.DrugDosageUnit.Description : "",
                            SpecialDrugDescription = x.SpecialDrug != null ? x.SpecialDrug.Description : "",
                            SpecialDrugFormulations = x.SpecialDrug != null ? x.SpecialDrug.DrugFormulation.Description : "",
                            SpecialDrugDrugDosageUnitsDescription = x.SpecialDrug != null ? x.SpecialDrug.DrugDosageUnit.Description : "",
                            FrequencyIntervalTimeInterval = x.FrequencyInterval != null ? x.FrequencyInterval.Description : "",
                            PrescribedDosage = x.PrescribedDosage,
                            DrugRoute_Route = x.DrugRoute.Description,
                            Duration = x.Duration,
                            TimeUnit = x.TimeUnit,
                            Frequency = x.Frequency,
                            IsPasserBy = x.IsPasserBy,
                            IsDispencedPasserBy = x.IsDispencedPasserBy,
                            DispensedDrugsFrequency = x.DispensedDrugsFrequency,
                            DispensedDrugsQuantity = x.DispensedDrugsQuantity,
                            DispansedDrugsDosage = x.DispensedDrugsDosage,
                            DispansedDrugTitle = x.DispensedDrugTitle,
                            DispensedDrugDuration = x.DispensedDrugDuration,
                            DispensedDrugsBrand = x.DispensedDrugsBrand,
                            DispensedDrugsEndDate = x.DispensedDrugsEndDate,
                            DispensedDrugsFormulation = x.DispensedDrugsFormulation,
                            DispensedDrugsFrequencyIntervalId = x.DispensedDrugsFrequencyIntervalId,
                            DispensedDrugsItemPerDose = x.DispensedDrugsItemPerDose,
                            DispensedDrugsRuteId = x.DispensedDrugsRouteId,
                            DispensedDrugsStartDate = x.DispensedDrugsStartDate,
                            DispensedDrugsTimeUnit = x.DispensedDrugsTimeUnit,
                            DosageUnit = x.DosageUnit,
                            EndDate = x.EndDate,
                            FrequencyIntervalId = x.FrequencyIntervalId,
                            StartDate = x.StartDate,
                            ItemPerDose = x.ItemPerDose,
                            PrescribedQuentity = x.PrescribedQuantity,
                            ReasonForReplacement = x.ReasonForReplacement,
                            Note = x.Note,
                            MedicationInteractionId = x.InteractionId,
                            PrescriptionDate = x.Prescription.PrescriptionDate,
                            DispensationDate = x.Prescription.DispensationDate,
                            CreatedBy = x.CreatedBy

                        }).ToList(),
                    };

                }
                return medicationDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Medication>> GetGeneralMedication()
        {
            try
            {
                return await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a GeneralMedication by PrescriptionId.
        /// </summary>
        /// <param name="prescriptionId">Primary key of the table GeneralMedication.</param>
        /// <returns>Returns a GeneralMedication if the PrescriptionId is matched.</returns>
        public async Task<IEnumerable<Medication>> GetGeneralMedicationByPrescriptionId(Guid prescriptionId)
        {
            try
            {
                return await LoadListWithChildAsync<Medication>(d => d.IsDeleted == false && d.PrescriptionId == prescriptionId, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Medication> GetGeneralMedicationByInteractionId(Guid interactionId)
        {
            try
            {
                return await LoadWithChildAsync<Medication>(d => d.IsDeleted == false && d.InteractionId == interactionId, dd => dd.GeneralDrugDefinition, df => df.GeneralDrugDefinition.DrugFormulation, du => du.GeneralDrugDefinition.DrugDosageUnit, gd => gd.GeneralDrugDefinition.GenericDrug, fi => fi.FrequencyInterval, sd => sd.SpecialDrug, sf => sf.SpecialDrug.DrugFormulation, sd => sd.SpecialDrug.DrugDosageUnit, pr => pr.Prescription, dr => dr.DrugRoute);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}