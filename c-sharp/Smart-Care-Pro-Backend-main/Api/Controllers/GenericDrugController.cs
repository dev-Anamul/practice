using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 11.03.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{

    /// <summary>
    /// GenericDrug controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class GenericDrugController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<GenericDrugController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public GenericDrugController(IUnitOfWork context, ILogger<GenericDrugController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/generic-medicine
        /// </summary>
        /// <param name="genericMedicine">GenericMedicine object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateGenericMedicine)]
        public async Task<IActionResult> CreateGenericMedicine(GenericDrug genericMedicine)
        {
            try
            {
                if (await IsGenericMedicineDuplicate(genericMedicine) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                genericMedicine.DateCreated = DateTime.Now;
                genericMedicine.IsDeleted = false;
                genericMedicine.IsSynced = false;

                context.GenericDrugRepository.Add(genericMedicine);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadGenericMedicineByKey", new { key = genericMedicine.Oid }, genericMedicine);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateGenericMedicine", "GenericDrugController.cs", ex.Message, genericMedicine.CreatedIn, genericMedicine.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generic-medicines
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGenericMedicines)]
        public async Task<IActionResult> ReadGenericMedicines()
        {
            try
            {
                var GenericMedicineInDb = await context.GenericDrugRepository.GetGenericMedicines();

                return Ok(GenericMedicineInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGenericMedicines", "GenericDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generic-medicines
        /// </summary>
        /// <param name="term"></param>
        /// <param name="drugUtility"></param>
        /// <param name="drugClass"></param>
        /// <param name="drugSubclass"></param>
        /// <param name="physicalSystem"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.SearchGenericMedicines)]
        public async Task<IActionResult> SearchGenericMedicines(string term, string? drugUtility, string? drugClass, string? drugSubclass, string? physicalSystem)
        {
            try
            {
                term = term.ToLower();
                var drugDetails = await context.DrugDefinitionRepository.LoadListWithChildAsync<GeneralDrugDefinition>(x => x.Description.ToLower().Contains(term) && x.IsDeleted == false, p => p.GenericDrug, p => p.DrugUtility, p => p.DrugDosageUnit, p => p.DrugFormulation, p => p.GenericDrug.DrugSubclass, p => p.GenericDrug.DrugSubclass.DrugClass, p => p.SystemRelevances);

                if (Convert.ToInt16(physicalSystem) > 0)
                    drugDetails = drugDetails.Where(x => x.SystemRelevances.Any(y => y.PhysicalSystemId == Convert.ToInt16(physicalSystem))).ToList();

                if (Convert.ToInt16(drugUtility) > 0)
                    drugDetails = drugDetails.Where(x => x.DrugUtilityId == Convert.ToInt16(drugUtility)).ToList();

                if (Convert.ToInt16(drugClass) > 0)
                {
                    if (Convert.ToInt16(drugSubclass) == 0)
                        drugDetails = drugDetails.Where(x => x.GenericDrug.DrugSubclass.DrugClassId == Convert.ToInt16(drugClass)).ToList();
                }

                if (Convert.ToInt16(drugSubclass) > 0)
                    drugDetails = drugDetails.Where(x => x.GenericDrug.SubclassId == Convert.ToInt16(drugSubclass)).ToList();

                var result = drugDetails.ToList().Select(x => new AutoCompleteOutPutDto
                {
                    value = x.Oid.ToString(),
                    desc = x.GenericDrug.Description + " " + x.DrugFormulation.Description + " " + x.Strength.ToString("0.####") + " " + x.DrugDosageUnit.Description,
                    GenericName = x.GenericDrug.Description,
                    PragnancyRisk = x.GenericDrug.PregnancyRisk.ToString(),
                    BreastFeedingRisk = x.GenericDrug.BreastFeedingRisk.ToString(),
                    Formulation = x.DrugFormulation.Description,
                    DosageUnit = x.DrugDosageUnit.Description,
                    Strength = x.Strength.ToString("0.####"),

                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "SearchGenericMedicines", "GenericDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generic-medicine/interactionId/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadGenericMedicineByInteractionId)]
        public async Task<IActionResult> ReadGenericMedicineByInteractionId(int key)
        {
            try
            {
                var drugDetails = await context.DrugDefinitionRepository.LoadWithChildAsync<GeneralDrugDefinition>(x => x.GenericDrugId == key, p => p.GenericDrug, p => p.DrugUtility, p => p.DrugDosageUnit, p => p.DrugFormulation, p => p.GenericDrug.DrugSubclass, p => p.GenericDrug.DrugSubclass.DrugClass, p => p.SystemRelevances);

                if (drugDetails is null)
                    return Ok(new AutoCompleteOutPutDto());

                AutoCompleteOutPutDto result = new()
                {
                    value = drugDetails.Oid.ToString(),
                    desc = drugDetails.GenericDrug.Description + " " + drugDetails.DrugFormulation.Description + " " + drugDetails.Strength.ToString("0.####") + " " + drugDetails.DrugDosageUnit.Description,
                    GenericName = drugDetails.GenericDrug.Description,
                    PragnancyRisk = drugDetails.GenericDrug.PregnancyRisk.ToString(),
                    BreastFeedingRisk = drugDetails.GenericDrug.BreastFeedingRisk.ToString(),
                    Formulation = drugDetails.DrugFormulation.Description,
                    DosageUnit = drugDetails.DrugDosageUnit.Description,
                    Strength = drugDetails.Strength.ToString("0.####"),

                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGenericMedicineByInteractionId", "GenericDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generic-medicine/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GenericMedicine.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadGenericMedicineByKey)]
        public async Task<IActionResult> ReadGenericMedicineByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var genericMedicineInDb = await context.GenericDrugRepository.GetGenericMedicineByKey(key);

                if (genericMedicineInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(genericMedicineInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadGenericMedicineByKey", "GenericDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generic-medicine/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GenericMedicines.</param>
        /// <param name="genericDrug">GenericMedicine to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateGenericMedicine)]
        public async Task<IActionResult> UpdateGenericMedicine(int key, GenericDrug genericDrug)
        {
            try
            {
                if (key != genericDrug.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                genericDrug.DateModified = DateTime.Now;
                genericDrug.IsDeleted = false;
                genericDrug.IsSynced = false;

                context.GenericDrugRepository.Update(genericDrug);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateGenericMedicine", "GenericDrugController.cs", ex.Message, genericDrug.ModifiedIn, genericDrug.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/generic-medicine/{key}
        /// </summary>
        /// <param name="key">Primary key of the table GenericMedicines.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteGenericMedicine)]
        public async Task<IActionResult> DeleteGenericMedicine(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var genericDrugInDb = await context.GenericDrugRepository.GetGenericMedicineByKey(key);

                if (genericDrugInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                genericDrugInDb.DateModified = DateTime.Now;
                genericDrugInDb.IsDeleted = true;
                genericDrugInDb.IsSynced = false;

                context.GenericDrugRepository.Update(genericDrugInDb);
                await context.SaveChangesAsync();

                return Ok(genericDrugInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteGenericMedicine", "GenericDrugController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the Generic Medicine name is duplicate or not.
        /// </summary>
        /// <param name="genericMedicine">GenericMedicine object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsGenericMedicineDuplicate(GenericDrug genericMedicine)
        {
            try
            {
                var genericMedicineInDb = await context.GenericDrugRepository.GetGenericMedicineByName(genericMedicine.Description);

                if (genericMedicineInDb != null)
                    if (genericMedicineInDb.Oid != genericMedicine.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsGenericMedicineDuplicate", "GenericDrugController.cs", ex.Message);
                throw;
            }
        }
    }
}