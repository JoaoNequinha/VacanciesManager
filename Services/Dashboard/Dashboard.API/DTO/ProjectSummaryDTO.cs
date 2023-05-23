using Dashboard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.API.DTO;

public record ProjectSummaryDTO
(
    [Required]
    [MaxLength(100)]
    int id,

    [Required]
    [MaxLength(100)]
    string name,

    [Required]
    [MaxLength(100)]
    int vacancy_count,

    [MaxLength(2048)]
     string description
)
{
}


