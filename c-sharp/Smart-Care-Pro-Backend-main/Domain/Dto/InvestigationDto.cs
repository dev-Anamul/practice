using Domain.Entities;
using Domain.Validations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;
using System.Xml.Linq;
using Utilities.Constants;

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
    public class InvestigationDto
    {
        public List<Investigation> investigation { get; set; }

        public List<InvestigationWithOutComposite> investigationWithOutComposite { get; set; }

        public List<InvestigationWithComposite>? investigationWithComposite { get; set; }

        public Guid EncounterID { get; set; }

        public Guid ClientID { get; set; }

        public Guid ClinicianID { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? EncounterDate { get; set; }

        public int? CreatedIn { get; set; }

        public Guid? CreatedBy { get; set; }
        public string? FacilityName { get; set; }
        public string? ClinicianName { get; set; }

    }
    public class InvestigationWithOutComposite
    {
        public Guid InteractionID { get; set; }

        public string TestResult { get; set; }

        public DateTime OrderDate { get; set; }

        public Guid EncounterID { get; set; }

        public int Quantity { get; set; }

        public int? SampleQuantity { get; set; }
   
        public Piority Piority { get; set; }

        public string OrderNumber { get; set; }

        public string ImagingTestDetails { get; set; }

        public string AdditionalComment { get; set; }

        public bool IsResultReceived { get; set; }
        
        public Guid ClinicianID { get; set; }
 
        public virtual UserAccount UserAccount { get; set; }

        public int? TestTypeId { get; set; }

        public int TestID { get; set; }

        public virtual Test Test { get; set; }
     
        public Guid ClientID { get; set; }

        public virtual Client Client { get; set; }

        public virtual IEnumerable<Result> Results { get; set; }

        public int? CreatedIn { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? EncounterDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public int? ModifiedIn { get; set; }

        public DateTime? DateModified { get; set; }

        public Guid? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSynced { get; set; }
        public decimal? MinumumRange { get; set; }

        public string UnitTest { get; set; }

        public decimal? MaxumumRange { get; set; }
        public string? FacilityName { get; set; }
        public string? ClinicianName { get; set; }
    }
    public class InvestigationWithComposite
    {
        public string CompositeName { get; set; }

        public Guid InteractionID { get; set; }

        public Guid EncounterID { get; set; }

        public string TestResult { get; set; }

        public DateTime OrderDate { get; set; }

        public int Quantity { get; set; }

        public int? SampleQuantity { get; set; }

        public Piority Piority { get; set; }

        public string ImagingTestDetails { get; set; }

        public string OrderNumber { get; set; }

        public string AdditionalComment { get; set; }

        public bool IsResultReceived { get; set; }

        public Guid ClinicianID { get; set; }

        public virtual UserAccount UserAccount { get; set; }

        public int? TestTypeId { get; set; }

        public int TestID { get; set; }

        public virtual Test Test { get; set; }

        public Guid ClientID { get; set; }

        public virtual Client Client { get; set; }

        public virtual IEnumerable<Result> Results { get; set; }

        public decimal? MinumumRange { get; set; }

        public string UnitTest { get; set; }

        public decimal? MaxumumRange { get; set; }

        public int? CreatedIn { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? EncounterDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public int? ModifiedIn { get; set; }

        public DateTime? DateModified { get; set; }

        public Guid? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSynced { get; set; }
        public string? FacilityName { get; set; }
        public string? ClinicianName { get; set; }
    }
}