using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DescentOfHeadsRepository : Repository<DescentOfHead>, IDescentOfHeadsRepository
    {
        private readonly DataContext context;

        public DescentOfHeadsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public DescentOfHead UpdateDescentOfHead(DescentOfHead descentOfHead)
        {
            try
            {
                var existingInDb = context.DescentOfHeads
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(descentOfHead.PartographId) &&
                        i.DescentOfHeadTime.Equals(descentOfHead.DescentOfHeadTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new DescentOfHead()
                    {
                        PartographId = descentOfHead.PartographId,
                        DescentOfHeadTime = descentOfHead.DescentOfHeadTime,
                        DescentOfHeadDetails = descentOfHead.DescentOfHeadDetails,
                        IsSynced = false,
                        IsDeleted = false,
                    };
                    context.DescentOfHeads.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.DescentOfHeadDetails != descentOfHead.DescentOfHeadDetails)
                    {

                        existingInDb.DescentOfHeadDetails = descentOfHead.DescentOfHeadDetails;
                        existingInDb.IsDeleted = false;
                        existingInDb.IsSynced = false;
                        context.Entry(existingInDb).State = EntityState.Modified;
                    }
                }

                return existingInDb;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}