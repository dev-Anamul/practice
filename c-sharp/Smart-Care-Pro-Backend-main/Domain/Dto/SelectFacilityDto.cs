using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class SelectFacilityDto
    {
        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
        public List<Facility> Facilities { get; set; }
    }
}
