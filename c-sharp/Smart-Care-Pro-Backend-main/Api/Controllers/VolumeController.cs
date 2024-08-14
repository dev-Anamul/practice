using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by    : Stephan
 * Date created  : 13.02.2023
 * Modified by   : Stephan
 * Last modified : 13.08.2023
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

    public class VolumeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<VolumeController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public VolumeController(IUnitOfWork context, ILogger<VolumeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/volume
        /// </summary>
        /// <param name="volumes">Volumes object.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateVolume)]
        public async Task<IActionResult> CreateVolume(VolumeDto volumes)
        {
            try
            {
                var volumesList = volumes.Data?.Select(x => new Volume()
                {
                    VolumesDetails = Convert.ToString(x[1]),
                    VolumesTime = Convert.ToInt64(x[0]),
                    PartographId = volumes.PartographId,

                }).ToList() ?? new List<Volume>();

                foreach (var item in volumesList)
                {
                    context.VolumesRepository.UpdateVolume(item);
                }

                await context.SaveChangesAsync();

                return Ok(volumesList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateVolume", "VolumeController.cs", ex.Message, "", "");
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}