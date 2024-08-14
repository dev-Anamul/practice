using static Utilities.Constants.Enums;

namespace Domain.Dto
{
    public class FacilityAccessDto
    {
        public Guid Oid { get; set; }   
        public Guid UserAccountId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
        public Sex Sex { get; set; }
        public string Designation { get; set; }
        public string NRC { get; set; }
        public bool NoNRC { get; set; }
        public string ContactAddress { get; set; }
        public string CountryCode { get; set; }
        public string Cellphone { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public bool ISDFZFacility { get; set; }

    }
}