using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class ReportParamsDto
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int FacilityId { get; set; }
    }
}
