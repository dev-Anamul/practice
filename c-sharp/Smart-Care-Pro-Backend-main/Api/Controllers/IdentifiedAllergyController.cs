using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Bella
 * Date created  : 25.12.2022
 * Modified by   : Bella
 * Last modified : 26.01.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedAllergy controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class IdentifiedAllergyController : ControllerBase
    {
        private IdentifiedAllergy identifiedAllergy;

        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedAllergyController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedAllergyController(IUnitOfWork context, ILogger<IdentifiedAllergyController> logger)
        {
            this.context = context;

            identifiedAllergy = new IdentifiedAllergy();
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-allergy
        /// </summary>
        /// <param name="identifiedAllergies">IdentifiedAllergy object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedAllergy)]
        public async Task<IActionResult> CreateIdentifiedAllergy(IdentifiedAllergy identifiedAllergies)
        {
            IdentifiedAllergy identifiedAllergyList = new IdentifiedAllergy();

            try
            {
                List<IdentifiedAllergy> listIdentifiedAllergies = new List<IdentifiedAllergy>();

                foreach (var item in identifiedAllergies.IdentifiedAllergies)
                {
                    var identifiedAllergy = await context.IdentifiedAllergyRepository.LoadWithChildAsync<IdentifiedAllergy>(x => x.EncounterId == identifiedAllergies.EncounterId && x.ClientId == identifiedAllergies.ClientId
                    && x.AllergyId == item.AllergyId
                    && x.AllergicDrugId == item.AllergicDrugId
                    && x.Severity == item.Severity && x.IsDeleted == false
                    );

                    if (identifiedAllergy == null)
                    {
                        Guid interactionId = Guid.NewGuid();

                        Interaction interaction = new Interaction();

                        interaction.Oid = interactionId;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedAllergy, identifiedAllergies.EncounterType);
                        interaction.EncounterId = identifiedAllergies.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = identifiedAllergies.CreatedBy;
                        interaction.CreatedIn = identifiedAllergies.CreatedIn;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        context.InteractionRepository.Add(interaction);

                        identifiedAllergies.InteractionId = interactionId;
                        identifiedAllergies.ClientId = identifiedAllergies.ClientId;
                        identifiedAllergies.AllergicDrugId = item.AllergicDrugId;
                        identifiedAllergies.Severity = item.Severity;
                        identifiedAllergies.AllergyId = item.AllergyId;
                        identifiedAllergies.DateCreated = DateTime.Now;
                        identifiedAllergies.CreatedBy = identifiedAllergies.CreatedBy;
                        identifiedAllergies.CreatedIn = identifiedAllergies.CreatedIn;
                        identifiedAllergies.IsDeleted = false;
                        identifiedAllergies.IsSynced = false;

                        context.IdentifiedAllergyRepository.Add(identifiedAllergies);
                        await context.SaveChangesAsync();

                        listIdentifiedAllergies.Add(identifiedAllergies);
                    }
                    else
                    {
                        listIdentifiedAllergies.Add(identifiedAllergy);
                    }
                }

                identifiedAllergyList.IdentifiedAllergies = listIdentifiedAllergies;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedAllergy", "IdentifiedAllergyController.cs", ex.Message, identifiedAllergies.CreatedIn, identifiedAllergies.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

            return CreatedAtAction("ReadIdentifiedAllergyByKey", new { key = identifiedAllergyList.IdentifiedAllergies.Select(x => x.InteractionId).FirstOrDefault() }, identifiedAllergyList);
        }

        /// <summary>
        /// URL: sc-api/identified-allergies
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedAllergies)]
        public async Task<IActionResult> ReadIdentifiedAllergies()
        {
            try
            {
                var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergies();

                return Ok(identifiedAllergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedAllergies", "IdentifiedAllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-allergy/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedAllergy.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedAllergyByKey)]
        public async Task<IActionResult> ReadIdentifiedAllergyByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByKey(key);

                if (identifiedAllergyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedAllergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedAllergyByKey", "IdentifiedAllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-allergy/clientId/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedAllergyByClient)]
        public async Task<IActionResult> ReadIdentifiedAllergyByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                   // var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByClient(clientId);
                    var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByClientLast24Hours(clientId);

                    return Ok(identifiedAllergyInDb);
                }
                else
                {
                    var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<IdentifiedAllergy> identifiedAllergyWithPaggingDto = new PagedResultDto<IdentifiedAllergy>()
                    {
                        Data = identifiedAllergyInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.IdentifiedAllergyRepository.GetIdentifiedAllergyByClientTotalCount(clientId, encounterType)
                    };
                    return Ok(identifiedAllergyWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedAllergyByClientID", "IdentifiedAllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-allergy/encounterId/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedAllergyByEncounterId)]
        public async Task<IActionResult> ReadIdentifiedAllergyByEncounterId(Guid encounterId)
        {
            try
            {
                var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByEncounterId(encounterId);

                return Ok(identifiedAllergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedAllergyByEncounterId", "IdentifiedAllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-allergy/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedAllergy.</param>
        /// <param name="allergyList">IdentifiedAllergy to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedAllergy)]
        public async Task<IActionResult> UpdateIdentifiedAllergy(Guid key, IdentifiedAllergy allergy)
        {
            try
            {
                if (key != allergy.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

               
                    var allergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByKey(key);

                    allergyInDb.AllergicDrugId = allergy.AllergicDrugId;
                    allergyInDb.AllergyId = allergy.AllergyId;
                    allergyInDb.Severity = allergy.Severity;
                    allergyInDb.DateModified = DateTime.Now;

                    context.IdentifiedAllergyRepository.Update(allergyInDb);
                

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedAllergy", "IdentifiedAllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-allergy/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedAllergy.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedAllergy)]
        public async Task<IActionResult> DeleteIdentifiedAllergy(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByKey(key);

                if (identifiedAllergyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedAllergyInDb.DateModified = DateTime.Now;
                identifiedAllergyInDb.IsDeleted = true;
                identifiedAllergyInDb.IsSynced = false;

                context.IdentifiedAllergyRepository.Update(identifiedAllergyInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedAllergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedAllergy", "IdentifiedAllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-allergy/remove/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveIdentifiedAllergy)]
        public async Task<IActionResult> RemoveIdentifiedAllergy(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedAllergyInDb = await context.IdentifiedAllergyRepository.GetIdentifiedAllergyByEncounterId(key);

                if (identifiedAllergyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in identifiedAllergyInDb)
                {
                    context.IdentifiedAllergyRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(identifiedAllergyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveIdentifiedAllergy", "IdentifiedAllergyController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}