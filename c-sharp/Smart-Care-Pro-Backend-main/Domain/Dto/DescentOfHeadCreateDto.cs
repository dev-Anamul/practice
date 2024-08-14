/*
 * Created by   : Lion
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class DescentOfHeadCreateDto
   {
      public Guid PartographId { get; set; }
      public List<long[]> Data { get; set; }
   }
}