using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ModuleAccessController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ModuleAccessController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ModuleAccessController(IUnitOfWork context, ILogger<ModuleAccessController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/module-access
        /// </summary>
        /// <param name="moduleAccessDto">ModuleAccessDto object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateModuleAccess)]
        public async Task<IActionResult> CreateModulesAccess(ModuleAccessDto moduleAccessDto)
        {
            try
            {
                var moduleAccessesByFacility = await context.ModuleAccessRepository.GetModuleAccessesByFacilityAccess(moduleAccessDto.FacilityAccessId);

                if (moduleAccessesByFacility.Count() > 0)
                {
                    foreach (var item in moduleAccessesByFacility)
                    {
                        context.ModuleAccessRepository.Delete(item);
                        await context.SaveChangesAsync();
                    }
                }

                foreach (var item in moduleAccessDto.Modules)
                {
                    if (item.Checked)
                    {
                        ModuleAccess moduleAccess = new ModuleAccess()
                        {
                            CreatedBy = moduleAccessDto.CreatedBy,
                            CreatedIn = moduleAccessDto.CreatedIn,
                            DateCreated = moduleAccessDto.DateCreated,
                            FacilityAccessId = moduleAccessDto.FacilityAccessId,
                            ModuleCode = item.Id,
                            IsDeleted = false,
                            IsSynced = false
                        };
                        context.ModuleAccessRepository.Add(moduleAccess);
                        if (item.Id == 37)
                        {//painRecord does not exixt in EncounterType its already been handled in front end MedicalEncounterIPD
                         //ModuleAccess moduleAccessPainRecords = new ModuleAccess()
                         //{
                         //    CreatedBy = moduleAccessDto.CreatedBy,
                         //    CreatedIn = moduleAccessDto.CreatedIn,
                         //    DateCreated = moduleAccessDto.DateCreated,
                         //    FacilityAccessId = moduleAccessDto.FacilityAccessId,
                         //    ModuleCode = (int)EncounterType.PainRecord,
                         //    IsDeleted = false,
                         //    IsSynced = false
                         //}; 
                         //context.ModuleAccessRepository.Add(moduleAccessPainRecords);
                        }
                        if (item.Id == 8)
                        {
                            ModuleAccess moduleAccessTBFollowUp = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.TBFollowUp,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessTBFollowUp);
                        }
                        if (item.Id == 4)
                        {
                            ModuleAccess moduleAccessARTIHPAI = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ARTIHPAI,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessARTIHPAI);

                            ModuleAccess moduleAccessARTFollowUpp = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ARTFollowUp,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessARTFollowUpp);

                            ModuleAccess moduleAccessARTStableOnCare = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ARTStableOnCare,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessARTStableOnCare);
                        }
                        if (item.Id == 14)
                        {
                            ModuleAccess moduleAccessPediatricIHPAI = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.PediatricIHPAI,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessPediatricIHPAI);

                            ModuleAccess moduleAccessPediatricFollowUp = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.PediatricFollowUp,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessPediatricFollowUp);

                            ModuleAccess moduleAccessPediatricStableOnCare = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.PediatricStableOnCare,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessPediatricStableOnCare);
                        }
                        if (item.Id == 16)
                        {
                            //painRecord does not exixt in EncounterType its already been handled in front end MedicalEncounterIPD
                            //ModuleAccess moduleAccessPainRecord = new ModuleAccess()
                            //{
                            //    CreatedBy = moduleAccessDto.CreatedBy,
                            //    CreatedIn = moduleAccessDto.CreatedIn,
                            //    DateCreated = moduleAccessDto.DateCreated,
                            //    FacilityAccessId = moduleAccessDto.FacilityAccessId,
                            //    ModuleCode = (int)EncounterType.PainRecord,
                            //    IsDeleted = false,
                            //    IsSynced = false
                            //};
                            //context.ModuleAccessRepository.Add(moduleAccessPainRecord);
                        }

                        if (item.Id == 17)
                        {
                            ModuleAccess moduleAccessPreTransfusionVital = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.PreTransfusionVital,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessPreTransfusionVital);

                            ModuleAccess moduleAccessIntraTransfusionVital = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.IntraTransfusionVital,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessIntraTransfusionVital);

                        }

                        if (item.Id == 22)
                        {

                            ModuleAccess moduleAccessANCLabourAndDeliveryPMTCT = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANCLabourAndDeliveryPMTCT,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessANCLabourAndDeliveryPMTCT);


                            ModuleAccess moduleAccessANCLabourAndDeliverySummary = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANCLabourAndDeliverySummary,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessANCLabourAndDeliverySummary);

                            ModuleAccess moduleAccessANCDeliveryDischargeMother = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANCDeliveryDischargeMother,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessANCDeliveryDischargeMother);

                            ModuleAccess moduleAccessANCDeliveryDischargeBaby = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANCDeliveryDischargeBaby,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessANCDeliveryDischargeBaby);

                        }
                        if (item.Id == 25)
                        {
                            ModuleAccess moduleAccessPostnatalAdult = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.PostnatalAdult,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessPostnatalAdult);

                            ModuleAccess moduleAccessPostnatalPMTCT_Adult = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.PostnatalPMTCT_Adult,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessPostnatalPMTCT_Adult);

                            ModuleAccess moduleAccessPostnatalPMTCT_Pediatric = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.PostnatalPMTCT_Pediatric,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessPostnatalPMTCT_Pediatric);

                        }

                        if (item.Id == 21)
                        {
                            ModuleAccess moduleAccessANCFollowUp = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANCFollowUp,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessANCFollowUp);

                            ModuleAccess moduleAccessANC_Initial_Already_On_ART = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANC_Initial_Already_On_ART,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessANC_Initial_Already_On_ART);

                            ModuleAccess moduleAccessANC_1st_Time_On_ART = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANC_1st_Time_On_ART,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessANC_1st_Time_On_ART);

                            ModuleAccess moduleAccessANC_Follow_up_PMTCT = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.ANC_Follow_up_PMTCT,
                                IsDeleted = false,
                                IsSynced = false
                            };

                            context.ModuleAccessRepository.Add(moduleAccessANC_Follow_up_PMTCT);

                        }


                        if (item.Id == 44)
                        {
                            ModuleAccess moduleAccessDispensations = new ModuleAccess()
                            {
                                CreatedBy = moduleAccessDto.CreatedBy,
                                CreatedIn = moduleAccessDto.CreatedIn,
                                DateCreated = moduleAccessDto.DateCreated,
                                FacilityAccessId = moduleAccessDto.FacilityAccessId,
                                ModuleCode = (int)EncounterType.Dispensations,
                                IsDeleted = false,
                                IsSynced = false
                            };
                            context.ModuleAccessRepository.Add(moduleAccessDispensations);

                        }
                        await context.SaveChangesAsync();
                    }
                }

                return StatusCode(StatusCodes.Status204NoContent,MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateModulesAccess", "ModuleAccessController.cs", ex.Message, moduleAccessDto.CreatedIn, moduleAccessDto.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/module-access/module-access-by-facility-access/{facilityAccessId}
        /// </summary>
        /// <param name="facilityAccessId">ModuleAccessDto object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModuleAccessByFacility)]
        public async Task<IActionResult> ReadModuleAccessByFacilityAccess(Guid facilityAccessId)
        {
            try
            {
                var facilityAccess = await context.ModuleAccessRepository.GetModuleAccessesByFacilityAccess(facilityAccessId);

                return Ok(facilityAccess);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadModuleAccessByFacilityAccess", "ModuleAccessController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

    }
}