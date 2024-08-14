using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 13.02.2023
 * Modified by   : Brian
 * Last modified : 12.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Api.Controllers
{
   /// <summary>
   /// Partograph controller.
   /// </summary>
   [Route(RouteConstants.BaseRoute)]
   [ApiController]
   [Authorize]

   public class PartographController : ControllerBase
   {
      private readonly IUnitOfWork context;
      private readonly ILogger<PartographController> logger;
      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="context">Instance of the UnitOfWork.</param>
      public PartographController(IUnitOfWork context, ILogger<PartographController> logger)
      {
         this.context = context;
         this.logger = logger;
      }

      /// <summary>
      /// URL: sc-api/fluid
      /// </summary>
      /// <param name="turningChart">TurningChart object.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpPost]
      [Route(RouteConstants.CreatePartograph)]
      public async Task<IActionResult> CreatePartograph(Partograph partograph)
      {
         try
         {
            if (partograph.InteractionId == Guid.Empty)
            {
               var partographInDb = new Partograph
               {
                  AdmissionId = partograph.AdmissionId,
                  Gravida = partograph.Gravida,
                  Parity = partograph.Parity,
                  SBOrNND = partograph.SBOrNND,
                  Abortion = partograph.Abortion,
                  EDD = partograph.EDD,
                  EncounterId = partograph.EncounterId,
                  BorderlineRiskFactors = partograph.BorderlineRiskFactors,
                  Height = partograph.Height,
                  RegularContractions = partograph.RegularContractions,
                  MembranesRuptured = partograph.MembranesRuptured,
                  InitiateDate = partograph.InitiateDate,
                  InitiateTime = partograph.InitiateTime,
                  DateCreated = partograph.DateCreated = DateTime.Now,
                  IsDeleted = false,
                  IsSynced = false,
                  CreatedBy = partograph.CreatedBy,
                  CreatedIn = partograph.CreatedIn,
               };

               var partographAdd = context.PartographRepository.Add(partographInDb);
               await context.SaveChangesAsync();
               return Ok(partographAdd);
            }
            else
            {
               var partographInDb = await context.PartographRepository.GetByIdAsync(partograph.InteractionId);

               if (partographInDb == null)
                  return NotFound();

               partographInDb.AdmissionId = partograph.AdmissionId;
               partographInDb.Gravida = partograph.Gravida;
               partographInDb.Parity = partograph.Parity;
               partographInDb.SBOrNND = partograph.SBOrNND;
               partographInDb.Abortion = partograph.Abortion;
               partographInDb.EDD = partograph.EDD;
               partographInDb.DateModified = partograph.DateModified = DateTime.Now;
               partographInDb.BorderlineRiskFactors = partograph.BorderlineRiskFactors;
               partographInDb.Height = partograph.Height;
               partographInDb.RegularContractions = partograph.RegularContractions;
               partographInDb.MembranesRuptured = partograph.MembranesRuptured;
               partographInDb.IsSynced = false;
               partographInDb.IsDeleted = false;

               context.PartographRepository.Update(partographInDb);
               await context.SaveChangesAsync();

               return Ok(partographInDb);
            }
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePartograph", "PartographController.cs", ex.Message, partograph.CreatedIn, partograph.CreatedBy);
            return BadRequest(ex.Message);
         }
      }

      /// <summary>
      /// URL: sc-api/partographs
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographs)]
      public IActionResult ReadPartographs(Guid admissionId)
      {
         try
         {
            var partographInDb = context.PartographRepository
                 .GetAll()
                 .Where(p => p.AdmissionId == admissionId && p.IsDeleted.Equals(false))
                 .OrderByDescending(o => o.DateCreated)
                 .ToList();

            return Ok(partographInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographs", "PartographController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }
      /// <summary>
      /// URL: sc-api/partograph/byclient/{clientid}
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographByClient)]
      public async Task<IActionResult> ReadPartographByClient(Guid clientId)
      {
         try
         {
            var partographInDb = await context.PartographRepository.GetPartographByClient(clientId);

            return Ok(partographInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographByClient", "PartographController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/partograph/ByAdmission/{AdmissionId}
      /// </summary>
      /// <param name="admissionId"></param>
      /// <returns></returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographByEncounterId)]
      public async Task<IActionResult> ReadPartographByEncounter(Guid encounterId)
      {
         try
         {
            var partographInDb = await context.PartographRepository
                .GetPartographByEncounter(encounterId);

            return Ok(partographInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographByEncounter", "PartographController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      // <summary>
      /// URL: sc-api/partograph/key/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Partograph.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographByKey)]
      public async Task<IActionResult> ReadPartographByKey(Guid key)
      {
         try
         {
            if (key == Guid.Empty)
               return BadRequest("Invalid key!");

            var partographInDb = await context.PartographRepository.GetByIdAsync(key);

            if (partographInDb == null)
               return NotFound();

            return Ok(partographInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographByKey", "PartographController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/partograph/{key}
      /// </summary>
      /// <param name="partograph"></param>
      /// <returns></returns>
      [HttpPut]
      [Route(RouteConstants.UpdatePartograph)]
      public async Task<IActionResult> UpdatePartograph(Partograph partograph)
      {
         try
         {
            if (partograph.InteractionId == Guid.Empty)
               return BadRequest();

            var partographInDb = await context.PartographRepository.GetByIdAsync(partograph.InteractionId);

            if (partographInDb == null)
               return NotFound();

            partographInDb.AdmissionId = partograph.AdmissionId;
            partographInDb.Gravida = partograph.Gravida;
            partographInDb.Parity = partograph.Parity;
            partographInDb.SBOrNND = partograph.SBOrNND;
            partographInDb.Abortion = partograph.Abortion;
            partographInDb.EDD = partograph.EDD;
            partographInDb.BorderlineRiskFactors = partograph.BorderlineRiskFactors;
            partographInDb.Height = partograph.Height;
            partographInDb.RegularContractions = partograph.RegularContractions;
            partographInDb.MembranesRuptured = partograph.MembranesRuptured;
            partographInDb.InitiateDate = partograph.InitiateDate;
            partographInDb.InitiateTime = partograph.InitiateTime;
            partographInDb.IsDeleted = false;
            partographInDb.IsSynced = false;
            partographInDb.DateModified = DateTime.Now;

            context.PartographRepository.Update(partographInDb);
            await context.SaveChangesAsync();

            var partographInDbToReturn = await context.PartographRepository.GetByIdAsync(partographInDb.InteractionId);

            return Ok(partographInDbToReturn);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePartograph", "PartographController.cs", ex.Message, partograph.ModifiedIn, partograph.ModifiedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }
   }
}