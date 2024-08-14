using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ReferralModuleController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ReferralModuleController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ReferralModuleController(IUnitOfWork context, ILogger<ReferralModuleController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/referal-module
        /// </summary>
        /// <param name="ReferralModule">ReferralModule object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateReferralModule)]
        public async Task<IActionResult> CreateReferralModule(ReferralModule referralModule)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ReferralModule, referralModule.EncounterType);
                interaction.EncounterId = referralModule.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = referralModule.CreatedBy;
                interaction.CreatedIn = referralModule.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                referralModule.InteractionId = interactionId;
                referralModule.EncounterId = referralModule.EncounterId;
                referralModule.ClientId = referralModule.ClientId;
                referralModule.DateCreated = DateTime.Now;
                referralModule.IsDeleted = false;
                referralModule.IsSynced = false;

                context.ReferralModuleRepository.Add(referralModule);
                await context.SaveChangesAsync();

                if (referralModule.IdentifiedReferralReasonsList != null)
                {
                    foreach (var item in referralModule.IdentifiedReferralReasonsList)
                    {
                        IdentifiedReferralReason identifiedReferralReason = new IdentifiedReferralReason();

                        identifiedReferralReason.InteractionId = Guid.NewGuid();
                        identifiedReferralReason.ReasonOfReferralId = item;
                        identifiedReferralReason.ReferralModuleId = referralModule.InteractionId;

                        identifiedReferralReason.CreatedIn = referralModule.CreatedIn;
                        identifiedReferralReason.CreatedBy = referralModule.CreatedBy;
                        identifiedReferralReason.DateCreated = DateTime.Now;
                        identifiedReferralReason.IsSynced = false;
                        identifiedReferralReason.IsDeleted = false;

                        context.IdentifiedReferralReasonRepository.Add(identifiedReferralReason);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadReferralModuleByKey", new { key = referralModule.InteractionId }, referralModule);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateReferralModule", "ReferralModuleController.cs", ex.Message, referralModule.CreatedIn, referralModule.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/referal-modules
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadReferralModules)]
        public async Task<IActionResult> ReadReferralModules()
        {
            try
            {
                var referralModuleInDb = await context.ReferralModuleRepository.GetReferralModules();

                return Ok(referralModuleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadReferralModules", "ReferralModuleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/referal-modules/by-client/clientId
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadReferralModuleByClient)]
        public async Task<IActionResult> ReadReferralModuleByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var referralModuleInDb = await context.ReferralModuleRepository.GetReferralModuleByClient(clientId);

                    foreach (var item in referralModuleInDb)
                    {
                        item.IdentifiedReferralReasons = await context.IdentifiedReferralReasonRepository.LoadListWithChildAsync<IdentifiedReferralReason>(g => (g.IsDeleted == false || g.IsDeleted == null) && g.ReferralModuleId == item.InteractionId, p => p.ReasonOfReferral);
                        item.ReferredFacility = context.FacilityRepository.GetByIdAsync((int)item.ReferredFacilityId).Result?.Description;
                    }

                    return Ok(referralModuleInDb);
                }
                else
                {
                    var referralModuleInDb = await context.ReferralModuleRepository.GetReferralModuleByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    foreach (var item in referralModuleInDb)
                    {
                        item.IdentifiedReferralReasons = await context.IdentifiedReferralReasonRepository.LoadListWithChildAsync<IdentifiedReferralReason>(g => (g.IsDeleted == false || g.IsDeleted == null) && g.ReferralModuleId == item.InteractionId, p => p.ReasonOfReferral);
                        item.ReferredFacility = context.FacilityRepository.GetByIdAsync((int)item.ReferredFacilityId).Result?.Description;
                    }

                    PagedResultDto<ReferralModule> referralModuleDto = new PagedResultDto<ReferralModule>()
                    {
                        Data = referralModuleInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.ReferralModuleRepository.GetReferralModuleByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(referralModuleDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadReferralModuleByClient", "ReferralModuleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/referal-modules/by-encounter/encounterId
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadReferralModuleByEncounter)]
        public async Task<IActionResult> ReadReferralModuleByEncounter(Guid encounterId)
        {
            try
            {
                var referralModuleInDb = await context.ReferralModuleRepository.GetReferralModuleByEncounter(encounterId);

                return Ok(referralModuleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadReferralModuleByEncounter", "ReferralModuleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/referal-module/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadReferralModuleByKey)]
        public async Task<IActionResult> ReadReferralModuleByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var referralModuleInDb = await context.ReferralModuleRepository.GetReferralModuleByKey(key);

                if (referralModuleInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(referralModuleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadReferralModuleByKey", "ReferralModuleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/referal-module/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <param name="ReferralModule">ReferralModule to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateReferralModule)]
        public async Task<IActionResult> UpdateReferralModule(Guid key, ReferralModule referralModule)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = referralModule.ModifiedBy;
                interactionInDb.ModifiedIn = referralModule.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != referralModule.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                referralModule.DateModified = DateTime.Now;
                referralModule.IsDeleted = false;
                referralModule.IsSynced = false;

                context.ReferralModuleRepository.Update(referralModule);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateReferralModule", "ReferralModuleController.cs", ex.Message, referralModule.ModifiedIn, referralModule.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/referal-module/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteReferralModule)]
        public async Task<IActionResult> DeleteReferralModule(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var referralModuleInDb = await context.ReferralModuleRepository.GetReferralModuleByKey(key);

                if (referralModuleInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);


                referralModuleInDb.DateModified = DateTime.Now;
                referralModuleInDb.IsDeleted = true;
                referralModuleInDb.IsSynced = false;

                context.ReferralModuleRepository.Update(referralModuleInDb);
                await context.SaveChangesAsync();

                return Ok(referralModuleInDb);
                //if (referralModuleInDb.Count() > 0)
                //{
                //    foreach (var item in referralModuleInDb)
                //    {
                //        item.DateModified = DateTime.Now;
                //        item.IsDeleted = true;
                //        item.IsSynced = false;

                //        context.ReferralModuleRepository.Update(item);
                //        await context.SaveChangesAsync();

                //        return Ok(referralModuleInDb);
                //    }
                //}

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteReferralModule", "ReferralModuleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

            return NotFound();
        }
    }
}