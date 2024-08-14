using Domain.Entities;

namespace Domain.Dto
{
    public class FamilySocialHistoriesDto : EncounterBaseModel
    {
        public string FamilyMedicalHistory { get; set; }

        public string AlcoholSmokingHistory { get; set; }

        public string SiblingHistory { get; set; }

        public Guid ClientID { get; set; }

        public int[] RiskList { get; set; }
    }
}