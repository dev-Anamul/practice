using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Domain.Dto
{
    public class SCRSLoginSuccessDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public Sex Sex { get; set; }
        public string Designation { get; set; }
        public string Username { get; set; }
       public long FacilityKey { get; set; }
    }
}
