using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Lion
 * Date created  : 25.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class GlasgowComaScaleController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<GlasgowComaScaleController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public GlasgowComaScaleController(IUnitOfWork context, ILogger<GlasgowComaScaleController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/glasgow-coma-scale
        /// </summary>
        /// <param name="glasgowComa">GlasgowComaScale object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateGlasgowComaScale)]
        public async Task<IActionResult> CreateGlasgowComaScale(GlasgowComaScale glasgowComa)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.GlasgowComaScale, glasgowComa.EncounterType);
                interaction.EncounterId = glasgowComa.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = glasgowComa.CreatedBy;
                interaction.CreatedIn = glasgowComa.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                glasgowComa.InteractionId = interactionId;
                glasgowComa.EncounterId = glasgowComa.EncounterId;
                glasgowComa.ClientId = glasgowComa.ClientId;
                glasgowComa.DateCreated = DateTime.Now;
                glasgowComa.IsDeleted = false;
                glasgowComa.IsSynced = false;

                context.GlasgowComaScaleRepository.Add(glasgowComa);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadGlasgowComaScaleByKey", new { key = glasgowComa.InteractionId }, glasgowComa);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateGlasgowComaScale", "GlasgowComaScaleController.cs", ex.Message, glasgowComa.CreatedIn, glasgowComa.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/glasgow-coma-scales
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGlasgowComaScales)]
        public async Task<IActionResult> ReadGlasgowComaScales()
        {
            try
            {
                var glasgowComaInDb = await context.GlasgowComaScaleRepository.GetGlasgowComaScales();

                return Ok(glasgowComaInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGlasgowComaScales", "GlasgowComaScaleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/glasgow-coma-scale/by-client/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadGlasgowComaScaleByClient)]
        public async Task<IActionResult> ReadGlasgowComaScalesByClient(Guid clientId, int page, int pageSize, EncounterType encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                  //  var glasgowComaInDb = await context.GlasgowComaScaleRepository.GetGlasgowComaScalesByClientID(clientId);
                    var glasgowComaInDb = await context.GlasgowComaScaleRepository.GetGlasgowComaScalesByClientIDLast24Hours(clientId);

                    return Ok(glasgowComaInDb);
                }
                else
                {
                    var glasgowComaInDb = await context.GlasgowComaScaleRepository.GetGlasgowComaScalesByClientID(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<GlasgowComaScale> glasgowComaScaleWithPaggingDto = new PagedResultDto<GlasgowComaScale>()
                    {
                        Data = glasgowComaInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.GlasgowComaScaleRepository.GetGlasgowComaScalesByClientIDTotalCount(clientId, encounterType)
                    };

                    return Ok(glasgowComaScaleWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGlasgowComaScalesByClient", "GlasgowComaScaleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/glasgow-coma-scale/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ReadGlasgowComaScales.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGlasgowComaScaleByKey)]
        public async Task<IActionResult> ReadGlasgowComaScaleByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var glasgowComaInDb = await context.GlasgowComaScaleRepository.GetGlasgowComaScaleByKey(key);

                if (glasgowComaInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(glasgowComaInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGlasgowComaScaleByKey", "GlasgowComaScaleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/glasgow-coma-scale/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GlasgowComaScales.</param>
        /// <param name="glasgowComaScale">GlasgowComaScale to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateGlasgowComaScale)]
        public async Task<IActionResult> UpdateGlasgowComaScale(Guid key, GlasgowComaScale glasgowComaScale)
        {
            try
            {
                if (key != glasgowComaScale.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = glasgowComaScale.ModifiedBy;
                interactionInDb.ModifiedIn = glasgowComaScale.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                glasgowComaScale.DateModified = DateTime.Now;
                glasgowComaScale.IsDeleted = false;
                glasgowComaScale.IsSynced = false;
                glasgowComaScale.ClientId = glasgowComaScale.ClientId;

                context.GlasgowComaScaleRepository.Update(glasgowComaScale);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateGlasgowComaScale", "GlasgowComaScaleController.cs", ex.Message, glasgowComaScale.ModifiedIn, glasgowComaScale.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/glasgow-coma-scale/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GlasgowComaScales.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteGlasgowComaScale)]
        public async Task<IActionResult> DeleteGlasgowComaScale(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var glasgowComaScaleInDb = await context.GlasgowComaScaleRepository.GetGlasgowComaScaleByKey(key);

                if (glasgowComaScaleInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                context.GlasgowComaScaleRepository.Update(glasgowComaScaleInDb);
                await context.SaveChangesAsync();

                return Ok(glasgowComaScaleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteGlasgowComaScale", "GlasgowComaScaleController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}