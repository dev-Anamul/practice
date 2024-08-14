using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    ///FamilyMember  Controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class FamilyMemberController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FamilyMemberController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public FamilyMemberController(IUnitOfWork context, ILogger<FamilyMemberController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/family-member
        /// </summary>
        /// <param name="familyMember">FamilyMember object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFamilyMember)]
        public async Task<IActionResult> CreateFamilyMember(FamilyMembersDto familyMember)
        {
            FamilyMember familyMembersLast = new FamilyMember();
            try
            {
                List<FamilyMember> listFamilyMembers = new List<FamilyMember>();

                foreach (var item in familyMember.FamilyMembers)
                {
                    Interaction interaction = new Interaction();

                    Guid interactionId = Guid.NewGuid();

                    interaction.Oid = interactionId;
                    interaction.ServiceCode = ServiceCodeFormatter.FormatServiceCode(Enums.StatusCode.FamilyMember, familyMember.EncounterType);
                    interaction.EncounterId = familyMember.EncounterID;
                    interaction.DateCreated = DateTime.Now;
                    interaction.CreatedBy = familyMember.CreatedBy;
                    interaction.CreatedIn = familyMember.CreatedIn;
                    interaction.IsDeleted = false;
                    interaction.IsSynced = false;

                    context.InteractionRepository.Add(interaction);

                    familyMembersLast.InteractionId = interactionId;
                    familyMembersLast.FirstName = item.FirstName;
                    familyMembersLast.FamilyMemberType = item.FamilyMemberType;
                    familyMembersLast.EncounterType = familyMember.EncounterType;
                    familyMembersLast.OtherFamilyMember = item.OtherFamilyMember;
                    familyMembersLast.Surname = item.Surname;
                    familyMembersLast.HIVResult = item.HIVResult;
                    familyMembersLast.HIVTested = item.HIVTested;
                    familyMembersLast.OnART = item.OnART;
                    familyMembersLast.Age = item.Age;
                    familyMembersLast.EncounterId = familyMember.EncounterID;
                    familyMembersLast.ClientId = familyMember.ClientID;
                    familyMembersLast.DateCreated = DateTime.Now;
                    familyMembersLast.CreatedBy = familyMember.CreatedBy;
                    familyMembersLast.CreatedIn = familyMember.CreatedIn;
                    familyMembersLast.IsDeleted = false;
                    familyMembersLast.IsSynced = false;

                    context.FamilyMemberRepository.Add(familyMembersLast);
                    await context.SaveChangesAsync();

                    listFamilyMembers.Add(item);
                }

                familyMembersLast.FamilyMembers = listFamilyMembers;

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateFamilyMember", "FamilyMemberController.cs", ex.Message, familyMember.CreatedIn, familyMember.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }

            return CreatedAtAction("ReadFamilyMemberByKey", new { key = familyMember.FamilyMembers.Select(x => x.InteractionId) }, familyMember);
        }

        /// <summary>
        /// URL: sc-api/family-members
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyMembers)]
        public async Task<IActionResult> ReadFamilyMembers()
        {
            try
            {
                var familyMemberInDb = await context.FamilyMemberRepository.GetFamilyMembers();

                return Ok(familyMemberInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyMembers", "FamilyMemberController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-member/by-client/{clientId}
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyMemberByClient)]
        public async Task<IActionResult> ReadFamilyMemberByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                if (pageSize == 0)
                {
                  //  var familyMemberInDb = await context.FamilyMemberRepository.GetFamilyMemberByClientId(clientId);
                    var familyMemberInDb = await context.FamilyMemberRepository.GetFamilyMemberByClientIdLast24Hours(clientId);

                    return Ok(familyMemberInDb);
                }
                else
                {
                    var familyMemberInDb = await context.FamilyMemberRepository.GetFamilyMemberByClientId(clientId, ((page - 1) * (pageSize)), pageSize, encounterType);
                    PagedResultDto<FamilyMember> familyMemberDto = new PagedResultDto<FamilyMember>()
                    {
                        Data = familyMemberInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = context.FamilyMemberRepository.GetFamilyMemberByClientIdTotalCount(clientId, encounterType)
                    };

                    return Ok(familyMemberDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyMemberByClient", "FamilyMemberController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/family-member/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyMembers.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFamilyMemberByKey)]
        public async Task<IActionResult> ReadFamilyMemberByKey(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var familyMemberInDb = await context.FamilyMemberRepository.GetFamilyMemberByKey(key);

                if (familyMemberInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(familyMemberInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFamilyMemberByKey", "FamilyMemberController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///URL: sc-api/family-member/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FamilyMembers.</param>
        /// <param name="familyMember">FamilyMember to be updated.</param>
        /// <returns>Http status code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFamilyMember)]
        public async Task<IActionResult> UpdateFamilyMember(Guid key, FamilyMember familyMember)
        {
            try
            {
                if (key != familyMember.InteractionId)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var interactionInDb = await context.InteractionRepository.GetInteractionByKey(key);

                if (interactionInDb == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                interactionInDb.ModifiedBy = familyMember.ModifiedBy;
                interactionInDb.ModifiedIn = familyMember.ModifiedIn;
                interactionInDb.DateModified = DateTime.Now;
                interactionInDb.IsDeleted = false;
                interactionInDb.IsSynced = false;

                context.InteractionRepository.Update(interactionInDb);


                familyMember.DateModified = DateTime.Now;
                familyMember.IsDeleted = false;
                familyMember.IsSynced = false;

                context.FamilyMemberRepository.Update(familyMember);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateFamilyMember", "FamilyMemberController.cs", ex.Message, familyMember.ModifiedIn, familyMember.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}