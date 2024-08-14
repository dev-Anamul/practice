using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContractionsRepository : Repository<Contraction>, IContractionsRepository
    {
        private readonly DataContext context;

        public ContractionsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Contraction UpdateContraction(Contraction contraction)
        {
            try
            {
                var existingInDb = context.Contractions
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(contraction.PartographId) &&
                        i.ContractionsTime.Equals(contraction.ContractionsTime)
                    );
                if (existingInDb == null)
                {
                    existingInDb = new Contraction()
                    {
                        PartographId = contraction.PartographId,
                        ContractionsTime = contraction.ContractionsTime,
                        ContractionsDetails = contraction.ContractionsDetails,
                        Duration = contraction.Duration,
                        IsDeleted = false,
                        IsSynced = false,
                    };
                    context.Contractions.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.ContractionsDetails != contraction.ContractionsDetails || existingInDb.Duration != contraction.Duration)
                    {

                        existingInDb.ContractionsDetails = contraction.ContractionsDetails;
                        existingInDb.Duration = contraction.Duration;
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