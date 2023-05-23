using Dashboard.API.DTO;
using Dashboard.API.ExtensionMethods;
using Dashboard.Domain.Logic;
using Dashboard.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/Projects")]
    public class ProjectController : Controller
    {

        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectController> _projectlogger;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> projectlogger)
        {
            _projectService = projectService;
            _projectlogger = projectlogger;
        }

        [HttpGet("{project_id:int}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProjectDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProject(int project_id, CancellationToken cancellationToken)
        {
            try
            {
                Project project = await _projectService.GetProjectAsync(project_id, cancellationToken);
                return Ok(ProjectMapper.ToProjectDTO(project));
            }
            catch (KeyNotFoundException ex)
            {
                _projectlogger.LogError($"Failed to get project, project ID: {project_id} was not found, " + ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _projectlogger.LogError($"Failed to get project, project ID: {project_id}" + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProjectSummaryDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetClientProjects([FromQuery] int client_id, CancellationToken cancellationToken)
        {
            try
            {
                List<ProjectSummaryDTO> clientProjects = (await _projectService.GetAllProjectsByClient(client_id))
                    .Select(project => ProjectMapper.ToDTO(project)).ToList();

                return Ok(clientProjects);
            }
            catch (Exception ex)
            {
                _projectlogger.LogError($"Failed retrieving Projects for client with ID : {client_id}", ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProject([FromBody] ProjectCreationDTO request)
        {
            Project? newProject = null;

            try
            {
                newProject = request.ToEntity();

            }
            catch (Exception ex)
            {
                _projectlogger.LogError($"Bad request while creating project, ", ex.Message);
                return BadRequest(ex.Message);
            }
            try
            {
                newProject = await _projectService.AddProject(newProject);

                return CreatedAtAction(nameof(GetProject), new { project_id = newProject.Id }, "Project has been created");
            }
            catch (ArgumentException ex)
            {
                _projectlogger.LogError($"Error occured while creating Project, project with name {newProject.Name} already exists");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _projectlogger.LogError("Error occurred while creating Project", ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPut("{project_id:int}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> ModifyProject(int project_id, [FromBody] ModifyProjectDTO modifyProjectDTO)
        {
            try
            {
                Project project = modifyProjectDTO.ToEntity();
                await _projectService.ModifyProject(project_id, project);
                return Ok("Project was modified.");
            }
            catch (KeyNotFoundException ex)
            {
                _projectlogger.LogError($"Failed to modify project, no such project with Id {project_id} was found", ex.Message);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _projectlogger.LogError($"Error occurred for modifying project, name already exists" + ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _projectlogger.LogError($"Error occurred for project ID: {project_id}, " + ex.Message);
                return Problem(ex.Message);
            }
        }
        //TODO   create method for deletion
    }
}
