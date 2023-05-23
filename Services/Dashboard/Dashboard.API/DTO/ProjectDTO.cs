using Dashboard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.API.DTO;

public record ProjectDTO
(
    [Required]
    [MaxLength(100)]
    int id,

    [Required]
    [MaxLength(100)]
     string name,

    [MaxLength(2048)]
    string description,

    [MaxLength(100)]
     string contact,

    [Required]
    [MaxLength(100)]
     string client_name,

    [Required]
    byte[] client_logo,

    [Required]
    [MaxLength(100)]
     int vacancy_count
)
{
}
