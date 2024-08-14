using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class OxytocinRepository : Repository<Oxytocin>, IOxytocinRepository
    {
        private readonly DataContext context;

        public OxytocinRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Oxytocin UpdateOxytocin(Oxytocin oxytocin)
        {
            try
            {
                var existingInDb = context.Oxytocins
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(oxytocin.PartographId) &&
                        i.OxytocinTime.Equals(oxytocin.OxytocinTime)
                    );
                if (existingInDb == null)
                {
                    existingInDb = new Oxytocin()
                    {
                        PartographId = oxytocin.PartographId,
                        OxytocinTime = oxytocin.OxytocinTime,
                        OxytocinDetails = oxytocin.OxytocinDetails,
                        IsSynced = false,
                        IsDeleted = false,
                    };
                    context.Oxytocins.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.OxytocinDetails != oxytocin.OxytocinDetails)
                    {
                        existingInDb.OxytocinDetails = oxytocin.OxytocinDetails;
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