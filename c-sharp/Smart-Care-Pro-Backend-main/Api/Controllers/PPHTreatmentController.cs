using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 01.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// PPHTreatment controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class PPHTreatmentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<PPHTreatmentController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public PPHTreatmentController(IUnitOfWork context, ILogger<PPHTreatmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/pph-treatment
        /// </summary>
        /// <param name="pPHTreatment">PPHTreatment object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreatePPHTreatment)]
        public async Task<IActionResult> CreatePPHTreatment(PPHTreatment pPHTreatment)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.PPHTreatment, pPHTreatment.EncounterType);
                interaction.EncounterId = pPHTreatment.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = pPHTreatment.CreatedBy;
                interaction.CreatedIn = pPHTreatment.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                pPHTreatment.InteractionId = interactionId;
                pPHTreatment.DateCreated = DateTime.Now;
                pPHTreatment.IsDeleted = false;
                pPHTreatment.IsSynced = false;

                var motherDelivery = await context.MotherDeliverySummaryRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == pPHTreatment.DeliveryId);

                if (motherDelivery == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                context.PPHTreatmentRepository.Add(pPHTreatment);
                await context.SaveChangesAsync();

                if (pPHTreatment.TreatmentsOfPPHList != null)
                {
                    foreach (var item in pPHTreatment.TreatmentsOfPPHList)
                    {
                        IdentifiedPPHTreatment identifiedPPHTreatment = new IdentifiedPPHTreatment();

                        identifiedPPHTreatment.InteractionId = Guid.NewGuid();
                        identifiedPPHTreatment.TreatmentsOfPPH = item;
                        identifiedPPHTreatment.PPHTreatmentsId = pPHTreatment.InteractionId;
                        identifiedPPHTreatment.IsDeleted = false;
                        identifiedPPHTreatment.IsSynced = false;
                        identifiedPPHTreatment.DateCreated = DateTime.Now;
                        identifiedPPHTreatment.CreatedBy = pPHTreatment.CreatedBy;
                        identifiedPPHTreatment.CreatedIn = pPHTreatment.CreatedIn;

                        context.IdentifiedPPHTreatmentRepository.Add(identifiedPPHTreatment);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadPPHTreatmentByKey", new { key = pPHTreatment.InteractionId }, pPHTreatment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePPHTreatment", "PPHTreatmentController.cs", ex.Message, pPHTreatment.CreatedIn, pPHTreatment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pph-treatments
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPPHTreatments)]
        public async Task<IActionResult> ReadPPHTreatments()
        {
            try
            {
                var inDb = await context.PPHTreatmentRepository.GetPPHTreatments();

                return Ok(inDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPPHTreatments", "PPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pph-treatment/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPPHTreatmentByKey)]
        public async Task<IActionResult> ReadPPHTreatmentByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var indb = await context.PPHTreatmentRepository.GetPPHTreatmentByKey(key);
                indb.IdentifiedPPHTreatments = await context.IdentifiedPPHTreatmentRepository.GetIdentifiedPPHTreatmentByPPHTreatments(indb.InteractionId);
                int i = 0;
                indb.TreatmentsOfPPHList = new Enums.TreatmentsOfPPH[indb.IdentifiedPPHTreatments.Count()];
                indb.IdentifiedPPHTreatments.ToList().ForEach(x =>
                {
                    indb.TreatmentsOfPPHList[i++] = x.TreatmentsOfPPH;
                });

                if (indb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(indb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPPHTreatmentByKey", "PPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pph-treatment/by-delivery/{deliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadPPHTreatmentByDelivery)]
        public async Task<IActionResult> ReadPPHTreatmentByDelivery(Guid deliveryId)
        {
            try
            {
                var pphTreatmentInDb = await context.PPHTreatmentRepository.GetPPHTreatmentByDelivery(deliveryId);

                return Ok(pphTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPPHTreatmentByDelivery", "PPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pph-treatment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PPHTreatments.</param>
        /// <param name="pPHTreatment">PPHTreatment to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdatePPHTreatment)]
        public async Task<IActionResult> UpdatePPHTreatment(Guid key, PPHTreatment pPHTreatment)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = pPHTreatment.ModifiedBy;
                interactionInDb.ModifiedIn = pPHTreatment.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != pPHTreatment.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                List<IdentifiedPPHTreatment> identifiedPPHTreatments = new();

                PPHTreatment dbPPHTreatment = await context.PPHTreatmentRepository.GetPPHTreatmentByKey(key);

                dbPPHTreatment.BloodAmount = pPHTreatment.BloodAmount;
                dbPPHTreatment.BloodType = pPHTreatment.BloodType;
                dbPPHTreatment.FluidAmount = pPHTreatment.FluidAmount;
                dbPPHTreatment.FluidsGiven = pPHTreatment.FluidsGiven;
                dbPPHTreatment.PPHSugery = pPHTreatment.PPHSugery;

                dbPPHTreatment.DateModified = DateTime.Now;
                dbPPHTreatment.IsSynced = false;
                dbPPHTreatment.IsDeleted = false;

                context.PPHTreatmentRepository.Update(dbPPHTreatment);

                if (pPHTreatment.TreatmentsOfPPHList != null)
                {
                    var dbIdentifiedPPHTreatment = await context.IdentifiedPPHTreatmentRepository.GetIdentifiedPPHTreatmentByPPHTreatments(dbPPHTreatment.InteractionId);

                    if (dbIdentifiedPPHTreatment != null)
                    {
                        foreach (var data in dbIdentifiedPPHTreatment)
                        {
                            context.IdentifiedPPHTreatmentRepository.Delete(data);
                        }
                    }

                    foreach (var item in pPHTreatment.TreatmentsOfPPHList)
                    {
                        IdentifiedPPHTreatment identifiedPPHTreatment = new IdentifiedPPHTreatment();

                        identifiedPPHTreatment.InteractionId = Guid.NewGuid();
                        identifiedPPHTreatment.TreatmentsOfPPH = item;
                        identifiedPPHTreatment.PPHTreatmentsId = pPHTreatment.InteractionId;

                        identifiedPPHTreatment.DateModified = DateTime.Now;
                        identifiedPPHTreatment.IsSynced = false;
                        identifiedPPHTreatment.IsDeleted = false;

                        identifiedPPHTreatment.CreatedIn = pPHTreatment.CreatedIn;
                        identifiedPPHTreatment.CreatedBy = pPHTreatment.CreatedBy;
                        identifiedPPHTreatment.ModifiedBy = pPHTreatment.ModifiedBy;
                        identifiedPPHTreatment.ModifiedIn = pPHTreatment.ModifiedIn;

                        context.IdentifiedPPHTreatmentRepository.Add(identifiedPPHTreatment);

                    }
                }
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePPHTreatment", "PPHTreatmentController.cs", ex.Message, pPHTreatment.ModifiedIn, pPHTreatment.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/pph-treatment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeletePPHTreatment)]
        public async Task<IActionResult> DeletePPHTreatment(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var pPHTreatmentInDb = await context.PPHTreatmentRepository.GetPPHTreatmentByKey(key);
                pPHTreatmentInDb.IdentifiedPPHTreatments = await context.IdentifiedPPHTreatmentRepository.GetIdentifiedPPHTreatmentByPPHTreatments(pPHTreatmentInDb.InteractionId);

                if (pPHTreatmentInDb.IdentifiedPPHTreatments is not null)
                {
                    pPHTreatmentInDb.IdentifiedPPHTreatments.ToList().ForEach(x =>
                    {
                        x.IsDeleted = true;
                        context.IdentifiedPPHTreatmentRepository.Update(x);
                    });
                }

                if (pPHTreatmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                pPHTreatmentInDb.DateModified = DateTime.Now;
                pPHTreatmentInDb.IsDeleted = true;
                pPHTreatmentInDb.IsSynced = false;

                context.PPHTreatmentRepository.Update(pPHTreatmentInDb);
                await context.SaveChangesAsync();

                return Ok(pPHTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeletePPHTreatment", "PPHTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}