namespace Domain.DataModelForReporting
{
    public class OPDAttendance
    {
        //public int FacilityId { get; set; }
        public int AdultMale { get; set; }
        public int AdultFemale { get; set; }
        public int ChildrenMale { get; set; }
        public int ChildrenFemale { get; set; }
        public int TotalAdults { get; set; }
        public int TotalChildren { get; set; }
        public int TotalMale { get; set; }
        public int TotalFemale { get; set; }
        public int TotalAttendence { get; set; }
        public int AdultReferral { get; set; }
        public int ChildrenReferral { get; set; }
        public int TotalReferral { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}