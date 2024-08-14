using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 13.02.2023
 * Modified by  : Brian
 * Last modified: 21.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Contractions Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ContractionsController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ContractionsController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ContractionsController(IUnitOfWork context, ILogger<ContractionsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/contraction
        /// </summary>
        /// <param name="contractions">Contraction object.</param>
        /// <returns>Http status code: CreatedAt.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateContraction)]
        public async Task<IActionResult> CreateContraction(ContractionsCreateDto contractions)
        {
            try
            {
                var contractionsList = contractions.Data?.Select(x => new Contraction()
                {
                    ContractionsTime = Convert.ToInt64(x[0]),
                    ContractionsDetails = Convert.ToInt32(x[1]),
                    Duration = Convert.ToString(x[2]),
                    PartographId = contractions.PartographId,
                }).ToList() ?? new List<Contraction>();

                foreach (var item in contractionsList)
                {
                    context.ContractionsRepository.UpdateContraction(item);
                }

                await context.SaveChangesAsync();

                return Ok(contractionsList);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateContraction", "ContractionsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/contractions
        /// </summary>
        /// <param name="partographId"></param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadContractions)]
        public async Task<IActionResult> ReadContractions(Guid partographId)
        {
            try
            {
                var contractionsInDb = context.ContractionsRepository
                    .GetAll()
                    .Where(p => p.PartographId == partographId && p.IsDeleted.Equals(false))
                    .OrderByDescending(o => o.DateCreated)
                    .ToList();

                var data = contractionsInDb.Select(x => new string[]
                {
                    x.ContractionsTime.ToString(),
                    x.ContractionsDetails.ToString(),
                    x.Duration
                })
                .OrderBy(i => i[0])
                .ToList();

                var contractions = new ContractionsCreateDto();

                if (data.Count > 0)
                {
                    contractions = new ContractionsCreateDto()
                    {
                        PartographId = partographId,
                        Data = data
                    };
                }

                return Ok(contractions);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadContractions", "ContractionsController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}