using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DataModelForReporting
{
    public class ARTPsychoSocialCounselling
    {
        //public int FacilityId { get; set; }
        public int MaleCount { get; set; }

        public int FemaleCount { get; set; }

        public int Total { get; set; }

        public int MalePositiveCount { get; set; }

        public int FemalePositiveCount { get; set; }

        public int MaleNegativeCount { get; set; }

        public int FemaleNegativeCount { get; set; }

        public int MalePEPCount { get; set; }

        public int FemalePEPCount { get; set; }

        public int MaleLinkageToCare { get; set; }

        public int FemaleLinkageToCare { get; set; }

        public int TotalLinkageToCare { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
