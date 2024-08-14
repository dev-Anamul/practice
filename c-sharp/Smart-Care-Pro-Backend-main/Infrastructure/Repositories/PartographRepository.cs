using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PartographRepository : Repository<Partograph>, IPartographRepository
    {
        private readonly DataContext context;

        public PartographRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Guid> GetPartographIdByAdmissionId(Guid aid)
        {
            var Partograph = await context.Partographs.Where(e => e.AdmissionId == aid).FirstOrDefaultAsync();

            if (Partograph == null)
                Partograph = new Partograph();

            return Partograph.InteractionId;
        }

        public async Task<Partograph> GetPartographByEncounter(Guid encounterId)
        {
            var Partograph = await context.Partographs.Where(e => e.EncounterId == encounterId).FirstOrDefaultAsync();

            if (Partograph == null)
                Partograph = new Partograph();

            return Partograph;
        }

        public async Task<Partograph> GetPartographByClient(Guid clientId)
        {
            var partograph = await context.Partographs.Where(e => e.EncounterId == clientId).Include(x=>x.Encounters).ThenInclude(x =>x.Client).FirstOrDefaultAsync();

            if (partograph == null)
               return new Partograph();

            return  partograph;
        }

    }
}