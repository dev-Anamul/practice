using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 09.02.2023
 * Modified by   : Bella
 * Last modified : 13.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
   public interface IFluidInputRepository : IRepository<FluidIntake>
   {
      /// <summary>
      /// The method is used to get a Fluid Input by key.
      /// </summary>
      /// <param name="key">Primary key of the table FluidInputs.</param>
      /// <returns>Returns a birth record if the key is matched.</returns>
      public Task<FluidIntake> GetFluidInputByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of Fluid Inputs.
      /// </summary>
      /// <returns>Returns a list of all birth histories.</returns>
      public Task<IEnumerable<FluidIntake>> GetFluidInputs();

      /// <summary>
      /// The method is used to get a FluidInput by FluidId.
      /// </summary>
      /// <param name="fluidId"></param>
      /// <returns>Returns a birth record if the FluidID is matched.</returns>
      public Task<IEnumerable<FluidIntake>> GetFluidInputByFluid(Guid fluidId);

      /// <summary>
      /// The method is used to get a FluidInput by Encounter.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a FluidInput if the EncounterId is matched.</returns>
      public Task<IEnumerable<FluidIntake>> GetFluidInputByEncounter(Guid encounterId);
   }
}