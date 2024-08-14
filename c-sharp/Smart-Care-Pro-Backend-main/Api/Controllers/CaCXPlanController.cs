using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace Api.Controllers
{
    /// <summary>
    ///caCXPlan Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class CaCXPlanController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CaCXPlanController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CaCXPlanController(IUnitOfWork context, ILogger<CaCXPlanController> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        /// <summary>
        /// URL: sc-api/caCXPlan
        /// </summary>
        /// <param name="caCXPlan">caCXPlan object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCACXPlan)]
        public async Task<IActionResult> CreateCACXPlan(CaCXPlan caCXPlan)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.CACXPlan, caCXPlan.EncounterType);
                interaction.EncounterId = caCXPlan.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = caCXPlan.CreatedBy;
                interaction.CreatedIn = caCXPlan.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);


                caCXPlan.InteractionId = interactionId;
                caCXPlan.DateCreated = DateTime.Now;
                caCXPlan.IsDeleted = false;
                caCXPlan.IsSynced = false;

                context.CaCXPlanRepository.Add(caCXPlan);

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadcaCXPlanByKey", new { key = caCXPlan.InteractionId }, caCXPlan);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "caCXPlan", "caCXPlanController.cs", ex.Message, caCXPlan.CreatedIn, caCXPlan.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/caCXPlans
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCACXPlan)]
        public async Task<IActionResult> ReadCACXPlan()
        {
            try
            {
                var caCXPlanInDb = await context.CaCXPlanRepository.GetCaCXPlan();

                return Ok(caCXPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadcaCXPlan", "CACXPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cacxplan/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table cacxplan.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCACXPlanByKey)]
        public async Task<IActionResult> ReadCACXPlanByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var cacxplanInDb = await context.CaCXPlanRepository.GetCaCXPlanByKey(key);

                if (cacxplanInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(cacxplanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadcacxplanByKey", "cacxplanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// CACXPlan-by-client/by-client/{clientid}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadCACXPlanByClient)]
        public async Task<IActionResult> ReadCACXPlanByClient(Guid clientId)
        {
            try
            {
                var cacxPlanInDb = await context.CaCXPlanRepository.GetCaCXPlanbyClienId(clientId);

                return Ok(cacxPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCACXPlanByClient", "CACXPlanClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/CACXPlan/by-encounter/{encounterId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCACXPlanByEncounter)]
        public async Task<IActionResult> ReadCACXPlanByEncounter(Guid EncounterId)
        {
            try
            {
                var cacxPlanInDb = await context.CaCXPlanRepository.GetCaCXPlanByEncounter(EncounterId);

                return Ok(cacxPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCACXPlanByEncounter", "CACXPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/cACXPlan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CACXPlan.</param>
        /// <param name="cACXPlan">cACXPlan to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCACXPlan)]
        public async Task<IActionResult> UpdateCACXPlan(Guid key, CaCXPlan cACXPlan)
        {
            try
            {
                if (key != cACXPlan.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var cACXPlanInDb = await context.CaCXPlanRepository.GetCaCXPlanByKey(key);

                if (cACXPlanInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                cACXPlanInDb.DateModified = DateTime.Now;
                cACXPlanInDb.IsDeleted = false;
                cACXPlanInDb.IsSynced = false;

                cACXPlanInDb.EncounterId = cACXPlan.EncounterId;
                cACXPlanInDb.EncounterType = cACXPlan.EncounterType;
                cACXPlanInDb.IsAtypicalVessels = cACXPlan.IsAtypicalVessels;
                cACXPlanInDb.IsClientReffered = cACXPlan.IsClientReffered;
                cACXPlanInDb.IsPostLEEPLesion = cACXPlan.IsPostLEEPLesion;
                cACXPlanInDb.IsLesionCovers = cACXPlan.IsLesionCovers;
                cACXPlanInDb.IsLesionExtendsIntoTheCervicalOs = cACXPlan.IsLesionExtendsIntoTheCervicalOs;
                cACXPlanInDb.IsPoly = cACXPlan.IsPoly;
                cACXPlanInDb.IsQueryICC = cACXPlan.IsQueryICC;
                cACXPlanInDb.IsPunctationsOrMoasicm = cACXPlan.IsPunctationsOrMoasicm;
                cACXPlanInDb.IsLesionTooLargeForThermoAblation = cACXPlan.IsLesionTooLargeForThermoAblation;
                cACXPlanInDb.IsLesionToThickForAblation = cACXPlan.IsLesionToThickForAblation;
                cACXPlanInDb.Others = cACXPlan.Others;

                context.CaCXPlanRepository.Update(cACXPlanInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateCACXPlanVisit", "CACXPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: sc-api/CACXPlan/{key}
        /// </summary>
        /// <param name="key">Primary key of the table CACXPlan.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCACXPlan)]
        public async Task<IActionResult> DeleteCACXPlan(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var cacxPlanInDb = await context.CaCXPlanRepository.GetCaCXPlanByKey(key);

                if (cacxPlanInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                cacxPlanInDb.IsDeleted = true;
                cacxPlanInDb.IsSynced = false;
                cacxPlanInDb.DateModified = DateTime.Now;

                context.CaCXPlanRepository.Update(cacxPlanInDb);
                await context.SaveChangesAsync();

                return Ok(cacxPlanInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCACXPlan", "CACXPlanController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}