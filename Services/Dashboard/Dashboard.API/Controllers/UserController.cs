using Dashboard.API.Models;
using Dashboard.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dashboard.API.Controllers;

[ApiController]
[Route("api/v1/users")]
public partial class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("{userId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(GetUserResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            User user = await _userRepository.GetAsync(userId, cancellationToken);
            
            return Ok(new GetUserResponse(user));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError("User not found", ex);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed retrieving user", ex);
            return Problem();
        }
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        User? user = null;

        try
        {
            user = request.ToEntity();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid {nameof(CreateUserRequest)}", ex);
            return BadRequest();
        }

        try
        {
            _userRepository.Add(user);
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, null);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed creating user", ex);
            return Problem();
        }
    }
}