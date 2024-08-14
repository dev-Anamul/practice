using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class TBConstitutionalSymptomsDto
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? EncounterDate { get; set; }
        
        public DateTime? DateModified { get; set; }

        public Guid IdentifiedConstitutionalSymptomInteractionID { get; set; }

        public int TBSymptomID { get; set; }
    
        public  TBSymptom TBSymptom { get; set; }

        public Guid ClientID { get; set; }

        public Guid OPDVisitID { get; set; }

        public Guid IdentifiedTBSymptomInteractionID { get; set; }
    
        public int ConstitutionalSymptomTypeID { get; set; }

        public  ConstitutionalSymptomType ConstitutionalSymptomType { get; set; }

        public int? CreatedIn { get; set; }

        public Guid? CreatedBy { get; set; }

        public EncounterType EncounterType { get; set; }
         
        public string? FacilityName { get; set; }
         
        public string? ClinicianName { get; set; }
    }
}
