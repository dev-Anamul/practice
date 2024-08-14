using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : Bella
 * Last modified : 14.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// ChiefComplaint controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class InvestigationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<InvestigationController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public InvestigationController(IUnitOfWork context, ILogger<InvestigationController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/investigations
        /// </summary>
        /// <param name="investigations">Investigations object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateInvestigation)]
        public async Task<IActionResult> CreateInvestigation(List<Investigation> investigations)
        {
            try
            {

                foreach (var investigation in investigations)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Investigation, Enums.EncounterType.Investigation);
                    interaction.EncounterId = investigation.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedIn = investigation.CreatedIn;
                    interaction.CreatedBy = investigation.CreatedBy;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;
                    context.InteractionRepository.Add(interaction);

                    investigation.InteractionId = interactionId;
                    investigation.DateCreated = DateTime.Now;
                    investigation.IsDeleted = false;
                    investigation.IsSynced = false;
                    context.InvestigationRepository.Add(investigation);
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("ReadInvestigationByKey", new { key = investigations.FirstOrDefault() }, investigations.FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateInvestigation", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/investigation
        /// </summary>
        /// <param name="model">Investigation object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateInvestigationByTest)]
        public async Task<IActionResult> CreateInvestigationByTest(Investigation model)
        {
            try
            {
                CompositeTest compositeTests = await context.CompositeTestRepository.GetCompositeTestByKey(model.TestId);

                List<Interaction> interactions = new();
                List<Investigation> investigations = new();


                if (compositeTests.TestItems.Count() > 0)
                {
                    foreach (var test in compositeTests.TestItems)
                    {
                        Interaction interaction = new();
                        Investigation investigation = new();

                        Guid interactionID = Guid.NewGuid();

                        interaction.Oid = interactionID;
                        interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Investigation, Enums.EncounterType.Investigation);
                        interaction.EncounterId = model.EncounterId;
                        interaction.DateCreated = DateTime.Now;
                        interaction.CreatedBy = model.CreatedBy;
                        interaction.CreatedIn = model.CreatedIn;
                        interaction.IsDeleted = false;
                        interaction.IsSynced = false;

                        interactions.Add(interaction);

                        investigation.InteractionId = interactionID;
                        investigation.DateCreated = DateTime.Now;
                        investigation.IsDeleted = false;
                        investigation.IsSynced = false;
                        investigation.Quantity = model.Quantity;
                        investigation.OrderNumber = model.OrderNumber;
                        investigation.SampleQuantity = model.SampleQuantity;
                        investigation.Piority = model.Piority;
                        investigation.AdditionalComment = model.AdditionalComment;
                        investigation.IsResultReceived = false;
                        investigation.OrderDate = model.OrderDate;
                        investigation.TestId = test.TestId;
                        investigation.EncounterId = model.EncounterId;
                        investigation.ClientId = model.ClientId;
                        investigation.ClinicianId = model.ClinicianId;
                        investigation.CreatedIn = model.CreatedIn;
                        investigation.CreatedBy = model.CreatedBy;
                        investigations.Add(investigation);
                    }

                    context.InteractionRepository.AddRange(interactions);

                    context.InvestigationRepository.AddRange(investigations);

                    await context.SaveChangesAsync();
                    return CreatedAtAction("ReadInvestigationByKey", new { key = model.TestId }, investigations);
                }
                else
                    return StatusCode(StatusCodes.Status204NoContent, MessageConstants.GenericError);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateInvestigationByTest", "InvestigationController.cs", ex.Message, model.CreatedIn, model.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/investigations
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInvestigations)]
        public async Task<IActionResult> ReadInvestigations()
        {
            try
            {
                var investigationInDb = await context.InvestigationRepository.GetInvestigations();

                return Ok(investigationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInvestigations", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/investigation-by-encounter/key/{encounterId}
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInvestigationsByEncounterId)]
        public async Task<IActionResult> ReadInvestigationsByEncounterId(Guid encounterId)
        {
            try
            {
                var investigationInDb = await context.InvestigationRepository.GetInvestigationByEncounter(encounterId);

                return Ok(investigationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInvestigationsByEncounterId", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/investigation-by-client/key/{clientId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInvestigationsByClient)]
        public async Task<IActionResult> ReadInvestigationsByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                    var investigationDto = await context.InvestigationRepository.GetInvestigationDtoByClient(clientId);

                    return Ok(investigationDto);
                }
                else
                {
                    var investigationDto = await context.InvestigationRepository.GetInvestigationDtoByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    PagedResultDto<InvestigationDto> investigationPagedDto = new PagedResultDto<InvestigationDto>()
                    {
                        Data = investigationDto.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.InvestigationRepository.GetInvestigationByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(investigationPagedDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInvestigationsByClient", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/investigation/investigation-dashboard/{facilityId}
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="PatientName"></param>
        /// <param name="investigationDateSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadInvestigationsDashBoard)]
        public async Task<ActionResult<InvestigationDashBoardDto>> ReadInvestigationsDashBoard(int facilityId, int page, int pageSize, string? patientName = "", string? investigationDateSearch = "")
        {
            try
            {
                var investigationInDb = await context.InvestigationRepository.GetInvestigationDashBoard(facilityId, ((page - 1) * (pageSize)), pageSize, patientName, investigationDateSearch);

                if (investigationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                int totalItems = 0;
                int resultReceivedTotalItems = 0;

                if (string.IsNullOrEmpty(patientName) && string.IsNullOrEmpty(investigationDateSearch))
                {
                    totalItems = context.InvestigationRepository.Count(x => x.CreatedIn == facilityId);
                    resultReceivedTotalItems = context.InvestigationRepository.Count(x => x.CreatedIn == facilityId && x.IsResultReceived == true);

                }
                else if (!string.IsNullOrEmpty(patientName) && string.IsNullOrEmpty(investigationDateSearch))
                {
                    string[] searchTerms = patientName.Split(' ');

                    totalItems = context.InvestigationRepository.Count(p => p.CreatedIn == facilityId && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname)), c => c.Client);
                    resultReceivedTotalItems = context.InvestigationRepository.Count(p => p.CreatedIn == facilityId && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname)) && p.IsResultReceived == true, c => c.Client);
                }
                else if (string.IsNullOrEmpty(patientName) && !string.IsNullOrEmpty(investigationDateSearch))
                {
                    DateTime investigationDate = DateTime.ParseExact(investigationDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    totalItems = context.InvestigationRepository.Count(p => p.CreatedIn == facilityId && p.OrderDate.Date == investigationDate);
                    resultReceivedTotalItems = context.InvestigationRepository.Count(p => p.CreatedIn == facilityId && p.OrderDate.Date == investigationDate && p.IsResultReceived == true);

                }
                else if (!string.IsNullOrEmpty(patientName) && !string.IsNullOrEmpty(investigationDateSearch))
                {
                    string[] searchTerms = patientName.Split(' ');

                    DateTime investigationDate = DateTime.ParseExact(investigationDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    totalItems = context.InvestigationRepository.Count(p => p.CreatedIn == facilityId && p.OrderDate.Date == investigationDate && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname)), c => c.Client);
                    resultReceivedTotalItems = context.InvestigationRepository.Count(p => p.CreatedIn == facilityId && p.OrderDate.Date == investigationDate && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname)) && p.IsResultReceived == true, c => c.Client);
                }

                var investigationDashBoardDto = new InvestigationDashBoardDto
                {
                    Investigations = new List<InvestigationItemDto>(),
                    TotalItems = totalItems,
                    ResultRecievedTotalItems = resultReceivedTotalItems,
                    PageNumber = page,
                    PageSize = pageSize
                };


                foreach (var investigationItem in investigationInDb)
                {
                    investigationDashBoardDto.Investigations.Add(new InvestigationItemDto
                    {
                        InteractionId = investigationItem.InteractionId,
                        EncounterId= investigationItem.EncounterId,
                        OrderDate = investigationItem.OrderDate,
                        OrderNumber = investigationItem.OrderNumber,
                        SampleCollectionDate = investigationItem.SampleCollectionDate,
                        Quantity = investigationItem.Quantity,
                        SampleQuantity = investigationItem.SampleQuantity,
                        Piority = investigationItem.Piority,
                        ImagingTestDetails = investigationItem.ImagingTestDetails,
                        AdditionalComment = investigationItem.AdditionalComment,
                        IsResultReceived = investigationItem.IsResultReceived,
                        ClinicianId = investigationItem.ClinicianId,
                        TestId = investigationItem.TestId,
                        ClientId = investigationItem.ClientId,
                        FirstName = investigationItem.Client.FirstName,
                        Surname = investigationItem.Client.Surname,
                        TestName = investigationItem.Test.Title,
                        Lonic=investigationItem.Test.LONIC,
                        SubTypeId = investigationItem.Test.SubtypeId,
                        ResultType= investigationItem.Test.ResultType,
                        Results = investigationItem.Results?.Select(result => new ResultDto
                        {
                            InteractionId=result.InteractionId,
                            ResultDate = result.ResultDate,
                            ResultDescriptive = result.ResultDescriptive,
                            ResultNumeric = result.ResultNumeric,
                            CommentOnResult = result.CommentOnResult,
                            IsResultNormal = result.IsResultNormal,
                            InvestigationId = result.InvestigationId
                        }).ToList() ?? new List<ResultDto>()
                    });
                }

                return Ok(investigationDashBoardDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInvestigationsDashBoard", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/investigation/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Investigations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadInvestigationByKey)]
        [AllowAnonymous]
        public async Task<IActionResult> ReadInvestigationByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var investigationInDb = await context.InvestigationRepository.GetInvestigationByKey(key);

                if (investigationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(investigationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInvestigationByKey", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/investigations-batch/key/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadInvestigationByBatch)]
        public async Task<IActionResult> ReadInvestigationByBatch(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var investigationInDb = await context.InvestigationRepository.GetInvestigationByEncounterId(key);

                if (investigationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(investigationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadInvestigationByBatch", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/investigation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Investigations.</param>
        /// <param name="investigation">Investigation to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateInvestigation)]
        public async Task<IActionResult> UpdateInvestigation(Guid key, Investigation investigation)
        {
            try
            {
                if (key != investigation.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = investigation.ModifiedBy;
                interactionInDb.ModifiedIn = investigation.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                Investigation investigationInDb = await context.InvestigationRepository.GetInvestigationByKey(investigation.InteractionId);

                if (investigationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                investigationInDb.DateModified = DateTime.Now;
                investigationInDb.AdditionalComment = investigation.AdditionalComment;
                investigationInDb.ImagingTestDetails = investigation.ImagingTestDetails;
                investigationInDb.Quantity = investigation.Quantity;
                investigationInDb.SampleQuantity = investigation.SampleQuantity;
                investigationInDb.OrderDate = investigation.OrderDate;
                investigationInDb.OrderNumber = investigation.OrderNumber;
                investigationInDb.TestId = investigation.TestId;
                investigationInDb.Piority = investigation.Piority;
                investigationInDb.ModifiedBy = investigation.ModifiedBy;
                investigationInDb.ModifiedIn = investigation.ModifiedIn;
                investigationInDb.IsSynced = false;
                investigationInDb.IsDeleted = false;

                context.InvestigationRepository.Update(investigationInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateInvestigation", "InvestigationController.cs", ex.Message, investigation.ModifiedIn, investigation.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL:sc-api/investigation/sample-collection/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <param name="investigation"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(RouteConstants.UpdateInvestigationSampleCollection)]
        public async Task<IActionResult> UpdateInvestigationSampleCollection(Guid key, InvestigationSampleCollectionDto investigation)
        {
            try
            {
                if (key != investigation.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                Investigation investigationInDb = await context.InvestigationRepository.GetInvestigationByKey(investigation.InteractionId);

                investigationInDb.DateModified = DateTime.Now;
                investigationInDb.SampleCollectionDate = investigation.SampleCollectionDate.Add(investigation.SampleCollectionTime);

                context.InvestigationRepository.Update(investigationInDb);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateInvestigationSampleCollection", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/investigation/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Investigations.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteInvestigation)]
        public async Task<IActionResult> DeleteInvestigation(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var investigationInDb = await context.InvestigationRepository.GetInvestigationByKey(key);

                if (investigationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                investigationInDb.DateModified = DateTime.Now;
                investigationInDb.IsDeleted = true;
                investigationInDb.IsSynced = false;

                context.InvestigationRepository.Update(investigationInDb);
                await context.SaveChangesAsync();

                return Ok(investigationInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteInvestigation", "InvestigationController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}