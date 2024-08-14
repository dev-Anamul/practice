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
    public class NewBornDetailHistoryDto : BaseModel
    {
        public List<ApgarScore> ApgarScores { get; set; }

        public List<NeonatalAbnormality> NeonatalAbnormalities { get; set; }

        public List<NeonatalInjury> NeonatalInjuries { get; set; }

        public List<NeonatalDeath> NeonatalDeaths { get; set; }
    }
}