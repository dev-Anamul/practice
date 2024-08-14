using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Domain.Dto
{
   public class TestCreateDto : BaseModel
   {
      public int Oid { get; set; }
      public int SubtypeId { get; set; }
      public string Title { get; set; }
      public string LONIC { get; set; }
      public string Description { get; set; }
      public ResultType ResultType { get; set; }
      public int TestId { get; set; }

      public string MeasuringUnitDescription { get; set; }
      public decimal MinimumRange { get; set; }
      public decimal MaximumRange { get; set; }

      public string ResultOptionDescriptions { get; set; }
   }
}
