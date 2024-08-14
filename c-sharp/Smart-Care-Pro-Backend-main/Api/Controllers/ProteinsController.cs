using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Proteins controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ProteinsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ProteinsController> logger;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ProteinsController(IUnitOfWork context, ILogger<ProteinsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/protein
        /// </summary>
        /// <param name="proteins">Protein object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateProtein)]
        public async Task<IActionResult> CreateProtein(ProteinsCreateDto proteins)
        {
            try
            {
                var proteinsList = proteins.Data?.Select(x => new Protein()
                {
                    ProteinsDetails = Convert.ToString(x[1]),
                    ProteinsTime = Convert.ToInt64(x[0]),
                    PartographId = proteins.PartographId,
                }).ToList() ?? new List<Protein>();

                foreach (var item in proteinsList)
                {
                    item.DateCreated = DateTime.Now;
                    item.IsSynced = false;
                    item.IsDeleted = false;

                    context.ProteinsRepository.UpdateProtein(item);
                }

                await context.SaveChangesAsync();

                return Ok(proteinsList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateProtein", "ProteinsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}