﻿using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IDescentOfHeadsRepository : IRepository<DescentOfHead>
   {
      DescentOfHead UpdateDescentOfHead(DescentOfHead descentOfHead);
   }
}