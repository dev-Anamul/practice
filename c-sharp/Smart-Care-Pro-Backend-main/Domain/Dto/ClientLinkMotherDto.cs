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
    public class ClientLinkMotherDto
    {
        public Guid? ChildOID { get; set; }
        public Guid? MotherOID { get; set; }
    }
}
