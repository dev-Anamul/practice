using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 13.02.2023
 * Modified by   : 
 * Last modified : 
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

   public class PartographDetailController : ControllerBase
   {
      private readonly IUnitOfWork context;
      private readonly ILogger<PartographDetailController> logger;

      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="context">Instance of the UnitOfWork.</param>
      public PartographDetailController(IUnitOfWork context)
      {
         this.context = context;
      }

      // <summary>
      /// URL: sc-api/partograph-detail
      /// </summary>
      /// <param name="partographDetails"></param>
      /// <returns></returns>
      [HttpPost]
      [Route(RouteConstants.CreatePartographDetail)]
      public async Task<IActionResult> CreatePartographDetail(PartographDetailDto partographDetail)
      {
         try
         {
            var partographDetailInDb = new PartographDetail
            {
               PartographId = partographDetail.InteractionId,
               Liquor = partographDetail.Liquor,
               LiquorTime = partographDetail.LiquorTime,
               Moulding = partographDetail.Moulding,
               MouldingTime = partographDetail.MouldingTime,
               Cervix = partographDetail.Cervix,
               CervixTime = partographDetail.CervixTime,
               DescentOfHead = partographDetail.DescentOfHead,
               DescentOfHeadTime = partographDetail.DescentOfHeadTime,
               Contractions = partographDetail.Contractions,
               ContractionsDuration = partographDetail.ContractionsDuration,
               ContractionsTime = partographDetail.ContractionsTime,
               Oxytocin = partographDetail.Oxytocin,
               OxytocinTime = partographDetail.OxytocinTime,
               Drops = partographDetail.Drops,
               DropsTime = partographDetail.DropsTime,
               Medicine = partographDetail.Medicine,
               MedicineTime = partographDetail.MedicineTime,
               Systolic = partographDetail.Systolic,
               Diastolic = partographDetail.Diastolic,
               BpTime = partographDetail.BpTime,
               Pulse = partographDetail.Pulse,
               PulseTime = partographDetail.PulseTime,
               Temp = partographDetail.Temp,
               TempTime = partographDetail.TempTime,
               Protein = partographDetail.Protein,
               ProteinTime = partographDetail.ProteinTime,
               Acetone = partographDetail.Acetone,
               AcetoneTime = partographDetail.AcetoneTime,
               Volume = partographDetail.Volume,
               VolumeTime = partographDetail.VolumeTime,
               FetalRate = partographDetail.FetalRate,
               FetalRateTime = partographDetail.FetalRateTime,
               IsSynced = false,
               IsDeleted = false,
               DateCreated = DateTime.Now,
               CreatedIn = partographDetail.CreatedIn,
               CreatedBy = partographDetail.CreatedBy
            };

            var partographDetailAdded = context.PartographDetailsRepository.Add(partographDetailInDb);
            await context.SaveChangesAsync();

            var partographDetailToReturn = await context.PartographDetailsRepository.GetByIdAsync(partographDetailAdded.InteractionId);

            return Ok(partographDetailToReturn);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreatePartographDetail", "PartographDetailController.cs", ex.Message, partographDetail.CreatedIn, partographDetail.CreatedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/partograph-details
      /// </summary>
      /// <param name="partographId"></param>
      /// <returns></returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographDetails)]
      public IActionResult ReadPartographDetails(Guid partographId)
      {
         try
         {
            var partographDetailInDb = context.PartographDetailsRepository
                .GetAll()
                .Where(p => p.PartographId == partographId && p.IsDeleted.Equals(false))
                .OrderByDescending(o => o.DateCreated)
                .ToList();

            return Ok(partographDetailInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographDetails", "PartographDetailController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/partograph-detail/key/{key}
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographDetailByKey)]
      public async Task<IActionResult> ReadPartographDetailByKey(Guid key)
      {
         try
         {
            if (key == Guid.Empty)
               return BadRequest("Invalid key!");

            var partographDetailInDb = await context.PartographDetailsRepository.GetByIdAsync(key);

            if (partographDetailInDb == null)
               return NotFound();

            return Ok(partographDetailInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographDetailByKey", "PartographDetailController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/partograph-detail/by-client/{partographId}
      /// </summary>
      /// <param name="partographId"></param>
      /// <returns></returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographDetailByPartographKey)]
      public async Task<IActionResult> ReadPartographDetailByPartographKey(Guid partographId)
      {
         try
         {
            if (partographId == Guid.Empty)
               return BadRequest("Invalid key!");

            var partographDetailInDb = await context.PartographDetailsRepository.GetPartographDetailsAsync(partographId);

            if (partographDetailInDb == null)
               return NotFound();

            return Ok(partographDetailInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographDetailByPartographKey", "PartographDetailController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: sc-api/partograph-detail/by-client/{partographId}
      /// </summary>
      /// <param name="partographId"></param>
      /// <returns></returns>
      [HttpGet]
      [Route(RouteConstants.ReadPartographDetailByPartograph)]
      public async Task<IActionResult> ReadPartographDetailByPartograph(Guid partographId)
      {
         try
         {
            if (partographId == Guid.Empty)
               return BadRequest("Invalid key!");

            var partographDetailInDb = await context.PartographDetailsRepository.GetPartographDetailsbyPartograph(partographId);

            if (partographDetailInDb == null)
               return NotFound();

            return Ok(partographDetailInDb);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadPartographDetailByPartograph", "PartographDetailController.cs", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      // <summary>
      /// URL: sc-api/partograph-detail/{key}
      /// </summary>
      /// <param name="partographDetail"></param>
      /// <returns></returns>
      [HttpPut]
      [Route(RouteConstants.UpdatePartographDetail)]
      public async Task<IActionResult> UpdatePartographDetail(PartographDetailDto partographDetail)
      {
         try
         {
            if (partographDetail.InteractionId == Guid.Empty)
               return BadRequest();

            var partographDetailInDb = await context.PartographDetailsRepository.GetByIdAsync(partographDetail.InteractionId);

            if (partographDetailInDb == null)
               return NotFound();

            partographDetailInDb.PartographId = partographDetail.InteractionId;
            partographDetailInDb.Liquor = partographDetail.Liquor;
            partographDetailInDb.LiquorTime = partographDetail.LiquorTime;
            partographDetailInDb.Moulding = partographDetail.Moulding;
            partographDetailInDb.MouldingTime = partographDetail.MouldingTime;
            partographDetailInDb.Cervix = partographDetail.Cervix;
            partographDetailInDb.CervixTime = partographDetail.CervixTime;
            partographDetailInDb.DescentOfHead = partographDetail.DescentOfHead;
            partographDetailInDb.DescentOfHeadTime = partographDetail.DescentOfHeadTime;
            partographDetailInDb.Contractions = partographDetail.Contractions;
            partographDetailInDb.ContractionsDuration = partographDetail.ContractionsDuration;
            partographDetailInDb.ContractionsTime = partographDetail.ContractionsTime;
            partographDetailInDb.Oxytocin = partographDetail.Oxytocin;
            partographDetailInDb.OxytocinTime = partographDetail.OxytocinTime;
            partographDetailInDb.Drops = partographDetail.Drops;
            partographDetailInDb.DropsTime = partographDetail.DropsTime;
            partographDetailInDb.Medicine = partographDetail.Medicine;
            partographDetailInDb.MedicineTime = partographDetail.MedicineTime;
            partographDetailInDb.Systolic = partographDetail.Systolic;
            partographDetailInDb.Diastolic = partographDetail.Diastolic;
            partographDetailInDb.BpTime = partographDetail.BpTime;
            partographDetailInDb.Pulse = partographDetail.Pulse;
            partographDetailInDb.Temp = partographDetail.Temp;
            partographDetailInDb.TempTime = partographDetail.TempTime;
            partographDetailInDb.Protein = partographDetail.Protein;
            partographDetailInDb.ProteinTime = partographDetail.ProteinTime;
            partographDetailInDb.Acetone = partographDetail.Acetone;
            partographDetailInDb.AcetoneTime = partographDetail.AcetoneTime;
            partographDetailInDb.Volume = partographDetail.Volume;
            partographDetailInDb.VolumeTime = partographDetail.VolumeTime;
            partographDetailInDb.FetalRate = partographDetail.FetalRate;
            partographDetailInDb.FetalRateTime = partographDetail.FetalRateTime;

            partographDetailInDb.IsDeleted = false;
            partographDetailInDb.IsSynced = false;
            partographDetailInDb.DateModified = DateTime.Now;

            context.PartographDetailsRepository.Update(partographDetailInDb);
            await context.SaveChangesAsync();

            var partographDetailsToReturn = await context.PartographDetailsRepository.GetByIdAsync(partographDetailInDb.InteractionId);

            return Ok(partographDetailsToReturn);
         }
         catch (Exception ex)
         {
            logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdatePartographDetail", "PartographDetailController.cs", ex.Message, partographDetail.ModifiedIn, partographDetail.ModifiedBy);
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }
   }
}