using Dashboard.API.DTO;
using Dashboard.API.ExtensionMethods;
using Dashboard.Domain.Logic;
using Dashboard.Domain.Models;
using Dashboard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;

        public ClientController(ILogger<ClientController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateClient()
        {
            Stream streamImage = null;
            ClientCreationDTO? clientCreationDTO = null;
            byte[] imageData = null;
            try
            {
                streamImage = Request.Form.Files[0].OpenReadStream();
                imageData = new byte[streamImage.Length];
                streamImage.Read(imageData, 0, imageData.Length);
                streamImage.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to convert file to byte array " + ex.Message);
                return BadRequest("Unable to convert uploaded file to byte array" + ex.Message);
            }

            try
            {
                clientCreationDTO = ClientMapper.ToDTO(Request.Form, imageData);
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to map request form to DTO " + ex.Message);
                return BadRequest("Failed to map the request form for creation" + ex.Message);
            }


            Client? client = null;

            try
            {
                client = clientCreationDTO.ToEntity();
            }
            catch (Exception ex)
            {
                _logger.LogError("Bad request while creating client, " + ex.Message);
                return BadRequest(ex.Message);
            }

            try
            {
                client = await _clientService.AddClient(client);
                return CreatedAtAction(nameof(GetClient), new { client_id = client.Id }, "Client was created.");
            }
            //TODO: Custom Error, as ArgumentException is generic error
            catch (ArgumentException ex)
            {
                _logger.LogError("Error creating client, client name already exists. " + ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while adding client, " + ex.Message);
                return Problem(ex.Message);
            }


            return Ok();
        }


        [HttpGet("{client_id:int}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ClientOverviewDetailsDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetClient(int client_id, CancellationToken cancellationToken)
        {
            try
            {
                Client client = await _clientService.GetClientAsync(client_id, cancellationToken);

                return Ok(new ClientOverviewDetailsDTO(client));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Failed to get client, client ID: {client_id} was not found, " + ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get client, for client ID {client_id},", ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ClientDetailsDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetClients()
        {
            try
            {
                List<ClientDetailsDTO> clients = (await _clientService.GetClients())
                    .Select(client => new ClientDetailsDTO(client)).ToList();

                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving clients", ex.Message);
                return Problem(ex.Message);
            }
        }


        [HttpPut("{client_id:int}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> ModifyClient(int client_id)
        {
            Stream streamImage = null;
            ModifyClientDTO? modifyClientDTO = null;
            byte[] imageData = null;
            try
            {
                streamImage = Request.Form.Files[0].OpenReadStream();
                imageData = new byte[streamImage.Length];
                streamImage.Read(imageData, 0, imageData.Length);
                streamImage.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to read uploaded file " + ex.Message);
                return BadRequest("Unable to read file" + ex.Message);
            }

            try
            {
                modifyClientDTO = ClientMapper.ToModifyDTO(Request.Form, imageData);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to map request form to modify DTO " + ex.Message);
                return BadRequest("Failed to map the request form for modification" + ex.Message);
            }

            try
            {
                Client client = modifyClientDTO.ToEntity();
                await _clientService.Modifyclient(client_id, client);
                return Ok("Client has been modified.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Failed to modify client, no such client with Id {client_id} was found", ex.Message);
                return NotFound(ex.Message);
            }
            //TODO: Custom Error, as ArgumentException is generic error
            catch (ArgumentException ex)
            {
                _logger.LogError("Error modifying client, client name already exists. " + ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for client ID {client_id}, " + ex.Message);
                return Problem(ex.Message);
            }
        }

    }
}
