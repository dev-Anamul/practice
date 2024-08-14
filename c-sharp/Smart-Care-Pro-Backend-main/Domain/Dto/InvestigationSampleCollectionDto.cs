using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Stephan
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class InvestigationSampleCollectionDto
    {
        public Guid InteractionId { get; set; }

        public DateTime SampleCollectionDate { get; set; }

        public TimeSpan SampleCollectionTime { get; set; }
    }
}
