using Dashboard.API.DTO;
using Dashboard.Domain.Logic;
using Dashboard.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/vacancies")]
    public class VacancyController : ControllerBase
    {
        private readonly ILogger<VacancyController> _logger;
        private readonly IVacancyService _vacancyService;

        public VacancyController(ILogger<VacancyController> logger, IVacancyService vacancyService)
        {
            _logger = logger;
            _vacancyService = vacancyService;
        }

        [HttpGet("{vacancy_id:int}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(VacancyDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetVacancy(int vacancy_id, CancellationToken cancellationToken)
        {
            try
            {
                Vacancy vacancy = await _vacancyService.GetVacancyAsync(vacancy_id, cancellationToken);

                return Ok(vacancy.ToDTO());
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Failed to get vacancy, vacancy with ID: {vacancy_id} was not found, " + ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception has occured while getting vacancy with ID: {vacancy_id},", ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<VacancyDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetVacancies([FromQuery] int project_id)
        {
            try
            {
                List<VacancyDTO> vacancies = new List<VacancyDTO>();

                if (project_id != 0)
                {
                    vacancies = (await _vacancyService.GetAllVacanciesByProject(project_id))
                        .Select(vacancy => vacancy.ToDTO()).ToList();
                }
                else
                {
                    vacancies = (await _vacancyService.GetAllVacancies())
                        .Select(vacancy => vacancy.ToDTO()).ToList();
                }

                return Ok(vacancies);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieving vacancies", ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
