using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CervixRepository : Repository<Cervix>, ICervixRepository
    {
        private readonly DataContext context;

        public CervixRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Cervix UpdateCervix(Cervix cervix)
        {
            try
            {
                var existingInDb = context.Cervixes
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(cervix.PartographId) &&
                        i.CervixTime.Equals(cervix.CervixTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Cervix()
                    {
                        PartographId = cervix.PartographId,
                        CervixTime = cervix.CervixTime,
                        CervixDetails = cervix.CervixDetails,
                        IsSynced = false,
                        IsDeleted = false,
                    };
                    context.Cervixes.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.CervixDetails != cervix.CervixDetails)
                    {
                        existingInDb.CervixDetails = cervix.CervixDetails;
                        existingInDb.IsSynced = false;
                        existingInDb.IsDeleted = false;
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