using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// NeonatalInjury controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class NeonatalInjuryController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<NeonatalInjuryController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public NeonatalInjuryController(IUnitOfWork context, ILogger<NeonatalInjuryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/neonatal-injury
        /// </summary>
        /// <param name="neonatalInjury">NeonatalInjury object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateNeonatalInjury)]
        public async Task<IActionResult> CreateNeonatalInjury(NeonatalInjury neonatalInjury)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.NeonatalInjury, neonatalInjury.EncounterType);
                interaction.EncounterId = neonatalInjury.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = neonatalInjury.CreatedBy;
                interaction.CreatedIn = neonatalInjury.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                var newBornDetails = await context.NewBornDetailRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == neonatalInjury.NeonatalId);

                if (newBornDetails == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                neonatalInjury.InteractionId = interactionId;
                neonatalInjury.CreatedIn = newBornDetails.CreatedIn;
                neonatalInjury.CreatedBy = newBornDetails.CreatedBy;
                neonatalInjury.DateCreated = DateTime.Now;
                neonatalInjury.IsDeleted = false;
                neonatalInjury.IsSynced = false;

                if (neonatalInjury.InjuriesList != null)
                {
                    foreach (var item in neonatalInjury.InjuriesList)
                    {
                        NeonatalInjury neonatalInjuryItem = new NeonatalInjury();

                        neonatalInjuryItem.NeonatalId = newBornDetails.InteractionId;
                        neonatalInjuryItem.CreatedIn = neonatalInjury.CreatedIn;
                        neonatalInjuryItem.CreatedBy = neonatalInjury.CreatedBy;
                        neonatalInjuryItem.EncounterId = neonatalInjury.EncounterId;
                        neonatalInjuryItem.InteractionId = Guid.NewGuid();
                        neonatalInjuryItem.Injuries = item;
                        neonatalInjuryItem.DateCreated = DateTime.Now;
                        neonatalInjuryItem.IsDeleted = false;
                        neonatalInjuryItem.IsSynced = false;

                        context.NeonatalInjuryRepository.Add(neonatalInjuryItem);
                    }
                }
                else
                {
                    neonatalInjury.NeonatalId = newBornDetails.InteractionId;
                    neonatalInjury.EncounterId = newBornDetails.EncounterId;
                    neonatalInjury.InteractionId = interactionId;
                    neonatalInjury.DateCreated = DateTime.Now;
                    neonatalInjury.IsDeleted = false;
                    neonatalInjury.IsSynced = false;

                    context.NeonatalInjuryRepository.Add(neonatalInjury);
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadNeonatalInjuryByKey", new { key = neonatalInjury.InteractionId }, neonatalInjury);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateNeonatalInjury", "NeonatalInjuryController.cs", ex.Message, neonatalInjury.CreatedIn, neonatalInjury.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-injuries
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalInjuries)]
        public async Task<IActionResult> ReadNeonatalInjuries()
        {
            try
            {
                var neonatalInjuryInDb = await context.NeonatalInjuryRepository.GetNeonatalInjuries();

                return Ok(neonatalInjuryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalInjuries", "NeonatalInjuryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-injury/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalInjuries.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalInjuryByKey)]
        public async Task<IActionResult> ReadNeonatalInjuryByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalInjuryInDb = await context.NeonatalInjuryRepository.GetNeonatalInjuryByKey(key);

                if (neonatalInjuryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(neonatalInjuryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalInjuryByKey", "NeonatalInjuryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-injury/byNeonatal/{NeonatalId}
        /// </summary>
        /// <param name="neonatalId">Primary key of the table NewBornDetails.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadNeonatalInjuryByNeonatal)]
        public async Task<IActionResult> ReadNeonatalInjuryByNeonatal(Guid neonatalId)
        {
            try
            {
                var neonatalInDb = await context.NeonatalInjuryRepository.GetNeonatalInjuryByNeonatal(neonatalId);
               

                return Ok(neonatalInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadNeonatalInjuryByNeonatal", "NeonatalInjuryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-injury/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalInjuries.</param>
        /// <param name="neonatalInjury">NeonatalInjury to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateNeonatalInjury)]
        public async Task<IActionResult> UpdateNeonatalInjury(Guid key, NeonatalInjury neonatalInjury)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = neonatalInjury.ModifiedBy;
                interactionInDb.ModifiedIn = neonatalInjury.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != neonatalInjury.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                
                neonatalInjury.DateModified = DateTime.Now;
                neonatalInjury.IsDeleted = false;
                neonatalInjury.IsSynced = false;
                context.NeonatalInjuryRepository.Update(neonatalInjury);

                await context.SaveChangesAsync();

            

                if (neonatalInjury.InjuriesList != null)
                {
                    var dbInjuriesList = await context.NeonatalInjuryRepository.GetNeonatalInjuryByNeonatal(key);
                    if (dbInjuriesList is not null)
                    {
                        foreach (var item in dbInjuriesList.ToList())
                        {
                            context.NeonatalInjuryRepository.Delete(item);
                            await context.SaveChangesAsync();
                        }
                    }

                    foreach (var item in neonatalInjury.InjuriesList)
                    {
                        NeonatalInjury neonatalInjuryItem = new NeonatalInjury();

                        neonatalInjuryItem.NeonatalId = neonatalInjury.InteractionId;
                        neonatalInjuryItem.CreatedIn = neonatalInjury.CreatedIn;
                        neonatalInjuryItem.CreatedBy = neonatalInjury.CreatedBy;
                        neonatalInjuryItem.EncounterId = neonatalInjury.EncounterId;
                        neonatalInjuryItem.InteractionId = Guid.NewGuid();
                        neonatalInjuryItem.Injuries = item;
                        neonatalInjuryItem.DateCreated = DateTime.Now;
                        neonatalInjuryItem.IsDeleted = false;
                        neonatalInjuryItem.IsSynced = false;

                        context.NeonatalInjuryRepository.Add(neonatalInjuryItem);
                    }
                    await context.SaveChangesAsync();
                }
                


                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateNeonatalInjury", "NeonatalInjuryController.cs", ex.Message, neonatalInjury.ModifiedIn, neonatalInjury.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/neonatal-injury/{key}
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalInjuries.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteNeonatalInjury)]
        public async Task<IActionResult> DeleteNeonatalInjury(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var neonatalInjuryInDb = await context.NeonatalInjuryRepository.GetNeonatalInjuryByKey(key);

                if (neonatalInjuryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                neonatalInjuryInDb.IsDeleted = true;
                context.NeonatalInjuryRepository.Update(neonatalInjuryInDb);

                await context.SaveChangesAsync();

                return Ok(neonatalInjuryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteNeonatalInjury", "NeonatalInjuryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}
