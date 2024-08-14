using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProteinsRepository : Repository<Protein>, IProteinsRepository
    {
        private readonly DataContext context;

        public ProteinsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Protein UpdateProtein(Protein protein)
        {
            try
            {
                var existingInDb = context.Proteins
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(protein.PartographId) &&
                        i.ProteinsTime.Equals(protein.ProteinsTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Protein()
                    {
                        PartographId = protein.PartographId,
                        ProteinsDetails = protein.ProteinsDetails,
                        ProteinsTime = protein.ProteinsTime,
                        IsSynced = false,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                    };
                    context.Proteins.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.ProteinsDetails != protein.ProteinsDetails)
                    {

                        existingInDb.ProteinsDetails = protein.ProteinsDetails;
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