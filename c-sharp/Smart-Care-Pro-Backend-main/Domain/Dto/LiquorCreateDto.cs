
/*
 * Created by   : Bella
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class LiquorCreateDto
   {
      public Guid PartographId { get; set; }
      public List<string[]> Data { get; set; }
   }
}