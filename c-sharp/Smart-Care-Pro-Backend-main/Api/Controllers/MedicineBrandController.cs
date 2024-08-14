using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;



namespace Api.Controllers
{
    /// <summary>
    /// MedicineBrand controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]


    public class MedicineBrandController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MedicineBrandController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MedicineBrandController(IUnitOfWork context, ILogger<MedicineBrandController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/medicine-brand
        /// </summary>
        /// <param name="medicineBrand">MedicineBrand object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMedicineBrand)]
        public async Task<ActionResult<MedicineBrand>> CreateMedicineBrand(MedicineBrand medicineBrand)
        {
            try
            {
                if (await IsMedicineBrandsDuplicate(medicineBrand) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                medicineBrand.DateCreated = DateTime.Now;
                medicineBrand.IsDeleted = false;
                medicineBrand.IsSynced = false;

                context.MedicineBrandRepository.Add(medicineBrand);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadMedicineBrandByKey", new { key = medicineBrand.Oid }, medicineBrand);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateMedicineBrand", "MedicineBrandController.cs", ex.Message, medicineBrand.CreatedIn, medicineBrand.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medicine-brands
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicineBrand)]
        public async Task<IActionResult> ReadMedicineBrand()
        {
            try
            {
                var medicineBrandInDb = await context.MedicineBrandRepository.GetMedicineBrand();

                return Ok(medicineBrandInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicineBrand", "MedicineBrandController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medicine-brand/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicineBrand.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicineBrandByKey)]
        public async Task<IActionResult> ReadMedicineBrandByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicineBrandInDb = await context.MedicineBrandRepository.GetMedicineBrandByKey(key);

                if (medicineBrandInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(medicineBrandInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicineBrandByKey", "MedicineBrandController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medicine-brand
        /// </summary>
        /// <param name="key">Primary key of the table MedicineBrand.</param>
        /// <param name="medicineBrandInDb">MedicineBrand to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMedicineBrand)]
        public async Task<IActionResult> UpdateMedicineBrand(int key, MedicineBrand medicineBrand)
        {
            try
            {
                if (key != medicineBrand.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                medicineBrand.DateModified = DateTime.Now;
                medicineBrand.IsDeleted = false;
                medicineBrand.IsSynced = false;

                context.MedicineBrandRepository.Update(medicineBrand);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMedicineBrand", "MedicineBrandController.cs", ex.Message, medicineBrand.ModifiedIn, medicineBrand.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medicine-brand
        /// </summary>
        /// <param name="key">Primary key of the table MedicineBrand.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMedicineBrand)]
        public async Task<IActionResult> DeleteMedicineBrand(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicineBrandInDb = await context.MedicineBrandRepository.GetMedicineBrandByKey(key);

                if (medicineBrandInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                medicineBrandInDb.DateModified = DateTime.Now;
                medicineBrandInDb.IsDeleted = true;
                medicineBrandInDb.IsSynced = false;

                context.MedicineBrandRepository.Update(medicineBrandInDb);
                await context.SaveChangesAsync();

                return Ok(medicineBrandInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMedicineBrand", "MedicineBrandController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the MedicineBrand name is duplicate or not.
        /// </summary>
        /// <param name="medicineBrand">MedicineBrand object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsMedicineBrandsDuplicate(MedicineBrand medicineBrand)
        {
            try
            {
                var medicineBrandInDb = await context.MedicineBrandRepository.GetMedicineBrandByName(medicineBrand.Description);

                if (medicineBrandInDb != null)
                    if (medicineBrandInDb.Oid != medicineBrandInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsMedicineBrandsDuplicate", "MedicineBrandController.cs", ex.Message);
                throw;
            }
        }
    }
}
