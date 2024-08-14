using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IInsertionAndRemovalProcedureRepository : IRepository<InsertionAndRemovalProcedure>
   {
      /// <summary>
      /// The method is used to get a InsertionAndRemovalProcedure by key.
      /// </summary>
      /// <param name="key">Primary key of the table InsertionAndRemovalProcedures.</param>
      /// <returns>Returns a InsertionAndRemovalProcedure if the key is matched.</returns>
      public Task<InsertionAndRemovalProcedure> GetInsertionAndRemovalProcedureByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of InsertionAndRemovalProcedures.
      /// </summary>
      /// <returns>Returns a list of all InsertionAndRemovalProcedures.</returns>
      public Task<IEnumerable<InsertionAndRemovalProcedure>> GetInsertionAndRemovalProcedures();

      /// <summary>
      /// The method is used to get a InsertionAndRemovalProcedure by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a InsertionAndRemovalProcedure if the ClientID is matched.</returns>
      public Task<IEnumerable<InsertionAndRemovalProcedure>> GetInsertionAndRemovalProcedureByClient(Guid clientId);

      /// <summary>
      /// The method is used to get the list of InsertionAndRemovalProcedure by encounterId.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a list of all InsertionAndRemovalProcedure by encounterId.</returns>
      public Task<IEnumerable<InsertionAndRemovalProcedure>> GetInsertionAndRemovalProcedureByEncounter(Guid encounterId);
   }
}