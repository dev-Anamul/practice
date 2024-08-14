using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
*Created by: Stephan
* Date created: 29.04.2023
* Modified by: Stephan
* Last modified: 13.08.2023
* Reviewed by:
*Date reviewed:
*/
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVolumesRepository interface.
    /// </summary>
    public class VolumesRepository : Repository<Volume>, IVolumesRepository
    {
        private readonly DataContext context;

        public VolumesRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Volume UpdateVolume(Volume volume)
        {
            try
            {
                var existingInDb = context.Volumes
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(volume.PartographId) &&
                        i.VolumesTime.Equals(volume.VolumesTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Volume()
                    {
                        PartographId = volume.PartographId,
                        VolumesDetails = volume.VolumesDetails,
                        VolumesTime = volume.VolumesTime,
                        IsSynced = false,
                        IsDeleted = false,
                        DateCreated=DateTime.Now
                    };
                    context.Volumes.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.VolumesDetails != volume.VolumesDetails)
                    {
                        existingInDb.VolumesDetails = volume.VolumesDetails;
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