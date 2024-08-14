using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class MouldingsRepository : Repository<Moulding>, IMouldingsRepository
    {
        private readonly DataContext context;

        public MouldingsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Moulding UpdateMoulding(Moulding moulding)
        {
            try
            {
                var existingInDb = context.Mouldings
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(moulding.PartographId) &&
                        i.MouldingTime.Equals(moulding.MouldingTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Moulding()
                    {
                        PartographId = moulding.PartographId,
                        MouldingTime = moulding.MouldingTime,
                        Description = moulding.Description,
                        IsSynced = false,
                        IsDeleted = false,
                    };
                    context.Mouldings.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.Description != moulding.Description)
                    {
                        existingInDb.Description = moulding.Description;
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