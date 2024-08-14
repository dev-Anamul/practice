using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 18.02.2023
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
    /// <summary>
    /// Covid controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class CovidController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<CovidController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public CovidController(IUnitOfWork context, ILogger<CovidController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/covid
        /// </summary>
        /// <param name="covid">Covid object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCovid)]
        public async Task<IActionResult> CreateCovid(Covid covid)
        {
            try
            {
                Interaction interaction = new Interaction();

                Guid interactionId = Guid.NewGuid();

                interaction.Oid = interactionId;
                interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.Covid, Enums.EncounterType.Covid);
                interaction.EncounterId = covid.EncounterId;
                interaction.DateCreated = DateTime.Now;
                interaction.CreatedBy = covid.CreatedBy;
                interaction.CreatedIn = covid.CreatedIn;
                interaction.IsSynced = false;
                interaction.IsDeleted = false;

                context.InteractionRepository.Add(interaction);

                covid.InteractionId = interactionId;
                covid.DateCreated = DateTime.Now;
                covid.IsDeleted = false;
                covid.IsSynced = false;

                context.CovidRepository.Add(covid);
                await context.SaveChangesAsync();

                if (covid.CovidSymptomScreeningList != null)
                {
                    foreach (var item in covid.CovidSymptomScreeningList)
                    {
                        CovidSymptomScreening covidSymptomScreening = new CovidSymptomScreening();
                        covidSymptomScreening.InteractionId = Guid.NewGuid();
                        covidSymptomScreening.CovidSymptomId = item;
                        covidSymptomScreening.CovidId = covid.InteractionId;
                        covidSymptomScreening.CreatedBy = covid.CreatedBy;
                        covidSymptomScreening.CreatedIn = covid.CreatedIn;
                        covidSymptomScreening.DateCreated = DateTime.Now;
                        covidSymptomScreening.IsDeleted = false;
                        covidSymptomScreening.IsSynced = false;

                        context.CovidSymptomScreeningRepository.Add(covidSymptomScreening);
                        await context.SaveChangesAsync();
                    }
                }

                if (covid.CovidComobidityList != null)
                {
                    foreach (var item in covid.CovidComobidityList)
                    {
                        CovidComorbidity covidComorbidities = new CovidComorbidity();

                        covidComorbidities.InteractionId = Guid.NewGuid();
                        covidComorbidities.CovidComorbidityConditions = item;
                        covidComorbidities.CovidId = covid.InteractionId;
                        covidComorbidities.CreatedBy = covid.CreatedBy;
                        covidComorbidities.CreatedIn = covid.CreatedIn;
                        covidComorbidities.DateCreated = DateTime.Now;
                        covidComorbidities.IsSynced = false;
                        covidComorbidities.IsDeleted = false;

                        context.CovidComorbidityRepository.Add(covidComorbidities);
                        await context.SaveChangesAsync();
                    }
                }

                if (covid.ExposureRisksList != null)
                {
                    foreach (var item in covid.ExposureRisksList)
                    {
                        ExposureRisk exposureRisk = new ExposureRisk();
                        exposureRisk.InteractionId = Guid.NewGuid();
                        exposureRisk.ExposureRisks = item;
                        exposureRisk.CovidId = covid.InteractionId;
                        exposureRisk.CreatedBy = covid.CreatedBy;
                        exposureRisk.CreatedIn = covid.CreatedIn;
                        exposureRisk.DateCreated = DateTime.Now;
                        exposureRisk.IsDeleted = false;
                        exposureRisk.IsSynced = false;

                        context.ExposerRiskRepository.Add(exposureRisk);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadCovidByKey", new { key = covid.InteractionId }, covid);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateCovid", "CovidController.cs", ex.Message, covid.CreatedIn, covid.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

        }

        /// <summary>
        /// URL: sc-api/covids
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovids)]
        public async Task<IActionResult> ReadCovids()
        {
            try
            {
                var covidInDb = await context.CovidRepository.GetCovids();

                covidInDb = covidInDb.OrderByDescending(x => x.DateCreated);

                return Ok(covidInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovids", "CovidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid/ByClient/{ClientID}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovidByClient)]
        public async Task<IActionResult> ReadCovidByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                  //  var covidInDb = await context.CovidRepository.GetCovidByClient(clientId);
                    var covidInDb = await context.CovidRepository.GetCovidByClientLast24Hours(clientId);

                    foreach (var item in covidInDb)
                    {
                        item.CovidSymptomScreenings = await context.CovidSymptomScreeningRepository.LoadListWithChildAsync<CovidSymptomScreening>(c => c.IsDeleted == false && c.CovidId == item.InteractionId, x => x.CovidSymptom);
                    }

                    return Ok(covidInDb);
                }
                else
                {
                    var covidInDb = await context.CovidRepository.GetCovidByClient(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);

                    foreach (var item in covidInDb)
                    {
                        item.CovidSymptomScreenings = await context.CovidSymptomScreeningRepository.LoadListWithChildAsync<CovidSymptomScreening>(c => c.IsDeleted == false && c.CovidId == item.InteractionId, x => x.CovidSymptom);
                    }

                    PagedResultDto<Covid> covidDto = new PagedResultDto<Covid>()
                    {
                        Data = covidInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.CovidRepository.GetCovidByClientTotalCount(clientId, encounterType)
                    };

                    return Ok(covidDto);
                }

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovidByClient", "CovidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/covid/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Covids.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCovidByKey)]
        public async Task<IActionResult> ReadCovidByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covidInDb = await context.CovidRepository.GetCovidByKey(key);

                if (covidInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                covidInDb.CovidSymptomScreenings = await context.CovidSymptomScreeningRepository.LoadListWithChildAsync<CovidSymptomScreening>(c => c.CovidId == covidInDb.InteractionId, s => s.CovidSymptom);
                covidInDb.CovidComorbidities = await context.CovidComorbidityRepository.LoadListWithChildAsync<CovidComorbidity>(c => c.CovidId == covidInDb.InteractionId);
                covidInDb.ExposureRisks = await context.ExposerRiskRepository.LoadListWithChildAsync<ExposureRisk>(c => c.CovidId == covidInDb.InteractionId);

                return Ok(covidInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCovidByKey", "CovidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/covid/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthRecords.</param>
        /// <param name="birthRecord">BirthRecord to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCovid)]
        public async Task<IActionResult> UpdateCovid(Guid key, Covid covid)
        {
            try
            {
                if (key != covid.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = covid.ModifiedBy;
                interactionInDb.ModifiedIn = covid.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);

                covid.DateModified = DateTime.Now;
                covid.IsDeleted = false;
                covid.IsSynced = false;

                context.CovidRepository.Update(covid);
                await context.SaveChangesAsync();

                var covidSymptom = await context.CovidSymptomScreeningRepository.GetCovidSymptomScreeenByCovid(covid.InteractionId);

                foreach (var item in covidSymptom.ToList())
                {
                    context.CovidSymptomScreeningRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                var comorbidity =  await context.CovidComorbidityRepository.GetCovidComorbidityByCovid(covid.InteractionId);

                foreach (var item in comorbidity.ToList())
                {
                    context.CovidComorbidityRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                var exposureRiskItem = await context.ExposerRiskRepository.GetExposureRiskByCovid(covid.InteractionId);

                foreach (var item in exposureRiskItem.ToList())
                {
                    context.ExposerRiskRepository.Delete(item);
                    await context.SaveChangesAsync();
                }

                if (covid.CovidSymptomScreeningList != null)
                {

                    foreach (var item in covid.CovidSymptomScreeningList)
                    {
                        CovidSymptomScreening covidSymptomScreening = new CovidSymptomScreening();

                        covidSymptomScreening.InteractionId = Guid.NewGuid();
                        covidSymptomScreening.CovidSymptomId = item;
                        covidSymptomScreening.CovidId = covid.InteractionId;
                        covidSymptomScreening.ModifiedIn = covid.ModifiedIn;
                        covidSymptomScreening.ModifiedBy = covid.ModifiedBy;
                        covidSymptomScreening.DateModified = DateTime.Now;
                        covidSymptomScreening.EncounterId = covid.EncounterId;
                        covidSymptomScreening.IsSynced = false;
                        covidSymptomScreening.IsDeleted = false;

                        context.CovidSymptomScreeningRepository.Add(covidSymptomScreening);
                        await context.SaveChangesAsync();
                    }
                }

                if (covid.CovidComobidityList != null)
                {
                    foreach (var item in covid.CovidComobidityList)
                    {
                        CovidComorbidity covidComorbidities = new CovidComorbidity();

                        covidComorbidities.InteractionId = Guid.NewGuid();
                        covidComorbidities.CovidComorbidityConditions = item;
                        covidComorbidities.CovidId = covid.InteractionId;
                        covidComorbidities.ModifiedIn = covid.ModifiedIn;
                        covidComorbidities.ModifiedBy = covid.ModifiedBy;
                        covidComorbidities.DateModified = DateTime.Now;
                        covidComorbidities.EncounterId = covid.EncounterId;
                        covidComorbidities.IsDeleted = false;
                        covidComorbidities.IsSynced = false;

                        context.CovidComorbidityRepository.Add(covidComorbidities);
                        await context.SaveChangesAsync();
                    }
                }

                if (covid.ExposureRisksList != null)
                {
                    foreach (var item in covid.ExposureRisksList)
                    {
                        ExposureRisk exposureRisk = new ExposureRisk();
                        exposureRisk.InteractionId = Guid.NewGuid();
                        exposureRisk.ExposureRisks = item;
                        exposureRisk.CovidId = covid.InteractionId;
                        exposureRisk.ModifiedIn = covid.ModifiedIn;
                        exposureRisk.ModifiedBy = covid.ModifiedBy;
                        exposureRisk.DateModified = DateTime.Now;
                        exposureRisk.EncounterId = covid.EncounterId;
                        exposureRisk.IsDeleted = false;
                        exposureRisk.IsSynced = false;

                        context.ExposerRiskRepository.Add(exposureRisk);
                        await context.SaveChangesAsync();
                    }
                }

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateCovid", "CovidController.cs", ex.Message, covid.ModifiedIn, covid.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/birth-record/{key}
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCovid)]
        public async Task<IActionResult> DeleteCovid(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var covidInDb = await context.CovidRepository.GetCovidByKey(key);

                if (covidInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                covidInDb.DateModified = DateTime.Now;
                covidInDb.IsDeleted = true;
                covidInDb.IsSynced = false;

                context.CovidRepository.Update(covidInDb);
                await context.SaveChangesAsync();

                return Ok(covidInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCovid", "CovidController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}