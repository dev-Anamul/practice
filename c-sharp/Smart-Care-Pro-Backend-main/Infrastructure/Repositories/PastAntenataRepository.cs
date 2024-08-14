using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
   public class PastAntenataRepository : Repository<PastAntenatalVisit>, IPastAntenataRepository
    {
      private readonly DataContext context;

      public PastAntenataRepository(DataContext context) : base(context)
      {
         this.context = context;
      }

 
   }
}