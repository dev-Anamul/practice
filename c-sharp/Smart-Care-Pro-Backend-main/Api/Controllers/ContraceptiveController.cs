using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 25.12.2022
 * Modified by  : Lion
 * Last modified: 21.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    ///Contraceptive Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class ContraceptiveController : ControllerBase
    {
        private Contraceptive contraceptive;
        private readonly ILogger<ContraceptiveController> logger;
        private readonly IUnitOfWork context;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ContraceptiveController(IUnitOfWork context, ILogger<ContraceptiveController> logger)
        {
            contraceptive = new Contraceptive();
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/contraceptive
        /// </summary>
        /// <param name="Contraceptive"> Contraceptive object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateContraceptive)]
        public async Task<ActionResult<Contraceptive>> CreateContraceptive(Contraceptive contraceptive)
        {
            try
            {
                contraceptive.DateCreated = DateTime.Now;
                contraceptive.IsDeleted = false;
                contraceptive.IsSynced = false;

                context.ContraceptiveRepository.Add(contraceptive);
                await context.SaveChangesAsync();

                return Ok(contraceptive);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateContraceptive", "ContraceptiveController.cs", ex.Message, contraceptive.CreatedIn, contraceptive.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/contraceptives
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadContraceptives)]
        public async Task<IActionResult> ReadContraceptives()
        {
            try
            {
                var contraceptiveInDb = await context.ContraceptiveRepository.GetContraceptives();

                contraceptiveInDb = contraceptiveInDb.OrderByDescending(x => x.DateCreated);

                return Ok(contraceptiveInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadContraceptives", "ContraceptiveController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/contraceptive/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Contraceptive.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadContraceptiveByKey)]
        public async Task<IActionResult> ReadContraceptiveByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var contraceptiveInDb = await context.ContraceptiveRepository.GetContraceptiveByKey(key);

                if (contraceptiveInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(contraceptiveInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadContraceptiveByKey", "ContraceptiveController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        ///URL: sc-api/contraceptive/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Contraceptive.</param>
        /// <param name="contraceptive">Contraceptive to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateContraceptive)]
        public async Task<IActionResult> UpdateContraceptive(int key, Contraceptive contraceptive)
        {
            try
            {
                if (key != contraceptive.Oid)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsContraceptiveDuplicate(contraceptive) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                contraceptive.DateModified = DateTime.Now;
                contraceptive.IsDeleted = false;
                contraceptive.IsSynced = false;

                context.ContraceptiveRepository.Update(contraceptive);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateContraceptive", "ContraceptiveController.cs", ex.Message, contraceptive.ModifiedIn, contraceptive.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/contraceptive/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Contraceptive.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteContraceptive)]
        public async Task<IActionResult> DeleteContraceptive(int key)
        {
            try
            {
                if (key == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var contraceptiveInDb = await context.ContraceptiveRepository.GetContraceptiveByKey(key);

                if (contraceptiveInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                contraceptiveInDb.DateModified = DateTime.Now;
                contraceptiveInDb.IsDeleted = true;
                contraceptiveInDb.IsSynced = false;

                context.ContraceptiveRepository.Update(contraceptiveInDb);
                await context.SaveChangesAsync();

                return Ok(contraceptiveInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteContraceptive", "ContraceptiveController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// Checks whether the Contraceptive name is duplicate or not.
        /// </summary>
        /// <param name="Contraceptive">Contraceptive object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsContraceptiveDuplicate(Contraceptive contraceptive)
        {
            try
            {
                var contraceptiveInDb = await context.ContraceptiveRepository.GetContraceptiveByName(contraceptive.Description);

                if (contraceptiveInDb != null)
                    if (contraceptiveInDb.Oid != contraceptiveInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsContraceptiveDuplicate", "ContraceptiveController.cs", ex.Message);
                throw;
            }
        }
    }
}