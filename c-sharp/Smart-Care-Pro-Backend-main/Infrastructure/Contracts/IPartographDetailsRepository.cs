using Domain.Dto;
using Domain.Entities;

namespace Infrastructure.Contracts
{
   public interface IPartographDetailsRepository : IRepository<PartographDetail>
   {
      Task<PartographDetailReadDto> GetPartographDetailsAsync(Guid partographId);


        Task<PartographDetailReadbyIdDto> GetPartographDetailsbyPartograph(Guid partographId);
    }
}