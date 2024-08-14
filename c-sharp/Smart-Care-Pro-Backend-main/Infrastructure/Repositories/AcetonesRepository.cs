using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Lion
 * Date created : 15-08-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class AcetoneRepository : Repository<Acetone>, IAcetonesRepository
    {
        private readonly DataContext context;

        public AcetoneRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Acetone UpdateAcetone(Acetone acetone)
        {
            try
            {
                var existingInDb = context.Acetones
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(acetone.PartographId) &&
                        i.AcetoneTime.Equals(acetone.AcetoneTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Acetone()
                    {
                        PartographId = acetone.PartographId,
                        AcetoneTime = acetone.AcetoneTime,
                        Description = acetone.Description,
                        IsSynced = false,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                    };
                    context.Acetones.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.Description != acetone.Description)
                    {
                        existingInDb.Description = acetone.Description;
                        existingInDb.IsDeleted = false;
                        existingInDb.IsSynced = false;
                        existingInDb.DateModified = DateTime.Now;
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