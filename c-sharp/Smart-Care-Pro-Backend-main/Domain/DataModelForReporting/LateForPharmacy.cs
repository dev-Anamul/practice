using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataModelForReporting
{
    public class LateForPharmacy
    {
        public string NUPN { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string ArtSmNumber { get; set; }
        public string LastVisitDate { get; set; }
        public string NextVisitDate { get; set; }
        public int DaysLate { get; set; }
        public int LastCD4Count { get; set; }
        public int YearsOnART { get; set; }
        public string OnARVS { get; set; }
        public string DueForVLp { get; set; }
        public string NextViralLoadDate { get; set; }
    }
}
