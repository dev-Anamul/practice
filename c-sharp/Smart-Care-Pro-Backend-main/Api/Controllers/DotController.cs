using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Bella
 * Date created  : 07.04.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class DotController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DotController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the DotController.</param>
        public DotController(IUnitOfWork context, ILogger<DotController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/dot
        /// </summary>
        /// <param name="dot">Dot object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDot)]
        public async Task<IActionResult> CreateDot(Dot dot)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Dot, dot.EncounterType);
                interaction.EncounterId = dot.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = dot.CreatedBy;
                interaction.CreatedIn = dot.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                dot.InteractionId = interactionId;
                dot.DateCreated = DateTime.Now;
                dot.IsDeleted = false;
                dot.IsSynced = false;
                dot.DotEndDate = dot.DotStartDate.AddMonths(6);

                context.DotRepository.Add(dot);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDotByKey", new { key = dot.InteractionId }, dot);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDot", "DotController.cs", ex.Message, dot.CreatedIn, dot.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dotcalender
        /// </summary>
        /// <param name="dotCalendar"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.CreateDotCalender)]
        public async Task<IActionResult> CreateDotCalender(DotCalendar dotCalendar)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.DotCalendar, dotCalendar.EncounterType);
                interaction.EncounterId = dotCalendar.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = dotCalendar.CreatedBy;
                interaction.CreatedIn = dotCalendar.CreatedIn;
                interaction.IsDeleted = false;
                interaction.IsSynced = false;

                context.InteractionRepository.Add(interaction);

                dotCalendar.InteractionId = interactionId;
                dotCalendar.DateCreated = DateTime.Now;
                dotCalendar.IsDeleted = false;
                dotCalendar.IsSynced = false;

                context.DotCalenderRepository.Add(dotCalendar);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDotByKey", new { key = dotCalendar.InteractionId }, dotCalendar);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateDotCalender", "DotController.cs", ex.Message, dotCalendar.CreatedIn, dotCalendar.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/AddMonth
        /// </summary>
        /// <param name="dot"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.AddMonthDotCalender)]
        public async Task<IActionResult> AddMonthDotCalender(Dot dot)
        {
            try
            {
                if (dot.InteractionId == Guid.Empty)
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);

                var dots = await context.DotRepository.FirstOrDefaultAsync(x => x.InteractionId == dot.InteractionId);

                if (dots.DotEndDate == null)
                    dots.DotEndDate = dots.DotStartDate.AddMonths(6 + 1);
                else
                    dots.DotEndDate = dots.DotEndDate?.AddMonths(1);

                dots.IsSynced = false;
                dots.IsSynced = false;

                context.DotRepository.Update(dots);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDotByKey", new { key = dot.InteractionId }, dot);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "AddMonthDotCalender", "DotController.cs", ex.Message, dot.CreatedIn, dot.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dots
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDots)]
        public async Task<IActionResult> ReadDots()
        {
            try
            {
                var dotsInDb = await context.DotRepository.GetDots();

                return Ok(dotsInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDots", "DotController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dot/key/{tbServiceId}
        /// </summary>
        /// <param name="tbServiceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadDotByTBService)]
        public async Task<IActionResult> ReadDotByTBService(Guid tbserviceid)
        {
            try
            {
                var dotInDb = await context.DotRepository.GetDotByTBService(tbserviceid);

                return Ok(dotInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDotByTBService", "DotController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dot/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Dots.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDotByKey)]
        public async Task<IActionResult> ReadDotByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dotInDb = await context.DotRepository.GetDotByKey(key);

                if (dotInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(dotInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDotByKey", "DotController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dot/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Dot.</param>
        /// <param name="dot">Dot to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDot)]
        public async Task<IActionResult> UpdateDot(Guid key, Dot dot)
        {
            try
            {
                if (key != dot.InteractionId)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = dot.ModifiedBy;
                interactionInDb.ModifiedIn = dot.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                dot.DateModified = DateTime.Now;
                dot.IsDeleted = false;
                dot.IsSynced = false;

                context.DotRepository.Update(dot);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateDot", "DotController.cs", ex.Message, dot.ModifiedIn, dot.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dot/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Dot.</param>
        /// <returns>Http status code: Ok.</returns> 
        [HttpDelete]
        [Route(RouteConstants.DeleteDot)]
        public async Task<IActionResult> DeleteDot(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dotInDb = await context.DotRepository.GetDotByKey(key);

                if (dotInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                dotInDb.DateModified = DateTime.Now;
                dotInDb.IsDeleted = true;
                dotInDb.IsSynced = false;

                context.DotRepository.Update(dotInDb);
                await context.SaveChangesAsync();

                return Ok(dotInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDot", "DotController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}