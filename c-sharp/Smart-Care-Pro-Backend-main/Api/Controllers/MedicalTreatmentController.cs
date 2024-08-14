using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// MedicalTreatment Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class MedicalTreatmentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<MedicalTreatmentController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public MedicalTreatmentController(IUnitOfWork context, ILogger<MedicalTreatmentController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/medical-treatment
        /// </summary>
        /// <param name="medicalTreatment">MedicalTreatment object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMedicalTreatment)]
        public async Task<IActionResult> CreateMedicalTreatment(MedicalTreatment medicalTreatment)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.MedicalTreatment, medicalTreatment.EncounterType);
                interaction.EncounterId = medicalTreatment.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = medicalTreatment.CreatedBy;
                interaction.CreatedIn = medicalTreatment.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                var motherDelivery = await context.MotherDeliverySummaryRepository.FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == medicalTreatment.DeliveryId);

                if (motherDelivery == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                if (medicalTreatment.TreatmentsList != null)
                {
                    foreach (var item in medicalTreatment.TreatmentsList)
                    {
                        MedicalTreatment medicalTreatmentItem = new MedicalTreatment();
                        medicalTreatmentItem.CreatedBy = medicalTreatment.CreatedBy;
                        medicalTreatmentItem.CreatedIn = medicalTreatment.CreatedIn;
                        medicalTreatmentItem.DeliveryId = motherDelivery.InteractionId;
                        medicalTreatmentItem.EncounterId = medicalTreatment.EncounterId;
                        medicalTreatmentItem.InteractionId = Guid.NewGuid();
                        medicalTreatmentItem.Treatments = item;
                        medicalTreatmentItem.DateCreated = DateTime.Now;
                        medicalTreatmentItem.IsDeleted = false;
                        medicalTreatmentItem.IsSynced = false;

                        context.MedicalTreatmentRepository.Add(medicalTreatmentItem);
                    }
                }
                else
                {
                    medicalTreatment.InteractionId = interactionId;
                    medicalTreatment.EncounterId = medicalTreatment.EncounterId;
                    medicalTreatment.DateCreated = DateTime.Now;
                    medicalTreatment.IsDeleted = false;
                    medicalTreatment.IsSynced = false;

                    context.MedicalTreatmentRepository.Add(medicalTreatment);
                }
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadMedicalTreatmentByKey", new { key = medicalTreatment.InteractionId }, medicalTreatment);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateMedicalTreatment", "MedicalTreatmentController.cs", ex.Message, medicalTreatment.CreatedIn, medicalTreatment.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-current-delivery-complication/byDelivery/{DeliveryId}
        /// </summary>
        /// <param name="deliveryId">Primary key of the table PPHTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalTreatmentByDelivery)]
        public async Task<IActionResult> ReadMedicalTreatmentByDelivery(Guid deliveryId)
        {
            try
            {
                var medicalTreatmentDb = await context.MedicalTreatmentRepository.GetMedicalTreatmentByDelivery(deliveryId);
                medicalTreatmentDb = medicalTreatmentDb.OrderByDescending(x => x.DateCreated);

                return Ok(medicalTreatmentDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalTreatmentByDelivery", "MedicalTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-treatments
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalTreatments)]
        public async Task<IActionResult> ReadMedicalTreatments()
        {
            try
            {
                var medicalTreatmentInDb = await context.MedicalTreatmentRepository.GetMedicalTreatments();
                medicalTreatmentInDb = medicalTreatmentInDb.OrderByDescending(x => x.DateCreated);

                return Ok(medicalTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalTreatments", "MedicalTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-treatment/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalTreatments.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMedicalTreatmentByKey)]
        public async Task<IActionResult> ReadMedicalTreatmentByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicalTreatmentInDb = await context.MedicalTreatmentRepository.GetMedicalTreatmentByKey(key);

                if (medicalTreatmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(medicalTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMedicalTreatmentByKey", "MedicalTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-treatment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalTreatments.</param>
        /// <param name="medicalTreatment">MedicalTreatment to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMedicalTreatment)]
        public async Task<IActionResult> UpdateMedicalTreatment(Guid key, MedicalTreatment medicalTreatment)
        {
            try
            {
                if (key != medicalTreatment.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                medicalTreatment.DateModified = DateTime.Now;
                medicalTreatment.IsDeleted = false;
                medicalTreatment.IsSynced = false;

                context.MedicalTreatmentRepository.Update(medicalTreatment);

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateMedicalTreatment", "MedicalTreatmentController.cs", ex.Message, medicalTreatment.ModifiedIn, medicalTreatment.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/medical-treatment/{key}
        /// </summary>
        /// <param name="key">Primary key of the table MedicalTreatments.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteMedicalTreatment)]
        public async Task<IActionResult> DeleteMedicalTreatment(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var medicalTreatmentInDb = await context.MedicalTreatmentRepository.GetMedicalTreatmentByEncounter(key);

                if (medicalTreatmentInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                medicalTreatmentInDb.ToList().ForEach(x =>
                {
                    x.IsDeleted = true;
                    x.IsSynced = false;

                    context.MedicalTreatmentRepository.Update(x);
                });

                await context.SaveChangesAsync();

                return Ok(medicalTreatmentInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteMedicalTreatment", "MedicalTreatmentController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}