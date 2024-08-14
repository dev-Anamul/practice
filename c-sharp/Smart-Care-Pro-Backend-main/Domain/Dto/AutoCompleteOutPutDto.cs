/*
 * Created by   : Lion
 * Date created : 14.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class AutoCompleteOutPutDto
    {
        public string value { get; set; }
        public string specialValue { get; set; }
        public string label { get; set; }
        public string desc { get; set; }
        public string VenStatus { get; set; }
        public string PragnancyRisk { get; set; }
        public string BreastFeedingRisk { get; set; }
        public string MaxDose { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Formulation { get; set; }
        public string DosageUnit { get; set; }
        public string Strength { get; set; }
    }
}