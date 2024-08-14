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
    public class HTSDetailDTO
    {
        public List<HTS> hTS { get; set; }
        public Client Client { get; set; }
        public Vital vital { get; set; }
    }
}
