using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DropsRepository : Repository<Drop>, IDropsRepository
    {
        private readonly DataContext context;

        public DropsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Drop UpdateDrop(Drop drop)
        {
            try
            {
                var existingInDb = context.Drops
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(drop.PartographId) &&
                        i.DropsTime.Equals(drop.DropsTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Drop()
                    {
                        PartographId = drop.PartographId,
                        DropsTime = drop.DropsTime,
                        DropsDetails = drop.DropsDetails,
                        IsDeleted=false,
                        IsSynced=false,
                    };
                    context.Drops.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.DropsDetails != drop.DropsDetails)
                    {
                        existingInDb.DropsDetails = drop.DropsDetails;
                        existingInDb.IsSynced = false;
                        existingInDb.IsDeleted=false;
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