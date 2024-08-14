namespace Domain.DataModelForReporting
{
    public class MCH
    {
        //public int FacilityId { get; set; }
        public int FirstBookingCount { get; set; }
        public int RevisitCount { get; set; }
        public int TotalVisit { get; set; }
        public int FirstVisitReferralCount { get; set; }
        public int RevisitReferralCount { get; set; }
        public int HighRiskCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
