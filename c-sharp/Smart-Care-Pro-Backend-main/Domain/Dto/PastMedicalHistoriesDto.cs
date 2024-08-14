using Domain.Entities;

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
    public class PastMedicalHistoriesDto : EncounterBaseModel
    {
        public string DrugHistory { get; set; }

        public string AdmissionHistory { get; set; }

        public string SurgicalHistory { get; set; }

        public Guid ClientID { get; set; }
    }
}