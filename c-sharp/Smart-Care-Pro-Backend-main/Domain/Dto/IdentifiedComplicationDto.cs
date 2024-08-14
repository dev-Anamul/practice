using Domain.Entities;
using static Utilities.Constants.Enums;

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
    public class IdentifiedComplicationDto : EncounterBaseModel
    {
        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public Guid IdentifiedComplicationInteractionID { get; set; }

        public int ComplicationTypeID { get; set; }

        public ComplicationType ComplicationType { get; set; }

        public Guid ComplicationID { get; set; }

        public Complication Complication { get; set; }

        public Guid VMMCServiceID { get; set; }

        public EncounterType EncounterType { get; set; }
    }
}