using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Utilities.Constants.Enums;

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
    public class InvestigationDashBoardDto
    {
        public List<InvestigationItemDto> Investigations { get; set; }
        public int TotalItems { get; set; }
        public int ResultRecievedTotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class InvestigationItemDto
    {
        public Guid InteractionId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }

        public Guid EncounterId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Sample Collection Date")]
        public DateTime? SampleCollectionDate { get; set; }
        public int Quantity { get; set; }
        public int? SampleQuantity { get; set; }
        public Piority Piority { get; set; }
        public string ImagingTestDetails { get; set; }
        public string AdditionalComment { get; set; }
        public bool IsResultReceived { get; set; }
        public Guid ClinicianId { get; set; }
        public int TestId { get; set; }
        public ResultType ResultType { get; set; }
        public int SubTypeId {  get; set; }
        public Guid ClientId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string TestName { get; set; }
        public string Lonic { get; set; }
        public List<ResultDto> Results { get; set; }
    }
    public class ResultDto
    {
        public Guid InteractionId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Result Date")]
        public DateTime ResultDate { get; set; }
        public string ResultDescriptive { get; set; }
        public decimal? ResultNumeric { get; set; }
        public string CommentOnResult { get; set; }
        public IsResultNormal IsResultNormal { get; set; }
        public Guid InvestigationId { get; set; }
    }

}
