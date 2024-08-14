using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bella
 * Date created : 01.01.2023
 * Modified by  : Stephan
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IAllergyRepository interface.
    /// </summary>
    public class AssessmentRepository : Repository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Assessment by key.
        /// </summary>
        /// <param name="key">Primary key of the table Assessments.</param>
        /// <returns>Returns a Assessment if the key is matched.</returns>
        public async Task<Assessment> GetAssessmentByKey(Guid key)
        {
            try
            {
                var assessment = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (assessment is not null)
                {
                    assessment.EncounterDate = await context.Encounters.Where(x => x.Oid == assessment.EncounterId).AsNoTracking().Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();
                    assessment.ClinicianName = await context.UserAccounts.Where(x => x.Oid == assessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    assessment.FacilityName = await context.Facilities.Where(x => x.Oid == assessment.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return assessment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Assessments.
        /// </summary>
        /// <returns>Returns a list of all Assessments.</returns>
        public async Task<IEnumerable<Assessment>> GetAssessments()
        {
            try
            {
                return await context.Assessments.AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        assessment => assessment.EncounterId,
                        encounter => encounter.Oid,
                        (assessment, encounter) => new Assessment
                        {
                            InteractionId = assessment.InteractionId,
                            GeneralCondition = assessment.GeneralCondition,
                            NutritionalStatus = assessment.NutritionalStatus,
                            JVP = assessment.JVP,
                            HydrationStatus = assessment.HydrationStatus,
                            Glucose = assessment.Glucose,
                            Scoring = assessment.Scoring,
                            VaricoseVein = assessment.VaricoseVein,
                            Albumin = assessment.Albumin,
                            UrineOutput = assessment.UrineOutput,
                            Feeding = assessment.Feeding,
                            PassedMeconium = assessment.PassedMeconium,
                            UrinePassed = assessment.UrinePassed,
                            ChildCardIssued = assessment.ChildCardIssued,
                            VitaminAGiven = assessment.VitaminAGiven,
                            FatherLiving = assessment.FatherLiving,
                            MotherLiving = assessment.MotherLiving,
                            Pallor = assessment.Pallor,
                            Edema = assessment.Edema,
                            Clubbing = assessment.Clubbing,
                            Jaundice = assessment.Jaundice,
                            Cyanosis = assessment.Cyanosis,
                            Hb = assessment.Hb,
                            PuerperalCondition = assessment.PuerperalCondition,
                            BreastFeeding = assessment.BreastFeeding,
                            InvolutionOfUterus = assessment.InvolutionOfUterus,
                            Lochia = assessment.Lochia,
                            PerineumCondition = assessment.PerineumCondition,
                            EpisiotomyCondition = assessment.EpisiotomyCondition,
                            AdditionalNotes = assessment.AdditionalNotes,
                            Fontanelles = assessment.Fontanelles,
                            Skull = assessment.Skull,
                            Eyes = assessment.Eyes,
                            Mouth = assessment.Mouth,
                            Chest = assessment.Chest,
                            Back = assessment.Back,
                            Limbs = assessment.Limbs,
                            Genitals = assessment.Genitals,
                            SymmetricalMoroReaction = assessment.SymmetricalMoroReaction,
                            MoroReaction = assessment.MoroReaction,
                            IsGoodGraspReflex = assessment.IsGoodGraspReflex,
                            IsMeconiumPassed = assessment.IsMeconiumPassed,
                            IsGoodHeadControl = assessment.IsGoodHeadControl,
                            OrtolaniSign = assessment.OrtolaniSign,
                            RootingReflex = assessment.RootingReflex,
                            SuckingReflex = assessment.SuckingReflex,
                            PalmarReflex = assessment.PalmarReflex,
                            PlantarGrasp = assessment.PlantarGrasp,
                            SteppingReflex = assessment.SteppingReflex,
                            GalantReflex = assessment.GalantReflex,
                            ClientId = assessment.ClientId,
                            EncounterId = assessment.EncounterId,
                            EncounterType = assessment.EncounterType,
                            CreatedIn = encounter.CreatedIn,
                            DateCreated = encounter.DateCreated,
                            CreatedBy = encounter.CreatedBy,
                            ModifiedIn = encounter.ModifiedIn,
                            DateModified = encounter.DateModified,
                            ModifiedBy = encounter.ModifiedBy,
                            IsDeleted = encounter.IsDeleted,
                            IsSynced = encounter.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == assessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == assessment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })
                    .Where(assessment => assessment.IsDeleted == false)
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
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPDVisit if the key is matched.</returns>
        public async Task<IEnumerable<Assessment>> GetAssessmentByEncounter(Guid encounterId)
        {
            try
            {
                return await context.Assessments.AsNoTracking().Where(assessment => assessment.IsDeleted == false && assessment.EncounterId == encounterId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        assessment => assessment.EncounterId,
                        encounter => encounter.Oid,
                        (assessment, encounter) => new Assessment
                        {
                            InteractionId = assessment.InteractionId,
                            GeneralCondition = assessment.GeneralCondition,
                            NutritionalStatus = assessment.NutritionalStatus,
                            JVP = assessment.JVP,
                            HydrationStatus = assessment.HydrationStatus,
                            Glucose = assessment.Glucose,
                            Scoring = assessment.Scoring,
                            VaricoseVein = assessment.VaricoseVein,
                            Albumin = assessment.Albumin,
                            UrineOutput = assessment.UrineOutput,
                            Feeding = assessment.Feeding,
                            PassedMeconium = assessment.PassedMeconium,
                            UrinePassed = assessment.UrinePassed,
                            ChildCardIssued = assessment.ChildCardIssued,
                            VitaminAGiven = assessment.VitaminAGiven,
                            FatherLiving = assessment.FatherLiving,
                            MotherLiving = assessment.MotherLiving,
                            Pallor = assessment.Pallor,
                            Edema = assessment.Edema,
                            Clubbing = assessment.Clubbing,
                            Jaundice = assessment.Jaundice,
                            Cyanosis = assessment.Cyanosis,
                            Hb = assessment.Hb,
                            PuerperalCondition = assessment.PuerperalCondition,
                            BreastFeeding = assessment.BreastFeeding,
                            InvolutionOfUterus = assessment.InvolutionOfUterus,
                            Lochia = assessment.Lochia,
                            PerineumCondition = assessment.PerineumCondition,
                            EpisiotomyCondition = assessment.EpisiotomyCondition,
                            AdditionalNotes = assessment.AdditionalNotes,
                            Fontanelles = assessment.Fontanelles,
                            Skull = assessment.Skull,
                            Eyes = assessment.Eyes,
                            Mouth = assessment.Mouth,
                            Chest = assessment.Chest,
                            Back = assessment.Back,
                            Limbs = assessment.Limbs,
                            Genitals = assessment.Genitals,
                            SymmetricalMoroReaction = assessment.SymmetricalMoroReaction,
                            MoroReaction = assessment.MoroReaction,
                            IsGoodGraspReflex = assessment.IsGoodGraspReflex,
                            IsMeconiumPassed = assessment.IsMeconiumPassed,
                            IsGoodHeadControl = assessment.IsGoodHeadControl,
                            OrtolaniSign = assessment.OrtolaniSign,
                            RootingReflex = assessment.RootingReflex,
                            SuckingReflex = assessment.SuckingReflex,
                            PalmarReflex = assessment.PalmarReflex,
                            PlantarGrasp = assessment.PlantarGrasp,
                            SteppingReflex = assessment.SteppingReflex,
                            GalantReflex = assessment.GalantReflex,
                            ClientId = assessment.ClientId,
                            EncounterId = assessment.EncounterId,
                            EncounterType = assessment.EncounterType,
                            CreatedIn = encounter.CreatedIn,
                            DateCreated = encounter.DateCreated,
                            CreatedBy = encounter.CreatedBy,
                            ModifiedIn = encounter.ModifiedIn,
                            DateModified = encounter.DateModified,
                            ModifiedBy = encounter.ModifiedBy,
                            IsDeleted = encounter.IsDeleted,
                            IsSynced = encounter.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == assessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == assessment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
        /// The method is used to get a birth history by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a birth history if the ClientID is matched.</returns>
        public async Task<IEnumerable<Assessment>> GetAssessmentsByClient(Guid clientId)
        {
            try
            {
                return await context.Assessments.AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        assessment => assessment.EncounterId,
                        encounter => encounter.Oid,
                        (assessment, encounter) => new Assessment
                        {
                            InteractionId = assessment.InteractionId,
                            GeneralCondition = assessment.GeneralCondition,
                            NutritionalStatus = assessment.NutritionalStatus,
                            JVP = assessment.JVP,
                            HydrationStatus = assessment.HydrationStatus,
                            Glucose = assessment.Glucose,
                            Scoring = assessment.Scoring,
                            VaricoseVein = assessment.VaricoseVein,
                            Albumin = assessment.Albumin,
                            UrineOutput = assessment.UrineOutput,
                            Feeding = assessment.Feeding,
                            PassedMeconium = assessment.PassedMeconium,
                            UrinePassed = assessment.UrinePassed,
                            ChildCardIssued = assessment.ChildCardIssued,
                            VitaminAGiven = assessment.VitaminAGiven,
                            FatherLiving = assessment.FatherLiving,
                            MotherLiving = assessment.MotherLiving,
                            Pallor = assessment.Pallor,
                            Edema = assessment.Edema,
                            Clubbing = assessment.Clubbing,
                            Jaundice = assessment.Jaundice,
                            Cyanosis = assessment.Cyanosis,
                            Hb = assessment.Hb,
                            PuerperalCondition = assessment.PuerperalCondition,
                            BreastFeeding = assessment.BreastFeeding,
                            InvolutionOfUterus = assessment.InvolutionOfUterus,
                            Lochia = assessment.Lochia,
                            PerineumCondition = assessment.PerineumCondition,
                            EpisiotomyCondition = assessment.EpisiotomyCondition,
                            AdditionalNotes = assessment.AdditionalNotes,
                            Fontanelles = assessment.Fontanelles,
                            Skull = assessment.Skull,
                            Eyes = assessment.Eyes,
                            Mouth = assessment.Mouth,
                            Chest = assessment.Chest,
                            Back = assessment.Back,
                            Limbs = assessment.Limbs,
                            Genitals = assessment.Genitals,
                            SymmetricalMoroReaction = assessment.SymmetricalMoroReaction,
                            MoroReaction = assessment.MoroReaction,
                            IsGoodGraspReflex = assessment.IsGoodGraspReflex,
                            IsMeconiumPassed = assessment.IsMeconiumPassed,
                            IsGoodHeadControl = assessment.IsGoodHeadControl,
                            OrtolaniSign = assessment.OrtolaniSign,
                            RootingReflex = assessment.RootingReflex,
                            SuckingReflex = assessment.SuckingReflex,
                            PalmarReflex = assessment.PalmarReflex,
                            PlantarGrasp = assessment.PlantarGrasp,
                            SteppingReflex = assessment.SteppingReflex,
                            GalantReflex = assessment.GalantReflex,
                            ClientId = assessment.ClientId,
                            EncounterId = assessment.EncounterId,
                            EncounterType = assessment.EncounterType,
                            CreatedIn = assessment.CreatedIn,
                            DateCreated = assessment.DateCreated,
                            CreatedBy = assessment.CreatedBy,
                            ModifiedIn = assessment.ModifiedIn,
                            DateModified = assessment.DateModified,
                            ModifiedBy = assessment.ModifiedBy,
                            IsDeleted = assessment.IsDeleted,
                            IsSynced = assessment.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == assessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == assessment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                        })
                    .Where(assessment => assessment.IsDeleted == false && assessment.ClientId == clientId)
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Assessment>> GetAssessmentsByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.Assessments.AsNoTracking()
                               .Join(
                                   context.Encounters.AsNoTracking(),
                                   assessment => assessment.EncounterId,
                                   encounter => encounter.Oid,
                                   (assessment, encounter) => new Assessment
                                   {
                                       InteractionId = assessment.InteractionId,
                                       GeneralCondition = assessment.GeneralCondition,
                                       NutritionalStatus = assessment.NutritionalStatus,
                                       JVP = assessment.JVP,
                                       HydrationStatus = assessment.HydrationStatus,
                                       Glucose = assessment.Glucose,
                                       Scoring = assessment.Scoring,
                                       VaricoseVein = assessment.VaricoseVein,
                                       Albumin = assessment.Albumin,
                                       UrineOutput = assessment.UrineOutput,
                                       Feeding = assessment.Feeding,
                                       PassedMeconium = assessment.PassedMeconium,
                                       UrinePassed = assessment.UrinePassed,
                                       ChildCardIssued = assessment.ChildCardIssued,
                                       VitaminAGiven = assessment.VitaminAGiven,
                                       FatherLiving = assessment.FatherLiving,
                                       MotherLiving = assessment.MotherLiving,
                                       Pallor = assessment.Pallor,
                                       Edema = assessment.Edema,
                                       Clubbing = assessment.Clubbing,
                                       Jaundice = assessment.Jaundice,
                                       Cyanosis = assessment.Cyanosis,
                                       Hb = assessment.Hb,
                                       PuerperalCondition = assessment.PuerperalCondition,
                                       BreastFeeding = assessment.BreastFeeding,
                                       InvolutionOfUterus = assessment.InvolutionOfUterus,
                                       Lochia = assessment.Lochia,
                                       PerineumCondition = assessment.PerineumCondition,
                                       EpisiotomyCondition = assessment.EpisiotomyCondition,
                                       AdditionalNotes = assessment.AdditionalNotes,
                                       Fontanelles = assessment.Fontanelles,
                                       Skull = assessment.Skull,
                                       Eyes = assessment.Eyes,
                                       Mouth = assessment.Mouth,
                                       Chest = assessment.Chest,
                                       Back = assessment.Back,
                                       Limbs = assessment.Limbs,
                                       Genitals = assessment.Genitals,
                                       SymmetricalMoroReaction = assessment.SymmetricalMoroReaction,
                                       MoroReaction = assessment.MoroReaction,
                                       IsGoodGraspReflex = assessment.IsGoodGraspReflex,
                                       IsMeconiumPassed = assessment.IsMeconiumPassed,
                                       IsGoodHeadControl = assessment.IsGoodHeadControl,
                                       OrtolaniSign = assessment.OrtolaniSign,
                                       RootingReflex = assessment.RootingReflex,
                                       SuckingReflex = assessment.SuckingReflex,
                                       PalmarReflex = assessment.PalmarReflex,
                                       PlantarGrasp = assessment.PlantarGrasp,
                                       SteppingReflex = assessment.SteppingReflex,
                                       GalantReflex = assessment.GalantReflex,
                                       ClientId = assessment.ClientId,
                                       EncounterId = assessment.EncounterId,
                                       EncounterType = assessment.EncounterType,
                                       CreatedIn = assessment.CreatedIn,
                                       DateCreated = assessment.DateCreated,
                                       CreatedBy = assessment.CreatedBy,
                                       ModifiedIn = assessment.ModifiedIn,
                                       DateModified = assessment.DateModified,
                                       ModifiedBy = assessment.ModifiedBy,
                                       IsDeleted = assessment.IsDeleted,
                                       IsSynced = assessment.IsSynced,
                                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                       ClinicianName = context.UserAccounts.Where(x => x.Oid == assessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                       FacilityName = context.Facilities.Where(x => x.Oid == assessment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                                   })
                               .Where(assessment => assessment.IsDeleted == false && assessment.DateCreated >= Last24Hours && assessment.ClientId == clientId)
                               .OrderByDescending(x => x.EncounterDate)
                               .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Assessment>> GetAssessmentsByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var assesments = context.Assessments.AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        assessment => assessment.EncounterId,
                        encounter => encounter.Oid,
                        (assessment, encounter) => new Assessment
                        {
                            InteractionId = assessment.InteractionId,
                            GeneralCondition = assessment.GeneralCondition,
                            NutritionalStatus = assessment.NutritionalStatus,
                            JVP = assessment.JVP,
                            HydrationStatus = assessment.HydrationStatus,
                            Glucose = assessment.Glucose,
                            Scoring = assessment.Scoring,
                            VaricoseVein = assessment.VaricoseVein,
                            Albumin = assessment.Albumin,
                            UrineOutput = assessment.UrineOutput,
                            Feeding = assessment.Feeding,
                            PassedMeconium = assessment.PassedMeconium,
                            UrinePassed = assessment.UrinePassed,
                            ChildCardIssued = assessment.ChildCardIssued,
                            VitaminAGiven = assessment.VitaminAGiven,
                            FatherLiving = assessment.FatherLiving,
                            MotherLiving = assessment.MotherLiving,
                            Pallor = assessment.Pallor,
                            Edema = assessment.Edema,
                            Clubbing = assessment.Clubbing,
                            Jaundice = assessment.Jaundice,
                            Cyanosis = assessment.Cyanosis,
                            Hb = assessment.Hb,
                            PuerperalCondition = assessment.PuerperalCondition,
                            BreastFeeding = assessment.BreastFeeding,
                            InvolutionOfUterus = assessment.InvolutionOfUterus,
                            Lochia = assessment.Lochia,
                            PerineumCondition = assessment.PerineumCondition,
                            EpisiotomyCondition = assessment.EpisiotomyCondition,
                            AdditionalNotes = assessment.AdditionalNotes,
                            Fontanelles = assessment.Fontanelles,
                            Skull = assessment.Skull,
                            Eyes = assessment.Eyes,
                            Mouth = assessment.Mouth,
                            Chest = assessment.Chest,
                            Back = assessment.Back,
                            Limbs = assessment.Limbs,
                            Genitals = assessment.Genitals,
                            SymmetricalMoroReaction = assessment.SymmetricalMoroReaction,
                            MoroReaction = assessment.MoroReaction,
                            IsGoodGraspReflex = assessment.IsGoodGraspReflex,
                            IsMeconiumPassed = assessment.IsMeconiumPassed,
                            IsGoodHeadControl = assessment.IsGoodHeadControl,
                            OrtolaniSign = assessment.OrtolaniSign,
                            RootingReflex = assessment.RootingReflex,
                            SuckingReflex = assessment.SuckingReflex,
                            PalmarReflex = assessment.PalmarReflex,
                            PlantarGrasp = assessment.PlantarGrasp,
                            SteppingReflex = assessment.SteppingReflex,
                            GalantReflex = assessment.GalantReflex,
                            ClientId = assessment.ClientId,
                            EncounterId = assessment.EncounterId,
                            EncounterType = assessment.EncounterType,
                            CreatedIn = assessment.CreatedIn,
                            DateCreated = assessment.DateCreated,
                            CreatedBy = assessment.CreatedBy,
                            ModifiedIn = assessment.ModifiedIn,
                            DateModified = assessment.DateModified,
                            ModifiedBy = assessment.ModifiedBy,
                            IsDeleted = assessment.IsDeleted,
                            IsSynced = assessment.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == assessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == assessment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }).AsQueryable();

                if (encounterType == null)
                    return await assesments.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await assesments.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetAssessmentsByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Assessments.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Assessments.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}