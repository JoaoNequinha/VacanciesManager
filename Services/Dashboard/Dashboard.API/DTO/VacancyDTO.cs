using System.ComponentModel.DataAnnotations;

namespace Dashboard.API.DTO;

public record VacancyDTO
(
    [Required]
    int id,

    [Required]
    [MaxLength(100)]
    string name,

    [Required]
    [MaxLength(100)]
    string skill,

    [Required]
    [MaxLength(100)]
    string location,

    [Required]
    [MaxLength(100)]
    string target_start_date,

    [Required]
    int vacancy_count,

    [Required]
    [MaxLength(100)]
    string is_open,

    [Required]
    [MaxLength(100)]
    string project_name,

    [Required]
    [MaxLength(100)]
    string client_name,

    [Required]
    int project_id,

    [Required]
    int client_id
)
{
}