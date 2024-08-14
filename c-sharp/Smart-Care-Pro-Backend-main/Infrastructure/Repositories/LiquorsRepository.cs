using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class LiquorsRepository : Repository<Liquor>, ILiquorsRepository
    {
        private readonly DataContext context;

        public LiquorsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Liquor UpdateLiquor(Liquor liquor)
        {
            try
            {
                var existingInDb = context.Liquors
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(liquor.PartographId) &&
                        i.LiquorTime.Equals(liquor.LiquorTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Liquor()
                    {
                        PartographId = liquor.PartographId,
                        LiquorTime = liquor.LiquorTime,
                        Description = liquor.Description,
                        IsDeleted = false,
                        IsSynced = false,

                    };
                    context.Liquors.Add(existingInDb);
                }
                else
                {
                    existingInDb.Description = liquor.Description;
                    existingInDb.IsDeleted = false;
                    existingInDb.IsSynced = false;
                    context.Entry(existingInDb).State = EntityState.Modified;
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