using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 15.03.2023
 * Modified by  : Stephan
 * Last modified: 28.10.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// ARTRegister controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ARTRegisterController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ARTRegisterController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ARTRegisterController(IUnitOfWork context, ILogger<ARTRegisterController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/art-register
        /// </summary>
        /// <param name="artRegister">ARTRegister object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateARTRegister)]
        public async Task<IActionResult> CreateARTRegister(ARTService artRegister)
        {
            try
            {
                var existingARTRegister = await context.ARTRegisterRepository.FirstOrDefaultAsync(x => x.Oid == artRegister.Oid);

                if (existingARTRegister != null)
                {
                    existingARTRegister.ModifiedBy = artRegister.ModifiedBy;
                    existingARTRegister.ModifiedIn = artRegister.ModifiedIn;
                    existingARTRegister.DateCreated = artRegister.DateCreated;
                    existingARTRegister.CanPatientBeVisitedAtHome = artRegister.CanPatientBeVisitedAtHome;
                    existingARTRegister.CanReceiveCalls = artRegister.CanReceiveCalls;
                    existingARTRegister.CanReceiveSMS = artRegister.CanReceiveSMS;
                    existingARTRegister.CanPatientBeVisitedAtWork = artRegister.CanPatientBeVisitedAtWork;
                    existingARTRegister.ConsentForFollowUpPrograms = artRegister.ConsentForFollowUpPrograms;
                    existingARTRegister.CoupleCounselling = artRegister.CoupleCounselling;
                    existingARTRegister.DisclosureCounselling = artRegister.DisclosureCounselling;
                    existingARTRegister.DoesChildKnowStatus = artRegister.DoesChildKnowStatus;
                    existingARTRegister.DoesClientWantToBeEnrolledInSupportGroup = artRegister.DoesClientWantToBeEnrolledInSupportGroup;
                    existingARTRegister.InterpersonalViolenceScreened = artRegister.InterpersonalViolenceScreened;
                    existingARTRegister.IsEnrolledInSupportGroup = artRegister.IsEnrolledInSupportGroup;
                    existingARTRegister.IsITPNSAccepted = artRegister.IsITPNSAccepted;
                    existingARTRegister.IsITPNSOffered = artRegister.IsITPNSOffered;
                    existingARTRegister.NotificationServices = artRegister.NotificationServices;
                    existingARTRegister.NumberOfOtherContacts = artRegister.NumberOfOtherContacts;
                    existingARTRegister.NumberOfSexualContacts = artRegister.NumberOfSexualContacts;
                    existingARTRegister.NumberOfBiologicalContacts = artRegister.NumberOfBiologicalContacts;
                    existingARTRegister.DateCreated = DateTime.Now;
                    existingARTRegister.IsSynced = false;
                    existingARTRegister.IsDeleted = false;

                    context.ARTRegisterRepository.Update(existingARTRegister);
                    await context.SaveChangesAsync();


                    var interactionInDb = await context.InteractionRepository.GetInteractionByKey(existingARTRegister.Oid);

                    if (interactionInDb == null)
                        return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                    interactionInDb.ModifiedBy = existingARTRegister.CreatedBy;
                    interactionInDb.ModifiedIn = existingARTRegister.CreatedIn;
                    interactionInDb.DateModified = DateTime.Now;
                    interactionInDb.IsDeleted = false;
                    interactionInDb.IsSynced = false;

                    context.InteractionRepository.Update(interactionInDb);
                    await context.SaveChangesAsync();
                }
                else
                {

                    artRegister.DateCreated = DateTime.Now;
                    artRegister.IsDeleted = false;
                    artRegister.IsSynced = false;

                    if (artRegister.ARTNumber == "-1")
                    {
                        int counter = 1;
                        artRegister.ARTNumber = await GenerateArtNoAsync(artRegister.CreatedIn.Value, counter);

                        while (context.ARTRegisterRepository.Count(x => x.ARTNumber == artRegister.ARTNumber) > 0)
                        {
                            counter = counter++;
                            artRegister.ARTNumber = await GenerateArtNoAsync(artRegister.CreatedIn.Value, counter);
                        }

                    }

                    var artNoCheck = await context.ARTRegisterRepository.FirstOrDefaultAsync(x => x.ARTNumber == artRegister.ARTNumber);

                    if (artNoCheck != null)
                        return StatusCode(StatusCodes.Status400BadRequest, "ART No Already Taken");

                    context.ARTRegisterRepository.Add(artRegister);
                    await context.SaveChangesAsync();

                    Interaction interaction = new Interaction();

                    interaction.Oid = artRegister.Oid;
                    interaction.EncounterId = artRegister.EncounterId;
                    interaction.DateCreated = DateTime.Now;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.ARTService, artRegister.EncounterType);
                    interaction.CreatedIn = artRegister.CreatedIn;
                    interaction.CreatedBy = artRegister.CreatedBy;
                    interaction.IsSynced = false;
                    interaction.IsDeleted = false;

                    context.InteractionRepository.Add(interaction);
                    await context.SaveChangesAsync();
                }

                var client = await context.ClientRepository.GetByIdAsync(artRegister.Oid);

                if (client == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Client Not found");

                client.IsOnART = true;
                client.HIVStatus = HIVStatus.Positive;

                context.ClientRepository.Update(client);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadARTRegisterByKey", new { key = artRegister.Oid }, artRegister);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateARTRegister", "ARTRegisterController.cs", ex.Message, artRegister.CreatedIn, artRegister.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-registers
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTRegister)]
        public async Task<IActionResult> ReadARTRegister()
        {
            try
            {
                var artRegisterInDb = await context.ARTRegisterRepository.GetARTRegisters();

                artRegisterInDb = artRegisterInDb.OrderByDescending(x => x.DateCreated);

                return Ok(artRegisterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTRegister", "ARTRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-register/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ArtRegister.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTRegisterByKey)]
        public async Task<IActionResult> ReadARTRegisterByKey(Guid key)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var artRegisterInDb = await context.ARTRegisterRepository.GetARTRegisterByKey(key);

                if (artRegisterInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(artRegisterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTRegisterByKey", "ARTRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// art-register/by-client/{clientid}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTRegisterByClient)]
        public async Task<IActionResult> ReadARTRegisterByClient(Guid clientId)
        {
            try
            {
                //var artRegisterInDb = await context.ARTRegisterRepository.GetARTRegisterbyClienId(clientId);
                var artRegisterInDb = await context.ARTRegisterRepository.GetARTRegisterbyClienIdLast24Hours(clientId);

                return Ok(artRegisterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTRegisterByClient", "ARTRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: art-registernumber/by-client/{clientid}
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadARTNumberByClient)]
        public async Task<IActionResult> ReadARTNumberByClient(Guid clientId)
        {
            try
            {
                var artRegisterInDb = await context.ARTRegisterRepository.FirstOrDefaultAsync(x => x.Oid == clientId && x.ARTNumber != null);
                var clinician = await context.UserAccountRepository.FirstOrDefaultAsync(x => x.Oid == artRegisterInDb.CreatedBy);
                if (clinician != null)
                    artRegisterInDb.ClinicianName = clinician.FirstName + " " + clinician.Surname;

                var facility = await context.FacilityRepository.FirstOrDefaultAsync(x => x.Oid == artRegisterInDb.CreatedIn);
                if (facility != null)
                    artRegisterInDb.FacilityName = facility.Description;

                return Ok(artRegisterInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadARTNumberByClient", "ARTRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/art-register/{key}
        /// </summary>
        /// <param name="key">Primary key of the table ARTRegister.</param>
        /// <param name="aRTRegister">ARTRegister to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateARTRegister)]
        public async Task<IActionResult> UpdateARTRegister(Guid key, ARTService artRegister)
        {
            try
            {
                if (string.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                artRegister.DateModified = DateTime.Now;
                artRegister.IsDeleted = false;
                artRegister.IsSynced = false;

                context.ARTRegisterRepository.Update(artRegister);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateARTRegister", "ARTRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: art-generatednumber/by-client/{clientId}
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadGeneratedARTNumber)]
        public async Task<IActionResult> ReadGeneratedARTNumber(int facilityId)
        {
            try
            {
                string ARTNumber = string.Empty;

                int counter = 1;

                ARTNumber = await GenerateArtNoAsync(facilityId, counter);

                while (context.ARTRegisterRepository.Count(x => x.ARTNumber == ARTNumber) > 0)
                {
                    counter = counter++;
                    ARTNumber = await GenerateArtNoAsync(facilityId, counter);
                }

                return Ok(ARTNumber);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGeneratedARTNumber", "ARTRegisterController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Generate ART Number Generate
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="Counter"></param>
        /// <returns></returns>
        private async Task<string> GenerateArtNoAsync(int facilityId, int Counter)
        {
            var facility = await context.FacilityRepository.FirstOrDefaultAsync(x => x.Oid == facilityId);
            var district = await context.DistrictRepository.FirstOrDefaultAsync(x => x.Oid == facility.DistrictId);
            var province = await context.ProvinceRepository.FirstOrDefaultAsync(x => x.Oid == district.ProvinceId);

            var count = context.ARTRegisterRepository.Count(x => x.Oid != Guid.Empty) + 1;

            string numberString = count.ToString();
            string serialNumber = numberString.ToString().PadLeft(5, '0');
            string provinceCode = province.ProvinceCode.Substring(0, 2);
            string districtCode = district.DistrictCode.Substring(0, 2);
            string facilityCode = facility.HMISCode.PadLeft(3, '0');

            facilityCode = facilityCode.Length >= 3 ? facilityCode.Substring(facilityCode.Length - 3) : facilityCode;
            facilityCode = facilityCode.PadLeft(3, '0');

            int sum = 0;

            foreach (char digitChar in numberString)
            {
                int digit = int.Parse(digitChar.ToString());
                sum += digit;
            }

            int lastDigit = sum % 10;
            string ARTNumber = provinceCode + districtCode + "-" + facilityCode + "-" + serialNumber + "-" + lastDigit.ToString();

            return ARTNumber;
        }
    }
}