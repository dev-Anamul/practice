
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
   public class MouldingCreateDto
   {
      public Guid PartographId { get; set; }
      public List<string[]> Data { get; set; }
   }
}