using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 26.12.2022
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// IdentifiedTBSymptom controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
  //  [Authorize]

    public class IdentifiedTBSymptomController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IdentifiedTBSymptomController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public IdentifiedTBSymptomController(IUnitOfWork context, ILogger<IdentifiedTBSymptomController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptom
        /// </summary>
        /// <param name="identifiedTBSymptom">IdentifiedTBSymptom object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIdentifiedTBSymptom)]
        public async Task<IActionResult> CreateIdentifiedTBSymptom(IdentifiedTBSymptom identifiedTBSymptom)
        {
            try
            {
                foreach (var item in identifiedTBSymptom.TBSymptomList)
                {
                    var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.LoadWithChildAsync<IdentifiedTBSymptom>(x => x.EncounterId == identifiedTBSymptom.EncounterId
                    && x.ClientId == identifiedTBSymptom.ClientId
                    && x.TBSymptomId == item
                    && x.IsDeleted == false);

                    if (identifiedTbSymptomInDb == null)
                    {
                        Interaction interaction = new Interaction();

                        Guid interactionID = Guid.NewGuid();

                        interaction.Oid = interactionID;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedTBSymptom, identifiedTBSymptom.EncounterType);
                        interaction.EncounterId = identifiedTBSymptom.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = identifiedTBSymptom.CreatedBy;
                        interaction.CreatedIn = identifiedTBSymptom.CreatedIn;
                        interaction.IsSynced = false;
                        interaction.IsDeleted = false;

                        context.InteractionRepository.Add(interaction);

                        identifiedTBSymptom.InteractionId = interactionID;
                        identifiedTBSymptom.ClientId = identifiedTBSymptom.ClientId;
                        identifiedTBSymptom.TBSymptomId = item;
                        identifiedTBSymptom.DateCreated = DateTime.Now;
                        identifiedTBSymptom.IsDeleted = false;
                        identifiedTBSymptom.IsSynced = false;

                        context.IdentifiedTBSymptomRepository.Add(identifiedTBSymptom);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadIdentifiedTBSymptomByKey", new { key = identifiedTBSymptom.InteractionId }, identifiedTBSymptom);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateIdentifiedTBSymptom", "IdentifiedTBSymptomController.cs", ex.Message, identifiedTBSymptom.CreatedIn, identifiedTBSymptom.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptoms
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBSymptoms)]
        public async Task<IActionResult> ReadIdentifiedTBSymptoms()
        {
            try
            {
                var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptoms();

                return Ok(identifiedTbSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBSymptoms", "IdentifiedTBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptom/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBSymptoms.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBSymptomByKey)]
        public async Task<IActionResult> ReadIdentifiedTBSymptomByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByKey(key);

                if (identifiedTbSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedTbSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBSymptomByKey", "IdentifiedTBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptom-by-encounterId/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBSymptomByEncounterId)]
        public async Task<IActionResult> ReadIdentifiedTBSymptomByEncounterId(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByEncounterId(encounterId);

                if (identifiedTbSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(identifiedTbSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBSymptomByEncounterId", "IdentifiedTBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-constitutional-symptom-by-encounterId/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBAndConstitutionalSymptomByEncounterId)]
        public async Task<IActionResult> ReadIdentifiedTBAndConstitutionalSymptomByEncounterId(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByEncounterId(encounterId);

                var constitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByEncounterId(encounterId);

                if (identifiedTbSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var tbAndConstituionalSymtomData = new { identifiedTbSymptom = identifiedTbSymptomInDb, constitutionalSymptom = constitutionalSymptomInDb };
                return Ok(tbAndConstituionalSymtomData);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBSymptomByEncounterId", "IdentifiedTBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBSymptom.</param>
        /// <param name="identifiedTBSymptom">IdentifiedTBSymptom to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedTBSymptom)]
        public async Task<IActionResult> UpdateIdentifiedTBSymptom(Guid key, IdentifiedTBSymptom identifiedTBSymptom)
        {
            try
            {
                if (key != identifiedTBSymptom.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = identifiedTBSymptom.ModifiedBy;
                interactionInDb.ModifiedIn = identifiedTBSymptom.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                identifiedTBSymptom.DateModified = DateTime.Now;
                identifiedTBSymptom.IsDeleted = false;
                identifiedTBSymptom.IsSynced = false;

                context.IdentifiedTBSymptomRepository.Update(identifiedTBSymptom);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedTBSymptom", "IdentifiedTBSymptomController.cs", ex.Message, identifiedTBSymptom.ModifiedIn, identifiedTBSymptom.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptom-by-clientId/{clientId}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIdentifiedTBSymptomByClientId)]
        public async Task<IActionResult> ReadIdentifiedTBSymptomByClientId(Guid clientId)
        {
            try
            {
                var identifiedConstitutionalSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByClient(clientId);

                return Ok(identifiedConstitutionalSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIdentifiedTBSymptomByClientId", "IdentifiedTBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.IdentifiedTBAndConstitutionalSymptom)]
        public async Task<IActionResult> IdentifiedTBAndConstitutionalSymptom(IdentifiedTBAndConstitutionalSymptom model)
        {
            try
            {
      
                if (model.IdentifiedTBSymptom.TBSymptomList?.Length > 0)
                {

                    foreach (var item in model.IdentifiedTBSymptom.TBSymptomList)
                    {
                        var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.LoadWithChildAsync<IdentifiedTBSymptom>(x => x.EncounterId == model.IdentifiedTBSymptom.EncounterId
                        && x.ClientId == model.IdentifiedTBSymptom.ClientId
                        && x.TBSymptomId == item
                        && x.IsDeleted == false);

                        if (identifiedTbSymptomInDb == null)
                        {
                            Interaction interaction = new Interaction();

                            Guid interactionID = Guid.NewGuid();

                            interaction.Oid = interactionID;
                            interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedTBSymptom, model.IdentifiedTBSymptom.EncounterType);
                            interaction.EncounterId = model.IdentifiedTBSymptom.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = model.IdentifiedTBSymptom.CreatedBy;
                            interaction.CreatedIn = model.IdentifiedTBSymptom.CreatedIn;
                            interaction.IsSynced = false;
                            interaction.IsDeleted = false;

                            context.InteractionRepository.Add(interaction);

                            model.IdentifiedTBSymptom.InteractionId = interactionID;
                            model.IdentifiedTBSymptom.ClientId = model.IdentifiedTBSymptom.ClientId;
                            model.IdentifiedTBSymptom.TBSymptomId = item;
                            model.IdentifiedTBSymptom.DateCreated = DateTime.Now;
                            model.IdentifiedTBSymptom.IsDeleted = false;
                            model.IdentifiedTBSymptom.IsSynced = false;

                            context.IdentifiedTBSymptomRepository.Add(model.IdentifiedTBSymptom);
                            await context.SaveChangesAsync();
                        }
                    }


                }

                if (model.ConstitutionalSymptom.ConstitutionalSymptomTypeList?.Length > 0)
                {

                    foreach (var item in model.ConstitutionalSymptom.ConstitutionalSymptomTypeList)
                    {
                        var constitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.LoadWithChildAsync<IdentifiedConstitutionalSymptom>(x => x.EncounterId == model.ConstitutionalSymptom.EncounterId
                        && x.ClientId == model.ConstitutionalSymptom.ClientId
                        && x.ConstitutionalSymptomTypeId == item
                        && x.IsDeleted == false);

                        if (constitutionalSymptomInDb == null)
                        {
                            Interaction interaction = new Interaction();

                            Guid interactionID = Guid.NewGuid();

                            interaction.Oid = interactionID;
                            interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedConstitutionalSymptom, model.ConstitutionalSymptom.EncounterType);
                            interaction.EncounterId = model.ConstitutionalSymptom.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = model.ConstitutionalSymptom.CreatedBy;
                            interaction.CreatedIn = model.ConstitutionalSymptom.CreatedIn;
                            interaction.IsSynced = false;
                            interaction.IsDeleted = false;

                            context.InteractionRepository.Add(interaction);

                            model.ConstitutionalSymptom.InteractionId = interactionID;
                            model.ConstitutionalSymptom.ClientId = model.ConstitutionalSymptom.ClientId;
                            model.ConstitutionalSymptom.EncounterId = model.ConstitutionalSymptom.EncounterId;
                            model.ConstitutionalSymptom.ConstitutionalSymptomTypeId = item;
                            model.ConstitutionalSymptom.DateCreated = DateTime.Now;
                            model.ConstitutionalSymptom.IsDeleted = false;
                            model.ConstitutionalSymptom.IsSynced = false;

                            context.IdentifiedConstitutionalSymptomRepository.Add(model.ConstitutionalSymptom);
                            await context.SaveChangesAsync();
                        }
                    }


                }



                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedTBSymptom", "IdentifiedTBSymptomController.cs", ex.Message, model.IdentifiedTBSymptom.ModifiedIn, model.IdentifiedTBSymptom.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/identified-tb-constitutional-symptom/{encounterId}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBSymptom.</param>
        /// <param name="identifiedTBSymptom">IdentifiedTBSymptom to be updated.</param>
        /// <param name="constitutionalSymptom">IdentifiedTBSymptom to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIdentifiedTBAndConstitutionalSymptom)]
        public async Task<IActionResult> UpdateIdentifiedTBAndConstitutionalSymptom(Guid encounterId, IdentifiedTBAndConstitutionalSymptom model)
        {
            try
            {
                if (encounterId == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (model.IdentifiedTBSymptom.TBSymptomList?.Length>0)
                {

                    var IdentifiedSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByEncounterId(encounterId);

                    if (IdentifiedSymptomInDb != null)
                    {
                        foreach(var item in IdentifiedSymptomInDb)
                        {
                            context.InteractionRepository.Delete(await context.InteractionRepository.GetInteractionByKey(item.InteractionId));
                            context.IdentifiedTBSymptomRepository.Delete(item);
                           await  context.SaveChangesAsync();
                        }

                    }

                    foreach (var item in model.IdentifiedTBSymptom.TBSymptomList)
                    {
                        var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.LoadWithChildAsync<IdentifiedTBSymptom>(x => x.EncounterId == model.IdentifiedTBSymptom.EncounterId
                        && x.ClientId == model.IdentifiedTBSymptom.ClientId
                        && x.TBSymptomId == item
                        && x.IsDeleted == false);

                        if (identifiedTbSymptomInDb == null)
                        {
                            Interaction interaction = new Interaction();

                            Guid interactionID = Guid.NewGuid();

                            interaction.Oid = interactionID;
                            interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedTBSymptom, model.IdentifiedTBSymptom.EncounterType);
                            interaction.EncounterId = model.IdentifiedTBSymptom.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = model.IdentifiedTBSymptom.CreatedBy;
                            interaction.CreatedIn = model.IdentifiedTBSymptom.CreatedIn;
                            interaction.IsSynced = false;
                            interaction.IsDeleted = false;

                            context.InteractionRepository.Add(interaction);

                            model.IdentifiedTBSymptom.InteractionId = interactionID;
                            model.IdentifiedTBSymptom.ClientId = model.IdentifiedTBSymptom.ClientId;
                            model.IdentifiedTBSymptom.TBSymptomId = item;
                            model.IdentifiedTBSymptom.DateCreated = DateTime.Now;
                            model.IdentifiedTBSymptom.IsDeleted = false;
                            model.IdentifiedTBSymptom.IsSynced = false;

                            context.IdentifiedTBSymptomRepository.Add(model.IdentifiedTBSymptom);
                            await context.SaveChangesAsync();
                        }
                    }


                }

                if (model.ConstitutionalSymptom.ConstitutionalSymptomTypeList?.Length>0)
                {

                    var IdentifiedConstitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.GetIdentifiedConstitutionalSymptomsByEncounterId(encounterId);

                    if (IdentifiedConstitutionalSymptomInDb != null)
                    {
                        foreach (var item in IdentifiedConstitutionalSymptomInDb)
                        {
                            context.InteractionRepository.Delete(await context.InteractionRepository.GetInteractionByKey(item.InteractionId));
                            context.IdentifiedConstitutionalSymptomRepository.Delete(item);
                            await context.SaveChangesAsync();
                        }

                    }

                    foreach (var item in model.ConstitutionalSymptom.ConstitutionalSymptomTypeList)
                    {
                        var constitutionalSymptomInDb = await context.IdentifiedConstitutionalSymptomRepository.LoadWithChildAsync<IdentifiedConstitutionalSymptom>(x => x.EncounterId == model.ConstitutionalSymptom.EncounterId
                        && x.ClientId == model.ConstitutionalSymptom.ClientId
                        && x.ConstitutionalSymptomTypeId == item
                        && x.IsDeleted == false);

                        if (constitutionalSymptomInDb == null)
                        {
                            Interaction interaction = new Interaction();

                            Guid interactionID = Guid.NewGuid();

                            interaction.Oid = interactionID;
                            interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.IdentifiedConstitutionalSymptom, model.ConstitutionalSymptom.EncounterType);
                            interaction.EncounterId = model.ConstitutionalSymptom.EncounterId;
                            interaction.DateCreated = DateTime.Now;
                            interaction.CreatedBy = model.ConstitutionalSymptom.CreatedBy;
                            interaction.CreatedIn = model.ConstitutionalSymptom.CreatedIn;
                            interaction.IsSynced = false;
                            interaction.IsDeleted = false;

                            context.InteractionRepository.Add(interaction);

                            model.ConstitutionalSymptom.InteractionId = interactionID;
                            model.ConstitutionalSymptom.ClientId = model.ConstitutionalSymptom.ClientId;
                            model.ConstitutionalSymptom.EncounterId = model.ConstitutionalSymptom.EncounterId;
                            model.ConstitutionalSymptom.ConstitutionalSymptomTypeId = item;
                            model.ConstitutionalSymptom.DateCreated = DateTime.Now;
                            model.ConstitutionalSymptom.IsDeleted = false;
                            model.ConstitutionalSymptom.IsSynced = false;

                            context.IdentifiedConstitutionalSymptomRepository.Add(model.ConstitutionalSymptom);
                            await context.SaveChangesAsync();
                        }
                    }


                }



                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateIdentifiedTBSymptom", "IdentifiedTBSymptomController.cs", ex.Message, model.IdentifiedTBSymptom.ModifiedIn, model.IdentifiedTBSymptom.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptom/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBSymptom.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIdentifiedTBSymptom)]
        public async Task<IActionResult> DeleteIdentifiedTBSymptom(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByKey(key);

                if (identifiedTbSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                identifiedTbSymptomInDb.DateModified = DateTime.Now;
                identifiedTbSymptomInDb.IsDeleted = true;
                identifiedTbSymptomInDb.IsSynced = false;

                context.IdentifiedTBSymptomRepository.Update(identifiedTbSymptomInDb);
                await context.SaveChangesAsync();

                return Ok(identifiedTbSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIdentifiedTBSymptom", "IdentifiedTBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/identified-tb-symptom/remove/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveIdentifiedTBSymptom)]
        public async Task<IActionResult> RemoveIdentifiedTBSymptom(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var identifiedTbSymptomInDb = await context.IdentifiedTBSymptomRepository.GetIdentifiedTBSymptomByEncounterId(key);

                if (identifiedTbSymptomInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in identifiedTbSymptomInDb)
                {
                    context.IdentifiedTBSymptomRepository.Delete(item);
                }

                await context.SaveChangesAsync();

                return Ok(identifiedTbSymptomInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveIdentifiedTBSymptom", "IdentifiedTBSymptomController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}