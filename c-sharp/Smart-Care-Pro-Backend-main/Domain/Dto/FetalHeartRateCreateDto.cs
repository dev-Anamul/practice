﻿/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Domain.Dto
{
   public class FetalHeartRateCreateDto
   {
      public Guid PartographID { get; set; }
      public List<long[]> Data { get; set; }
   }
}