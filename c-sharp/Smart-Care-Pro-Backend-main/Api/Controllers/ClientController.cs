using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Lion
 * Date created  : 12.09.2022
 * Modified by   : Stephan
 * Last modified : 28.10.2023
 * Reviewed by   :
 * Date Reviewed :
 */
namespace Api.Controllers
{
    /// <summary>
    /// Client controller.
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ClientController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Instance of the UnitOfWork.</param>
        public ClientController(IUnitOfWork context, ILogger<ClientController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: sc-api/client
        /// </summary>
        /// <param name="client">Client object.</param>
        /// <returns>Http status code: Created At</returns>
        [HttpPost]
        [Route(RouteConstants.CreateClient)]
        public async Task<IActionResult> CreateClient(Client client)
        {
            try
            {
                if (client.IsDFZClient == true && client.DFZClient != null)
                {
                    if (await IsHospitalNoDuplicate(client.DFZClient) == true)
                        return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateHospitalNoError);

                    //We will check Service No when not null because its not required field
                    if (!string.IsNullOrEmpty(client.DFZClient.ServiceNo))
                        if (await IsServiceNoDuplicate(client.DFZClient) == true)
                            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateServiceNoError);
                }

                var clientWithSameNRC = await context.ClientRepository.GetClientNRC(client.NRC);

                if (clientWithSameNRC != null && client.NoNRC == false)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateClientNRCError);

                int counter = 1;
                client.NUPN = await GenerateNUPNAsync(client.CreatedIn.Value, counter);

                while (context.ClientRepository.Count(x => x.NUPN == client.NUPN) > 0)
                {
                    counter = counter++;
                    client.NUPN = await GenerateNUPNAsync(client.CreatedIn.Value, counter);
                }

                client.DateCreated = DateTime.Now;
                client.IsDeleted = false;
                client.IsSynced = false;
                client.IsAdmitted = false;

                if (client.CellphoneCountryCode == "+260" && client.Cellphone[0] == '0')
                    client.Cellphone = client.Cellphone.Substring(1);

                context.ClientRepository.Add(client);

                if (client.IsDFZClient == true && client.DFZClient != null)
                {
                    DFZClient dFZClient = new DFZClient()
                    {
                        Oid = client.Oid,
                        DFZRank = client.DFZClient.DFZRank,
                        DFZPatientTypeId = client.DFZClient.DFZPatientTypeId,
                        DFZRankId = client.DFZClient.DFZRankId,
                        HospitalNo = client.DFZClient.HospitalNo,
                        Unit = client.DFZClient.Unit,
                        ServiceNo = client.DFZClient.ServiceNo,
                        IsDeleted = false,
                        CreatedBy = client.CreatedBy,
                        DateCreated = client.DateCreated,
                        CreatedIn = client.CreatedIn,
                        IsSynced = client.IsSynced,
                    };
                    context.DFZClientRepository.Add(dFZClient);
                }

                await context.SaveChangesAsync();

                return CreatedAtAction("CreateClient", new { key = client.Oid }, client);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateClient", "ClientController.cs", ex.Message, client.CreatedIn, client.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/client
        /// </summary>
        /// <param name="client">Client object.</param>
        /// <returns>Http status code: Created At</returns>
        [HttpPost]
        [Route(RouteConstants.CreateClientWithRelation)]
        public async Task<IActionResult> CreateClientWithRelation(ClientRelationDto clientRelation)
        {
            try
            {
                if (clientRelation == null)
                    return NotFound();

                Client client = new Client()
                {
                    Oid = new Guid(),
                    FirstName = clientRelation.FirstName,
                    Surname = clientRelation.Surname,
                    Sex = clientRelation.Sex,
                    DOB = clientRelation.DOB,
                    IsDOBEstimated = clientRelation.IsDOBEstimated,
                    NRC = clientRelation.NRC,
                    NoNRC = clientRelation.NoNRC,
                    NAPSANumber = clientRelation.NAPSANumber,
                    UnderFiveCardNumber = clientRelation.UnderFiveCardNumber,
                    NUPN = clientRelation.NUPN,
                    RegistrationDate = clientRelation.RegistrationDate,
                    FathersFirstName = clientRelation.FathersFirstName,
                    FathersSurname = clientRelation.FathersSurname,
                    FathersNRC = clientRelation.FathersNRC,
                    FatherNAPSANumber = clientRelation.FatherNAPSANumber,
                    FatherNationality = clientRelation.FatherNationality,
                    IsFatherDeceased = clientRelation.IsFatherDeceased,
                    MothersFirstName = clientRelation.MothersFirstName,
                    MothersSurname = clientRelation.MothersSurname,
                    MothersNRC = clientRelation.MothersNRC,
                    MotherNAPSANumber = clientRelation.MotherNAPSANumber,
                    MotherNationality = clientRelation.MotherNationality,
                    IsMotherDeceased = clientRelation.IsMotherDeceased,
                    GuardiansFirstName = clientRelation.GuardiansFirstName,
                    GuardiansSurname = clientRelation.GuardiansSurname,
                    GuardiansNRC = clientRelation.GuardiansNRC,
                    GuardianNAPSANumber = clientRelation.GuardianNAPSANumber,
                    GuardianNationality = clientRelation.GuardianNationality,
                    GuardianRelationship = clientRelation.GuardianRelationship,
                    SpousesLegalName = clientRelation.SpousesLegalName,
                    SpousesSurname = clientRelation.SpousesSurname,
                    MaritalStatus = clientRelation.MaritalStatus,
                    CellphoneCountryCode = clientRelation.CellphoneCountryCode,
                    Cellphone = clientRelation.Cellphone,
                    OtherCellphoneCountryCode = clientRelation.OtherCellphoneCountryCode,
                    OtherCellphone = clientRelation.OtherCellphone,
                    NoCellphone = clientRelation.NoCellphone,
                    LandlineCountryCode = clientRelation.LandlineCountryCode,
                    Landline = clientRelation.Landline,
                    Email = clientRelation.Email,
                    HouseholdNumber = clientRelation.HouseholdNumber,
                    Road = clientRelation.Road,
                    Area = clientRelation.Area,
                    Landmarks = clientRelation.Landmarks,
                    IsZambianBorn = clientRelation.IsZambianBorn,
                    BirthPlace = clientRelation.BirthPlace,
                    TownName = clientRelation.TownName,
                    Religion = clientRelation.Religion,
                    HIVStatus = clientRelation.HIVStatus,
                    IsDeceased = clientRelation.IsDeceased,
                    IsOnART = clientRelation.IsOnART,
                    IsAdmitted = clientRelation.IsAdmitted,
                    IsDFZClient = clientRelation.IsDFZClient,
                    MotherProfileId = clientRelation.MotherProfileId,
                    FatherProfileId = clientRelation.FatherProfileId,
                    CountryId = clientRelation.CountryId,
                    DistrictId = clientRelation.DistrictId,
                    HomeLanguageId = clientRelation.HomeLanguageId,
                    EducationLevelId = clientRelation.EducationLevelId,
                    OccupationId = clientRelation.OccupationId,
                    CreatedIn = clientRelation.CreatedIn,
                    DateCreated = DateTime.Now,
                    IsSynced = false,
                    IsDeleted = false,
                    CreatedBy = clientRelation.CreatedBy,
                };

                var existingDependentWithHospitalNo = await context.DFZClientDependentRepository.GetDependentByHospitalNo(clientRelation.HospitalNo);

                if (existingDependentWithHospitalNo != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                var clientWithSameNRC = await context.ClientRepository.GetClientNRC(client.NRC);

                if (clientWithSameNRC != null && client.NoNRC == false)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateClientNRCError);

                int counter = 1;
                client.NUPN = await GenerateNUPNAsync(client.CreatedIn.Value, counter);

                if (client.NUPN == null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.GenericError);

                while (context.ClientRepository.Count(x => x.NUPN == client.NUPN) > 0)
                {
                    counter = counter++;
                    client.NUPN = await GenerateNUPNAsync(client.CreatedIn.Value, counter);
                }

                client.DateCreated = DateTime.Now;
                client.IsDeleted = false;
                client.IsSynced = false;
                client.IsAdmitted = false;

                if (client.CellphoneCountryCode == "+260" && client.Cellphone[0] == '0')
                    client.Cellphone = client.Cellphone.Substring(1);

                context.ClientRepository.Add(client);
                await context.SaveChangesAsync();

                DFZDependent dFZDependent = new DFZDependent()
                {
                    Oid = new Guid(),
                    PrincipalId = clientRelation.PrincipalId,
                    DependentClientId = client.Oid,
                    IsDeleted = false,
                    RelationType = clientRelation.RelationType,
                    IsSynced = false,
                    CreatedBy = client.CreatedBy,
                    DateCreated = DateTime.Now,
                    Description = clientRelation.Description,
                    HospitalNo = clientRelation.HospitalNo,
                    DFZPatientTypeId = clientRelation.DFZPatientTypeId,
                };

                context.DFZClientDependentRepository.Add(dFZDependent);

                await context.SaveChangesAsync();

                return CreatedAtAction("CreateClient", new { key = client.Oid }, client);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateClient", "ClientController.cs", ex.Message, clientRelation.CreatedIn, clientRelation.CreatedIn);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        [HttpPost]
        [Route(RouteConstants.CreateDFZClientRelation)]
        public async Task<IActionResult> CreateDFZClientRelation(DFZDependent client)
        {
            try
            {
                var existingDependentWithHospitalNo = await context.DFZClientDependentRepository.GetDependentByHospitalNo(client.HospitalNo);

                if (existingDependentWithHospitalNo != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                DFZDependent dFZDependent = new DFZDependent()
                {
                    Oid = new Guid(),
                    PrincipalId = client.PrincipalId,
                    DependentClientId = client.DependentClientId,
                    IsDeleted = false,
                    RelationType = client.RelationType,
                    DFZPatientTypeId = client.DFZPatientTypeId,
                    HospitalNo = client.HospitalNo,
                    IsSynced = false,
                    CreatedBy = client.CreatedBy,
                    DateCreated = DateTime.Now,
                    Description = client.Description,
                };

                context.DFZClientDependentRepository.Add(dFZDependent);
                await context.SaveChangesAsync();

                return CreatedAtAction("CreateClient", new { key = client.Oid }, client);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateClient", "ClientController.cs", ex.Message, client.CreatedIn, client.CreatedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/clients
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDFZClientsRelations)]
        public async Task<IActionResult> ReadDFZClientsRelations(Guid key)
        {
            try
            {
                var relativeClientIndb = await context.DFZClientDependentRepository.GetDFZDependentsByPrincipleId(key);

                relativeClientIndb = relativeClientIndb.OrderByDescending(x => x.DateCreated);

                return Ok(relativeClientIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDFZClientsRelations", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
        /// <summary>
        /// URL: dfz-clients-dependency/dependentid/{dependentId}
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDFZClientsByDependentId)]
        public async Task<IActionResult> ReadDFZClientsByDependentId(Guid id)
        {
            try
            {
                var relativeClientIndb = await context.DFZClientDependentRepository.GetDFZDependentByDFZClientId(id);

                if (relativeClientIndb == null)
                    return NotFound();

                // Load principle client data
                var principleClient = await context.DFZClientRepository.GetDFZClientByKey(relativeClientIndb.PrincipalId);

                var relativeClient = new
                {
                    principleClient,
                    dependentClient = relativeClientIndb.Client

                };

                return Ok(relativeClient);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDFZClientsByDependentId", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/clients
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDFZClientsRelationById)]
        public async Task<IActionResult> ReadDFZClientsRelationById(Guid id)
        {
            try
            {
                var relativeClientIndb = await context.DFZClientDependentRepository.GetDFZDependentsByPrincipleId(id);

                return Ok(relativeClientIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDFZClientsRelationById", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/clients
        /// </summary>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClients)]
        public async Task<IActionResult> ReadClients()
        {
            try
            {
                var clientIndb = await context.ClientRepository.GetClients();

                clientIndb = clientIndb.OrderByDescending(x => x.DateCreated);

                return Ok(clientIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClients", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientDetailsForTOPCardBykey)]
        public async Task<IActionResult> ReadClientDetailsForTOPCard(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                Guid oid = new Guid(key);

                var singleClient = new ClientRelationDto();

                var client = await context.ClientRepository.LoadWithChildAsync<ClientRelationDto>(x => x.Oid == oid,
                    h => h.HomeLanguage,
                    o => o.Occupation,
                    e => e.EducationLevel,
                    c => c.Country,
                    d => d.District);

                if (client == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var vitalInDb = await context.VitalRepository.GetLatestVitalByClient(oid);
                var htsIndb = await context.HTSRepository.GetLatestHTSByClient(oid);

                if (htsIndb != null)
                    htsIndb.RiskAssessments = await context.RiskAssessmentRepository.LoadListWithChildAsync<RiskAssessment>(x => x.HTSId == htsIndb.InteractionId, p => p.HIVRiskFactor);

                TOPCardDto tOPCardDto = new TOPCardDto()
                {
                    client = client,
                    vital = vitalInDb ?? new Vital(),
                    hts = htsIndb ?? new HTS(),
                };

                return Ok(tOPCardDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientDetailsForTOPCard", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : sc-api/client-details-for-rightcard/key/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientDetailsForRightCardByKey)]
        public async Task<IActionResult> ReadClientDetailsForRightCard(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                Guid oid = new Guid(key);

                var diagnosisDb = await context.DiagnosisRepository.GetLastEncounterDiagnosisByClient(oid);
                var investigationDto = await context.InvestigationRepository.GetInvestigationDtoByClient(oid);
                var generalMedicationsInDb = await context.MedicationRepository.GetLatestGeneralMedicationByClient(oid);
                var treatmentPlansInDb = await context.TreatmentPlanRepository.GetLastEncounterTreatmentPlanByClient(oid);

                RightCardDto rightCardDto = new RightCardDto()
                {
                    diagnoses = diagnosisDb == null ? new List<Diagnosis>() : diagnosisDb.ToList(),
                    investigations = investigationDto.ToList() ?? new List<InvestigationDto>(),
                    medicationDto = generalMedicationsInDb ?? new MedicationDto(),
                    treatmentPlans = treatmentPlansInDb == null ? new List<TreatmentPlan>() : treatmentPlansInDb.ToList()
                };

                return Ok(rightCardDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientDetailsForRightCard", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Http status code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByKey)]
        public async Task<IActionResult> ReadClientByKey(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                Guid oid = new Guid(key);

                var singleClient = new ClientRelationDto();

                var client = await context.ClientRepository.LoadWithChildAsync<ClientRelationDto>(
                    x => x.Oid == oid,
                    h => h.HomeLanguage,
                    o => o.Occupation,
                    e => e.EducationLevel,
                    c => c.Country,
                    d => d.District);

                if (client != null && client.IsDFZClient == true)
                {
                    var dFZClient = await context.DFZClientRepository.FirstOrDefaultAsync(x => x.Oid == client.Oid);
                    if (dFZClient != null)
                    {
                       
                        client.DFZClient = dFZClient;
                    }
                }

                if (client == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(client);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByKey", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/client/NRC/{NRC}
        /// </summary>
        /// <param name="NRC">NRC of a client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByNRC)]
        public async Task<IActionResult> ReadClientByNRC(string NRC)
        {
            try
            {
                if (string.IsNullOrEmpty(NRC))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                if (NRC == "000000/00/0")
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.NRC);

                if (NRC == "000000/00/0")
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.NRC);

                var clientIndb = await context.ClientRepository.GetClientListByNRC(HttpUtility.UrlDecode(NRC));

                if (clientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var clientInOrder = clientIndb.OrderByDescending(c => c.DateCreated);

                return Ok(clientInOrder);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByNRC", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/NRC/{NRC}
        /// </summary>
        /// <param name="NRC">NRC of a client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByNUPN)]

        public async Task<IActionResult> ReadClientByNUPN(string NUPN)
        {
            try
            {
                if (string.IsNullOrEmpty(NUPN))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientIndb = await context.ClientRepository.GetClientByNUPN(HttpUtility.UrlDecode(NUPN));

                if (clientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var clientInOrder = clientIndb.OrderByDescending(c => c.DateCreated);

                return Ok(clientInOrder);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByNUPN", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/serviceno/{serviceno}
        /// </summary>
        /// <param name="serviceno">serviceno of a DFZ client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByServiceNo)]

        public async Task<IActionResult> ReadClientByServiceNo(string serviceNo)
        {
            try
            {
                if (string.IsNullOrEmpty(serviceNo))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dfzClient = await context.DFZClientRepository.GetDFZByServiceNo(serviceNo);

                if (dfzClient == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var client = await context.ClientRepository.GetClientByKey(dfzClient.Oid);
                var dfzClientWithClient = new{
                    DFZClient = dfzClient,
                    Client = client
                };
                
                
                return Ok(dfzClientWithClient);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByServiceNo", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/hospitalNo/{hospitalNo}
        /// </summary>
        /// <param name="hospitalNo">hospitalNo of a client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByHospitalNo)]
        public async Task<IActionResult> ReadClientByHospitalNo(string hospitalNo)
        {
            try
            {
                if (string.IsNullOrEmpty(hospitalNo))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dfzClientIndb = await context.DFZClientRepository.GetDFZByHospitalNo(hospitalNo);

                if (dfzClientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var dfzClientList = new List<DFZClient> { dfzClientIndb };

                var dfzClientoOrderBy = dfzClientList.OrderByDescending(c => c.DateCreated);

                return Ok(dfzClientoOrderBy);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByNUPN", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/dependent-client/hospitalNo/{hospitalNo}
        /// </summary>
        /// <param name="hospitalNo">hospitalNo of a client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeptendentClientByHospitalNo)]
        public async Task<IActionResult> ReadDeptendentClientByHospitalNo(string hospitalNo)
        {
            try
            {
                if (string.IsNullOrEmpty(hospitalNo))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dfzDependentClientIndb = await context.DFZClientDependentRepository.GetDependentByHospitalNo(hospitalNo);

                if (dfzDependentClientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var dfzClientList = new List<DFZDependent> { dfzDependentClientIndb };
                var dfzClientoOrderBy = dfzClientList.OrderByDescending(c => c.DateCreated);

                return Ok(dfzClientoOrderBy);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByNUPN", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/client/serviceNo/{serviceNo}
        /// </summary>
        /// <param name="serviceNo">serviceNo of a client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDFZClientByServiceNo)]
        public async Task<IActionResult> ReadDFZClientByServiceNo(string serviceNo)
        {
            try
            {
                if (string.IsNullOrEmpty(serviceNo))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var dfzClientIndb = await context.DFZClientRepository.GetDFZByServiceNo(serviceNo);

                if (dfzClientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var dfzClientList = new List<DFZClient> { dfzClientIndb };

                var dfzClientoOrderBy = dfzClientList.OrderByDescending(c => c.DateCreated);

                return Ok(dfzClientoOrderBy);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDFZClientByServiceNo", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

       
        /// <summary>
        /// URL: sc-api/client/{facilityId}
        /// </summary>
        /// <param name="facilityId">facilityId of a client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ClientByFacility)]
        public async Task<IActionResult> ReadClientByFacility(int facilityId, int page, int pageSize)
        {
            try
            {
                if (pageSize == 0)
                {
                    var clientIndb = await context.ClientRepository.GetClientByFacility(facilityId);

                    if (clientIndb == null)
                        return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                    var clientInOrder = clientIndb.OrderByDescending(c => c.DateCreated);

                    return Ok(clientInOrder);
                }
                else
                {
                    var clientsInDb = await context.ClientRepository.GetClientByFacility(facilityId, ((page - 1) * (pageSize)), pageSize);

                    PagedResultDto<Client> clientsWithPaggingDto = new PagedResultDto<Client>()
                    {
                        Data = clientsInDb.ToList(),
                        PageNumber = page,
                        PageSize = pageSize
                    };

                    return Ok(clientsWithPaggingDto);
                }

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByFacility", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/NRC/{NRC}
        /// </summary>
        /// <param name="NRC">NRC of a client.</param>
        /// <returns>Http Status Code: OK.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByNUPNAndGender)]

        public async Task<IActionResult> ReadClientByNUPNAndGender(string NUPN)
        {
            try
            {
                if (string.IsNullOrEmpty(NUPN))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientIndb = await context.ClientRepository.GetClientByNUPNAndGender(HttpUtility.UrlDecode(NUPN));

                if (clientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (clientIndb != null && clientIndb.IsDFZClient)
                {
                    clientIndb.DFZClient = await context.DFZClientRepository.FirstOrDefaultAsync(x => x.Oid == clientIndb.Oid);
                }
                return Ok(clientIndb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByNUPNAndGender", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/Cellphone/{Cellphone}
        /// </summary>
        /// <param name="cellphone">Cellphone number of a client.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByCellphone)]
        public async Task<IActionResult> ReadClientsByCellphone(string cellphone, string? countryCode)
        {
            try
            {
                if (string.IsNullOrEmpty(cellphone))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                if (countryCode == "+260" && cellphone[0] == '0')
                    cellphone = cellphone.Substring(1);

                var clientIndb = await context.ClientRepository.GetClientByCellPhone(cellphone, countryCode);

                if (clientIndb.Count() <= 0)
                {
                    if (countryCode == "+260")
                        cellphone = "0" + cellphone;

                    clientIndb = await context.ClientRepository.GetClientByCellPhone(cellphone, countryCode);
                }

                if (clientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var clientInOrder = clientIndb.OrderByDescending(c => c.DateCreated);

                return Ok(clientInOrder);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientsByCellphone", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/clientbasicinfo/
        /// </summary>
        /// <param name="firstname">Firstname of a client.</param>
        /// <param name="surname">Surname of a client.</param>
        /// <param name="sex">Sex of a client.</param>
        /// <param name="dob">Date of Birth of a client.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByClientBasicInfo)]

        public async Task<IActionResult> ReadClientByClientBasicInfo(string? firstname, string? surname, string sex, string dob)
        {
            try
            {
                if (string.IsNullOrEmpty(sex))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientIndb = await context.ClientRepository.GetClientByClientBasicInfo(firstname, surname, sex, dob);

                if (clientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var clientInOrder = clientIndb.OrderByDescending(c => c.DateCreated);

                return Ok(clientInOrder);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByClientBasicInfo", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/client/clientbasicinfo/
        /// </summary>
        /// <param name="firstname">Firstname of a client.</param>
        /// <param name="surname">Surname of a client.</param>
        /// <param name="sex">Sex of a client.</param>
        /// <param name="dob">Date of Birth of a client.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadClientByClientNoDob)]
        public async Task<IActionResult> ReadClientByClientNoDob(string? firstname, string? surname, string sex)
        {
            try
            {
                if (string.IsNullOrEmpty(sex))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var clientIndb = await context.ClientRepository.GetClientByClientNoDob(firstname, surname, sex);

                if (clientIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var clientInOrder = clientIndb.OrderByDescending(c => c.DateCreated);

                return Ok(clientInOrder);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadClientByClientNoDob", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        ///  URL: sc-api/client/NUPN-Generate/{facilityId}
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteConstants.GenerateClientNUPN)]

        public async Task<IActionResult> GenerateNUPN(int facilityId)
        {
            if (facilityId == 0)
                return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            int counter = 1;
            string NUPN = await GenerateNUPNAsync(facilityId, counter);

            while (context.ClientRepository.Count(x => x.NUPN == NUPN) > 0)
            {
                counter = counter++;
                NUPN = await GenerateNUPNAsync(facilityId, counter);
            }

            return Ok(NUPN);
        }

        /// <summary>
        /// NUPN Number Generate
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="Counter"></param>
        /// <returns></returns>
        private async Task<string> GenerateNUPNAsync(int facilityId, int Counter)
        {
            var facility = await context.FacilityRepository.LoadWithChildAsync<Facility>(a => a.Oid == facilityId, d => d.District);
            var districtCode = facility.District.DistrictCode;

            string clinicCode = facility.HMISCode;

            if (clinicCode.Length > 4)
                clinicCode = clinicCode.Substring(4);
            else
                clinicCode = "";

            int serial = context.ClientRepository.Count(x => x.IsDeleted == false) + Counter;
            string getserial = serial.ToString();
            getserial = getserial.PadLeft(5, '0');
            int checksum = 0;

            for (int i = 0; i < getserial.Length; i++)
            {
                checksum += int.Parse(getserial[i].ToString());
            }

            if (checksum > 9)
                checksum %= 10;

            string code = $"{districtCode}-{clinicCode}P-{getserial}-{checksum}";
            return code;
        }

        /// <summary>
        /// URL: sc-api/client/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <param name="client">Client to be updated.</param>
        /// <returns>Http Status Code: NoContent.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateClient)]
        public async Task<IActionResult> UpdateClient(Guid key, Client client)
        {
            try
            {
                if (key != client.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                var clientWithSameNRC = await context.ClientRepository.GetClientNRC(client.NRC);

                if (clientWithSameNRC != null)
                {
                    if (clientWithSameNRC.Oid != client.Oid)
                    {
                        if (clientWithSameNRC != null && client.NoNRC == false)
                            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateClientNRCError);
                    }
                }

                client.DateModified = DateTime.Now;
                client.IsDeleted = false;
                client.IsSynced = false;

                if (client.CellphoneCountryCode == "+260" && client.Cellphone[0] == '0')
                    client.Cellphone = client.Cellphone.Substring(1);

                if (client.IsDFZClient == true && client.DFZClient != null)
                {
                    if (await IsHospitalNoDuplicate(client.DFZClient) == true)
                        return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateHospitalNoError);

                    //We will check ServiceNo when not null because its not required field
                    if (!string.IsNullOrEmpty(client.DFZClient.ServiceNo))
                        if (await IsServiceNoDuplicate(client.DFZClient) == true)
                            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateServiceNoError);


                    DFZClient dFZClient = await context.DFZClientRepository.GetByIdAsync(client.Oid);

                    if (dFZClient == null)
                    {
                        dFZClient = new DFZClient
                        {
                            Oid = client.Oid,
                            DFZRank = client.DFZClient.DFZRank,
                            DFZRankId = client.DFZClient.DFZRankId,
                            HospitalNo = client.DFZClient.HospitalNo,
                            Unit = client.DFZClient.Unit,
                            ServiceNo = client.DFZClient.ServiceNo,
                            CreatedBy = client.ModifiedBy,
                            DFZPatientTypeId = client.DFZClient.DFZPatientTypeId,
                            DateCreated = DateTime.Now,
                            CreatedIn = client.ModifiedIn,
                            IsSynced = client.IsSynced,
                            IsDeleted = false,
                        };

                        context.DFZClientRepository.Add(dFZClient);
                    }
                    else
                    {
                        dFZClient.DFZRank = client.DFZClient.DFZRank;
                        dFZClient.DFZRankId = client.DFZClient.DFZRankId;
                        dFZClient.HospitalNo = client.DFZClient.HospitalNo;
                        dFZClient.Unit = client.DFZClient.Unit;
                        dFZClient.DFZPatientTypeId = client.DFZClient.DFZPatientTypeId;
                        dFZClient.ServiceNo = client.DFZClient.ServiceNo;
                        dFZClient.ModifiedIn = client.ModifiedIn;
                        dFZClient.DateModified = DateTime.Now;

                        context.DFZClientRepository.Update(dFZClient);
                    }
                }

                context.ClientRepository.Update(client);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "UpdateClient", "ClientController.cs", ex.Message, client.ModifiedIn, client.ModifiedBy);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL:sc-api/client/link-mother/{key}
        /// </summary>
        /// <param name="key"></param>
        /// <param name="linkMotherDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(RouteConstants.UpdateClientMother)]

        public async Task<IActionResult> UpdateClientMother(string key, ClientLinkMotherDto linkMotherDto)
        {
            try
            {
                if (key != linkMotherDto.ChildOID.ToString())
                    linkMotherDto.ChildOID = new Guid(key);

                var dbClient = context.ClientRepository.Get(Guid.Parse(linkMotherDto.ChildOID.ToString()));

                if (dbClient == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                dbClient.DateModified = DateTime.Now;
                dbClient.IsDeleted = false;
                dbClient.MotherProfileId = linkMotherDto.MotherOID;
                dbClient.IsSynced = false;

                context.ClientRepository.Update(dbClient);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent, MessageConstants.UpdateMessage);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateClientMother", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: sc-api/client
        /// </summary>
        /// <param name="client">Client object.</param>
        /// <returns>Http status code: Created At</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDependentClient)]
        public async Task<IActionResult> UpdateDependentClient(Guid key, ClientRelationDto clientRelation)
        {
            try
            {

                if (clientRelation.Oid != key)
                    return NotFound();

                var clientInDb = await context.ClientRepository.GetClientByKey(clientRelation.Oid);

                if (clientInDb is null)
                    return NotFound();

                clientInDb.FirstName = clientRelation.FirstName;
                clientInDb.Surname = clientRelation.Surname;
                clientInDb.Sex = clientRelation.Sex;
                clientInDb.DOB = clientRelation.DOB;
                clientInDb.IsDOBEstimated = clientRelation.IsDOBEstimated;
                clientInDb.NRC = clientRelation.NRC;
                clientInDb.NoNRC = clientRelation.NoNRC;
                clientInDb.NAPSANumber = clientRelation.NAPSANumber;
                clientInDb.UnderFiveCardNumber = clientRelation.UnderFiveCardNumber;
                clientInDb.NUPN = clientRelation.NUPN;
                clientInDb.RegistrationDate = clientRelation.RegistrationDate;
                clientInDb.FathersFirstName = clientRelation.FathersFirstName;
                clientInDb.FathersSurname = clientRelation.FathersSurname;
                clientInDb.FathersNRC = clientRelation.FathersNRC;
                clientInDb.FatherNAPSANumber = clientRelation.FatherNAPSANumber;
                clientInDb.FatherNationality = clientRelation.FatherNationality;
                clientInDb.IsFatherDeceased = clientRelation.IsFatherDeceased;
                clientInDb.MothersFirstName = clientRelation.MothersFirstName;
                clientInDb.MothersSurname = clientRelation.MothersSurname;
                clientInDb.MothersNRC = clientRelation.MothersNRC;
                clientInDb.MotherNAPSANumber = clientRelation.MotherNAPSANumber;
                clientInDb.MotherNationality = clientRelation.MotherNationality;
                clientInDb.IsMotherDeceased = clientRelation.IsMotherDeceased;
                clientInDb.GuardiansFirstName = clientRelation.GuardiansFirstName;
                clientInDb.GuardiansSurname = clientRelation.GuardiansSurname;
                clientInDb.GuardiansNRC = clientRelation.GuardiansNRC;
                clientInDb.GuardianNAPSANumber = clientRelation.GuardianNAPSANumber;
                clientInDb.GuardianNationality = clientRelation.GuardianNationality;
                clientInDb.GuardianRelationship = clientRelation.GuardianRelationship;
                clientInDb.SpousesLegalName = clientRelation.SpousesLegalName;
                clientInDb.SpousesSurname = clientRelation.SpousesSurname;
                clientInDb.MaritalStatus = clientRelation.MaritalStatus;
                clientInDb.CellphoneCountryCode = clientRelation.CellphoneCountryCode;
                clientInDb.Cellphone = clientRelation.Cellphone;
                clientInDb.OtherCellphoneCountryCode = clientRelation.OtherCellphoneCountryCode;
                clientInDb.OtherCellphone = clientRelation.OtherCellphone;
                clientInDb.NoCellphone = clientRelation.NoCellphone;
                clientInDb.LandlineCountryCode = clientRelation.LandlineCountryCode;
                clientInDb.Landline = clientRelation.Landline;
                clientInDb.Email = clientRelation.Email;
                clientInDb.HouseholdNumber = clientRelation.HouseholdNumber;
                clientInDb.Road = clientRelation.Road;
                clientInDb.Area = clientRelation.Area;
                clientInDb.Landmarks = clientRelation.Landmarks;
                clientInDb.IsZambianBorn = clientRelation.IsZambianBorn;
                clientInDb.BirthPlace = clientRelation.BirthPlace;
                clientInDb.TownName = clientRelation.TownName;
                clientInDb.Religion = clientRelation.Religion;
                clientInDb.HIVStatus = clientRelation.HIVStatus;
                clientInDb.IsDeceased = clientRelation.IsDeceased;
                clientInDb.IsOnART = clientRelation.IsOnART;
                clientInDb.IsAdmitted = clientRelation.IsAdmitted;
                clientInDb.IsDFZClient = clientRelation.IsDFZClient;
                clientInDb.MotherProfileId = clientRelation.MotherProfileId;
                clientInDb.FatherProfileId = clientRelation.FatherProfileId;
                clientInDb.CountryId = clientRelation.CountryId;
                clientInDb.DistrictId = clientRelation.DistrictId;
                clientInDb.HomeLanguageId = clientRelation.HomeLanguageId;
                clientInDb.EducationLevelId = clientRelation.EducationLevelId;
                clientInDb.OccupationId = clientRelation.OccupationId;
                clientInDb.ModifiedIn = clientRelation.ModifiedIn;
                clientInDb.DateModified = DateTime.Now;
                clientInDb.IsSynced = false;
                clientInDb.IsDeleted = false;
                clientInDb.ModifiedBy = clientRelation.ModifiedBy;

                if (await IsClientNRCDuplicate(clientInDb) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                context.ClientRepository.Update(clientInDb);
                await context.SaveChangesAsync();

                var dFZDependentInDb = await context.DFZClientDependentRepository.GetDFZDependentByDFZClientId(clientInDb.Oid);

                if (dFZDependentInDb is null)
                    return NotFound();

                if (await IsDependentHospitalNoDuplicate(dFZDependentInDb) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                dFZDependentInDb.PrincipalId = clientRelation.PrincipalId;
                dFZDependentInDb.DependentClientId = clientInDb.Oid;
                dFZDependentInDb.IsDeleted = false;
                dFZDependentInDb.RelationType = clientRelation.RelationType;
                dFZDependentInDb.IsSynced = false;
                dFZDependentInDb.ModifiedBy = clientInDb.ModifiedBy;
                dFZDependentInDb.DateModified = DateTime.Now;
                dFZDependentInDb.Description = clientRelation.Description;
                dFZDependentInDb.HospitalNo = clientRelation.HospitalNo;
                dFZDependentInDb.DFZPatientTypeId = clientRelation.DFZPatientTypeId;

                context.DFZClientDependentRepository.Update(dFZDependentInDb);
                await context.SaveChangesAsync();

                return CreatedAtAction("UpdateClient", new { key = clientInDb.Oid }, clientInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}{FacilityId}{UserId}", DateTime.Now, "BusinessLayer", "CreateClient", "ClientController.cs", ex.Message, clientRelation.CreatedIn, clientRelation.CreatedIn);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: sc-api/client/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteClient)]
        public async Task<IActionResult> DeleteClient(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);


                var clientInDb = await context.ClientRepository.GetClientByKey(key);

                if (clientInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                clientInDb.DateModified = DateTime.Now;
                clientInDb.IsDeleted = true;
                clientInDb.IsSynced = false;
                clientInDb.IsAdmitted = false;

                context.ClientRepository.Update(clientInDb);
                await context.SaveChangesAsync();

                return Ok(clientInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteClient", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: carepro-api/client-remove-dependent/{key}
        /// </summary>
        /// <param name="key">Client object.</param>
        /// <returns>Http status code: Created At</returns>
        [HttpDelete]
        [Route(RouteConstants.RemoveDFZClientRelation)]
        public async Task<IActionResult> RemoveDFZClientRelation(Guid key)
        {
            try
            {
                var existingDependent = await context.DFZClientDependentRepository.GetDFZDependentByKey(key);

                if (existingDependent == null)
                    return NotFound();

                context.DFZClientDependentRepository.Delete(existingDependent);
                await context.SaveChangesAsync();

                return Ok(); // Or you can return a different status code if needed.
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RemoveDFZClientRelation", "ClientController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the DFZ client HospitalNo is duplicate or not.
        /// </summary>
        /// <param name="dFZClient">DFZ client object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsClientNRCDuplicate(Client client)
        {
            try
            {
                var clientInDb = await context.ClientRepository.GetClientNRC(client.NRC);

                if (clientInDb != null)
                    if (clientInDb.Oid != client.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDistrictDuplicate", "DistrictController.cs", ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Checks whether the DFZ client HospitalNo is duplicate or not.
        /// </summary>
        /// <param name="dFZClient">DFZ client object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsHospitalNoDuplicate(DFZClient dFZClient)
        {
            try
            {
                var dFZClientInDb = await context.DFZClientRepository.GetDFZByHospitalNo(dFZClient.HospitalNo);

                if (dFZClientInDb != null)
                    if (dFZClientInDb.Oid != dFZClient.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsHospitalNoDuplicate", "ClientController.cs", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Checks whether the DFZ client HospitalNo is duplicate or not.
        /// </summary>
        /// <param name="dFZClient">DFZ client object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDependentHospitalNoDuplicate(DFZDependent dFZDependent)
        {
            try
            {
                var dFZdeptClientInDb = await context.DFZClientDependentRepository.GetDependentByHospitalNo(dFZDependent.HospitalNo);

                if (dFZdeptClientInDb != null)
                    if (dFZdeptClientInDb.Oid != dFZdeptClientInDb.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDependentHospitalNoDuplicate", "ClientController.cs", ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Checks whether the DFZ client ServiceNo is duplicate or not.
        /// </summary>
        /// <param name="dFZClient">DFZ client object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsServiceNoDuplicate(DFZClient dFZClient)
        {
            try
            {
                var dFZClientInDb = await context.DFZClientRepository.GetDFZByServiceNo(dFZClient.ServiceNo);

                if (dFZClientInDb != null)
                    if (dFZClientInDb.Oid != dFZClient.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsServiceNoDuplicate", "ClientController.cs", ex.Message);
                throw;
            }
        }
    }
}