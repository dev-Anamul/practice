using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class CompleteNursingPlanDto : EncounterBaseModel
    {
        public List<NursingPlan> nursingPlans { get; set; }

        public List<Vital> vitals { get; set; }

        public List<GlasgowComaScale> glasgowComaScales { get; set; }

        public List<TurningChart> turningCharts { get; set; }

        public List<Fluid> fluids { get; set; }

        public List<FluidIntake> fluidIntakes { get; set; }

        public List<FluidOutput> fluidOutputs { get; set; }
    }
}