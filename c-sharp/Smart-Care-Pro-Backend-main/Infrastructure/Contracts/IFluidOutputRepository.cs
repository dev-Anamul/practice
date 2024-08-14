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
   public interface IFluidOutputRepository : IRepository<FluidOutput>
   {
      /// <summary>
      /// The method is used to get a birth record by key.
      /// </summary>
      /// <param name="key">Primary key of the table FluidOutputs.</param>
      /// <returns>Returns a birth record if the key is matched.</returns>
      public Task<FluidOutput> GetFluidOutputByKey(Guid key);

      /// <summary>
      /// The method is used to get the FluidOutput data.
      /// </summary>
      /// <param name="encounterId">Primary key of Fluid Table</param>
      /// <returns>List of FluidOutput object</returns>
      public Task<IEnumerable<FluidOutput>> GetFluidOutputByEncounter(Guid encounterId);

      /// <summary>
      /// The method is used to get the list of FluidOutputs.
      /// </summary>
      /// <returns>Returns a list of all FluidOutputs.</returns>
      public Task<IEnumerable<FluidOutput>> GetFluidOutputs();

      /// <summary>
      /// The method is used to get a FluidOutput by FluidId.
      /// </summary>
      /// <param name="fluidId"></param>
      /// <returns>Returns a FluidOutputs if the FluidId is matched.</returns>
      public Task<IEnumerable<FluidOutput>> GetFluidOutputByFluid(Guid fluidId);
   }
}