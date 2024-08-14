using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Domain.Dto
{
    public class DFZClientDepenentDto
    {
        public RelationType RelationType { get; set; }
        public Client RelativeClient { get; set; }
    }

    public class DFzClientRelationDto
    {
        //public DFZClientDepenentDto 
    }
}
