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
   public class TemperaturesCreateDto
   {
      public Guid PartographId { get; set; }
      public List<long[]> Data { get; set; }
   }
}