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
 * Last modified : 05.02.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// SystemReview controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class SystemReviewController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<SystemReviewController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public SystemReviewController(IUnitOfWork context, ILogger<SystemReviewController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region SystemReview
        /// <summary>
        /// URL: sc-api/system-review
        /// </summary>
        /// <param name="systemReviewDto">SystemReview object</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSystemReview)]
        public async Task<IActionResult> CreateSystemReview(SystemReviewDto systemReviewDto)
        {
            SystemReview systemReviewLast = new SystemReview();

            try
            {
                List<SystemReview> listSystemReview = new List<SystemReview>();

                foreach (var item in systemReviewDto.systemReviews)
                {
                    Guid interactionId = Guid.NewGuid();

                    Interaction interaction = new Interaction();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.SystemReview, systemReviewDto.EncounterType);
                    interaction.EncounterId = systemReviewDto.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = systemReviewDto.CreatedBy;
                    interaction.CreatedIn = systemReviewDto.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    context.InteractionRepository.Add(interaction);
                    await context.SaveChangesAsync();

                    item.InteractionId = interactionId;
                    item.ClientId = systemReviewDto.ClientId;
                    item.PhysicalSystemId = item.PhysicalSystemId;
                    item.Note = item.Note;
                    item.EncounterId = systemReviewDto.EncounterId;
                    item.EncounterType = systemReviewDto.EncounterType;
                    item.CreatedIn = systemReviewDto.CreatedIn;
                    item.CreatedBy = systemReviewDto.CreatedBy;
                    item.DateCreated = DateTime.Now;
                    item.IsDeleted = false;
                    item.IsSynced = false;

                    context.SystemReviewRepository.Add(item);
                    await context.SaveChangesAsync();

                    listSystemReview.Add(item);
                }

                systemReviewLast.SystemReviews = listSystemReview;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateSystemReview", "SystemReviewController.cs", ex.Message, systemReviewDto.CreatedIn, systemReviewDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

            return CreatedAtAction("ReadSystemReviewByKey", new { key = systemReviewLast.SystemReviews.Select(x => x.InteractionId).FirstOrDefault() }, systemReviewLast);
        }

        /// <summary>
        /// URL: sc-api/system-reviews
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemReviews)]
        public async Task<IActionResult> ReadSystemReviews()
        {
            try
            {
                var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviews();

                return Ok(systemReviewInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemReviews", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-review/by-client/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemReviewByClient)]
        public async Task<IActionResult> ReadSystemReviewsByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByClient(clientId);
                    systemReviewInDb = systemReviewInDb.OrderByDescending(x => x.DateCreated);
                    return Ok(systemReviewInDb);
                }
                else
                {
                    var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<SystemReview> systemReviewWithPaggingDto = new PagedResultDto<SystemReview>()
                    {
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.SystemReviewRepository.GetSystemReviewByClientTotalCount(clientId, encounterType),
                        Data = systemReviewInDb.ToList()
                    };
                    return Ok(systemReviewWithPaggingDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemReviewsByClient", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-review/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemView.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemReviewByKey)]
        public async Task<IActionResult> ReadSystemReviewByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(key);

                if (systemReviewInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(systemReviewInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemReviewByKey", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-review-by-encounter/key/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemReviewByOPDVisit)]
        public async Task<IActionResult> ReadSystemReviewByOPDVisit(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(encounterId);

                if (systemReviewInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(systemReviewInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemReviewByOPDVisit", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-review/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemReview.</param>
        /// <param name="systemReview">SystemReview to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSystemReview)]
        public async Task<IActionResult> UpdateSystemReview(Guid key, SystemReviewDto systemReviewDto)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = systemReviewDto.ModifiedBy;
                interactionInDb.ModifiedIn = systemReviewDto.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                if (key != systemReviewDto.systemReviews.Select(x => x.EncounterId).FirstOrDefault())
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                foreach (var item in systemReviewDto.systemReviews)
                {
                    var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(item.InteractionId);

                    systemReviewInDb.Note = item.Note;
                    systemReviewInDb.PhysicalSystemId = item.PhysicalSystemId;

                    systemReviewInDb.DateModified = DateTime.Now;
                    systemReviewInDb.IsSynced = false;
                    systemReviewInDb.IsDeleted = false;
                    systemReviewInDb.ModifiedIn = item.ModifiedIn;
                    systemReviewInDb.ModifiedBy = item.ModifiedBy;

                    context.SystemReviewRepository.Update(systemReviewInDb);
                }
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSystemReview", "SystemReviewController.cs", ex.Message, systemReviewDto.ModifiedIn, systemReviewDto.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        [HttpPut]
        [Route(RouteConstants.UpdateSystemReviewBySingleOjbect)]
        public async Task<IActionResult> UpdateSystemReviewBySingleOjbect(Guid key, SystemReview systemReview)
        {
            try
            {
                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = systemReview.ModifiedBy;
                interactionInDb.ModifiedIn = systemReview.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

               

              
                    var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(systemReview.InteractionId);

                    systemReviewInDb.Note = systemReview.Note;
                    systemReviewInDb.PhysicalSystemId = systemReview.PhysicalSystemId;

                    systemReviewInDb.DateModified = DateTime.Now;
                    systemReviewInDb.IsSynced = false;
                    systemReviewInDb.IsDeleted = false;
                    systemReviewInDb.ModifiedIn = systemReview.ModifiedIn;
                    systemReviewInDb.ModifiedBy = systemReview.ModifiedBy;

                    context.SystemReviewRepository.Update(systemReviewInDb);
                
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateSystemReview", "SystemReviewController.cs", ex.Message, systemReview.ModifiedIn, systemReview.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/system-review/{key}
        /// </summary>
        /// <param name="key">Primary key of the table SystemReview.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSystemReview)]
        public async Task<IActionResult> DeleteSystemReview(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(key);

                if (systemReviewInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                systemReviewInDb.DateModified = DateTime.Now;
                systemReviewInDb.IsDeleted = true;
                systemReviewInDb.IsSynced = false;

                context.SystemReviewRepository.Update(systemReviewInDb);
                await context.SaveChangesAsync();

                return Ok(systemReviewInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSystemReview", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// sc-api/system-review/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveSystemReview)]
        public async Task<IActionResult> RemoveSystemReview(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemInDb = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(encounterId);

                if (systemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in systemInDb.ToList())
                {
                    context.SystemReviewRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(systemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveSystemReview", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region PEPSystemReview
        ///// <summary>
        ///// URL: sc-api/pep-system-review
        ///// </summary>
        ///// <param name="systemReviewDto">PEPSystemReview object</param>
        ///// <returns>Http status code: CreatedAt.</returns>
        //[HttpPost]
        //[Route(RouteConstants.CreatePEPSystemReview)]
        //public async Task<IActionResult> CreatePEPSystemReview(SystemReviewDto systemReviewDto)
        //{
        //    SystemReview systemReviewLast = new SystemReview();

        //    try
        //    {
        //        List<SystemReview> listSystemReview = new List<SystemReview>();

        //        foreach (var item in systemReviewDto.systemReviews)
        //        {
        //            Guid interactionID = Guid.NewGuid();

        //            Interaction interaction = new Interaction();

        //            interaction.OID = interactionID;
        //            interaction.ServiceCode = 5;
        //            interaction.EncounterID = systemReviewDto.OPDVisitID;
        //            interaction.DateCreated = DateTime.Now;
        //            interaction.CreatedBy = Guid.Empty;

        //            await context.SaveChangesAsync();
        //            context.InteractionRepository.Add(interaction);

        //            item.InteractionID = interactionID;
        //            item.ClientID = systemReviewDto.ClientID;
        //            item.PhysicalSystemID = item.PhysicalSystemID;
        //            item.Note = item.Note;
        //            item.EncounterID = systemReviewDto.OPDVisitID;
        //            item.DateCreated = DateTime.Now;
        //            item.IsDeleted = false;
        //            item.IsSynced = false;

        //            context.SystemReviewRepository.Add(item);
        //            await context.SaveChangesAsync();

        //            listSystemReview.Add(item);
        //        }

        //        systemReviewLast.SystemReviews = listSystemReview;
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }

        //    return CreatedAtAction("ReadPEPSystemReviewByKey", new { key = systemReviewLast.SystemReviews.Select(x => x.InteractionID).FirstOrDefault() }, systemReviewLast);
        //}

        ///// <summary>
        ///// URL: sc-api/pep-system-reviews
        ///// </summary>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPEPSystemReviews)]
        //public async Task<IActionResult> ReadPEPSystemReviews()
        //{
        //    try
        //    {
        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviews();

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/pep-system-review/byClient/{ClientID}
        ///// </summary>
        ///// <param name="ClientID">Primary key of the table Clients.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPEPSystemReviewByClient)]
        //public async Task<IActionResult> ReadPEPSystemReviewsByClient(Guid ClientID)
        //{
        //    try
        //    {
        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByClient(ClientID);

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/pep-system-review/key/{key}
        ///// </summary>
        ///// <param name="key">Primary key of the table SystemView.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPEPSystemReviewByKey)]
        //public async Task<IActionResult> ReadPEPSystemReviewByKey(Guid key)
        //{
        //    try
        //    {
        //        if (key == Guid.Empty)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(key);

        //        if (systemReviewInDb == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/pep-system-review-byOPDVisitID/key/{OPDVisitID}
        ///// </summary>
        ///// <param name="OPDVisitID">Primary key of the table OPDVisits.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPEPSystemReviewByEncounter)]
        //public async Task<IActionResult> ReadPEPSystemReviewByEncounter(Guid EncounterID)
        //{
        //    try
        //    {
        //        if (EncounterID == Guid.Empty)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(EncounterID);

        //        if (systemReviewInDb == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/pep-system-review/{key}
        ///// </summary>
        ///// <param name="key">Primary key of the table SystemReview.</param>
        ///// <param name="systemReview">SystemReview to be updated.</param>
        ///// <returns>Http Status Code: NoContent.</returns>
        //[HttpPut]
        //[Route(RouteConstants.UpdatePEPSystemReview)]
        //public async Task<IActionResult> UpdatePEPSystemReview(Guid key, SystemReviewDto systemReviewDto)
        //{
        //    try
        //    {
        //        if (key != systemReviewDto.systemReviews.Select(x => x.EncounterID).FirstOrDefault())
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);
        //        //    var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(systemReviewDto.systemReviews.Select(x=>x.OPDVisitID).FirstOrDefault());

        //        foreach (var item in systemReviewDto.systemReviews)
        //        {
        //            var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(item.InteractionID);
        //            systemReviewInDb.Note = item.Note;
        //            systemReviewInDb.PhysicalSystemID = item.PhysicalSystemID;
        //            context.SystemReviewRepository.Update(systemReviewInDb);

        //        }
        //        await context.SaveChangesAsync();
        //        //if (key != systemReview.InteractionID)
        //        //    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

        //        //systemReview.DateModified = DateTime.Now;
        //        //systemReview.IsSynced = false;

        //        //context.SystemReviewRepository.Update(systemReview);
        //        //await context.SaveChangesAsync();

        //        return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/pep-system-review/{key}
        ///// </summary>
        ///// <param name="key">Primary key of the table SystemReview.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpDelete]
        //[Route(RouteConstants.DeletePEPSystemReview)]
        //public async Task<IActionResult> DeletePEPSystemReview(Guid key)
        //{
        //    try
        //    {
        //        if (key == Guid.Empty)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(key);

        //        if (systemReviewInDb == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        systemReviewInDb.DateModified = DateTime.Now;
        //        systemReviewInDb.IsDeleted = true;
        //        systemReviewInDb.IsSynced = false;

        //        context.SystemReviewRepository.Update(systemReviewInDb);
        //        await context.SaveChangesAsync();

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        /// <summary>
        /// sc-api/pep-system-review/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePEPSystemReview)]
        public async Task<IActionResult> RemovePEPSystemReview(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemInDb = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(encounterId);

                if (systemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in systemInDb.Where(e => e.EncounterType == Enums.EncounterType.PEP))
                {
                    context.SystemReviewRepository.Delete(item);
                    await context.SaveChangesAsync();
                }
                return Ok(systemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePEPSystemReview", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion

        #region PrEPSystemReview
        ///// <summary>
        ///// URL: sc-api/prep-system-review
        ///// </summary>
        ///// <param name="systemReviewDto">PrEPSystemReview object</param>
        ///// <returns>Http status code: CreatedAt.</returns>
        //[HttpPost]
        //[Route(RouteConstants.CreatePrEPSystemReview)]
        //public async Task<IActionResult> CreatePrEPSystemReview(SystemReviewDto systemReviewDto)
        //{
        //    SystemReview systemReviewLast = new SystemReview();

        //    try
        //    {
        //        List<SystemReview> listSystemReview = new List<SystemReview>();

        //        foreach (var item in systemReviewDto.systemReviews)
        //        {
        //            Guid interactionID = Guid.NewGuid();

        //            Interaction interaction = new Interaction();

        //            interaction.OID = interactionID;
        //            interaction.ServiceCode = 5;
        //            interaction.EncounterID = systemReviewDto.OPDVisitID;
        //            interaction.DateCreated = DateTime.Now;
        //            interaction.CreatedBy = Guid.Empty;

        //            await context.SaveChangesAsync();
        //            context.InteractionRepository.Add(interaction);

        //            item.InteractionID = interactionID;
        //            item.ClientID = systemReviewDto.ClientID;
        //            item.PhysicalSystemID = item.PhysicalSystemID;
        //            item.Note = item.Note;
        //            item.EncounterID = systemReviewDto.OPDVisitID;
        //            item.DateCreated = DateTime.Now;
        //            item.IsDeleted = false;
        //            item.IsSynced = false;

        //            context.SystemReviewRepository.Add(item);
        //            await context.SaveChangesAsync();

        //            listSystemReview.Add(item);
        //        }

        //        systemReviewLast.SystemReviews = listSystemReview;
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }

        //    return CreatedAtAction("ReadPrEPSystemReviewByKey", new { key = systemReviewLast.SystemReviews.Select(x => x.InteractionID).FirstOrDefault() }, systemReviewLast);
        //}

        ///// <summary>
        ///// URL: sc-api/prep-system-reviews
        ///// </summary>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPrEPSystemReviews)]
        //public async Task<IActionResult> ReadPrEPSystemReviews()
        //{
        //    try
        //    {
        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviews();

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/prep-system-review/byClient/{ClientID}
        ///// </summary>
        ///// <param name="ClientID">Primary key of the table Clients.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPrEPSystemReviewByClient)]
        //public async Task<IActionResult> ReadPrEPSystemReviewByClient(Guid ClientID)
        //{
        //    try
        //    {
        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByClient(ClientID);

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/prep-system-review/key/{key}
        ///// </summary>
        ///// <param name="key">Primary key of the table SystemView.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPrEPSystemReviewByKey)]
        //public async Task<IActionResult> ReadPrEPSystemReviewByKey(Guid key)
        //{
        //    try
        //    {
        //        if (key == Guid.Empty)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(key);

        //        if (systemReviewInDb == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/prep-system-review-byEncounterID/key/{EncounterID}
        ///// </summary>
        ///// <param name="EncounterID">Primary key of the table OPDVisits.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpGet]
        //[Route(RouteConstants.ReadPrEPSystemReviewByEncounter)]
        //public async Task<IActionResult> ReadPrEPSystemReviewByEncounter(Guid EncounterID)
        //{
        //    try
        //    {
        //        if (EncounterID == Guid.Empty)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(EncounterID);

        //        if (systemReviewInDb == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/prep-system-review/{key}
        ///// </summary>
        ///// <param name="key">Primary key of the table SystemReview.</param>
        ///// <param name="systemReviewDto">SystemReview to be updated.</param>
        ///// <returns>Http Status Code: NoContent.</returns>
        //[HttpPut]
        //[Route(RouteConstants.UpdatePrEPSystemReview)]
        //public async Task<IActionResult> UpdatePrEPSystemReview(Guid key, SystemReviewDto systemReviewDto)
        //{
        //    try
        //    {
        //        if (key != systemReviewDto.systemReviews.Select(x => x.EncounterID).FirstOrDefault())
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

        //        foreach (var item in systemReviewDto.systemReviews)
        //        {
        //            var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(item.InteractionID);
        //            systemReviewInDb.Note = item.Note;
        //            systemReviewInDb.PhysicalSystemID = item.PhysicalSystemID;
        //            context.SystemReviewRepository.Update(systemReviewInDb);

        //        }
        //        await context.SaveChangesAsync();

        //        return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        ///// <summary>
        ///// URL: sc-api/prep-system-review/{key}
        ///// </summary>
        ///// <param name="key">Primary key of the table SystemReview.</param>
        ///// <returns>Http status code: Ok.</returns>
        //[HttpDelete]
        //[Route(RouteConstants.DeletePrEPSystemReview)]
        //public async Task<IActionResult> DeletePrEPSystemReview(Guid key)
        //{
        //    try
        //    {
        //        if (key == Guid.Empty)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var systemReviewInDb = await context.SystemReviewRepository.GetSystemReviewByKey(key);

        //        if (systemReviewInDb == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        systemReviewInDb.DateModified = DateTime.Now;
        //        systemReviewInDb.IsDeleted = true;
        //        systemReviewInDb.IsSynced = false;

        //        context.SystemReviewRepository.Update(systemReviewInDb);
        //        await context.SaveChangesAsync();

        //        return Ok(systemReviewInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        /// <summary>
        /// sc-api/prep-system-review/remove/{encounterId}
        /// </summary>
        /// <param name="encounterId">Primary key of the table OPDVisits.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.RemovePrEPSystemReview)]
        public async Task<IActionResult> RemovePrEPSystemReview(Guid encounterId)
        {
            try
            {
                if (encounterId == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemInDb = await context.SystemReviewRepository.GetSystemReviewByOPDVisit(encounterId);

                if (systemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                foreach (var item in systemInDb.Where(e => e.EncounterType == Enums.EncounterType.PrEP))
                {
                    context.SystemReviewRepository.Delete(item);
                    await context.SaveChangesAsync();
                }


                foreach (var item in systemInDb.Where(e => e.EncounterType == Enums.EncounterType.ANCService))
                {
                    context.SystemReviewRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                return Ok(systemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemovePrEPSystemReview", "SystemReviewController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        #endregion
    }
}