using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class ComplicationTypeDto : EncounterBaseModel
    {
        public int[] ComplicationTypeList { get; set; }

        public Guid VMMCServiceID { get; set; }
    }
}