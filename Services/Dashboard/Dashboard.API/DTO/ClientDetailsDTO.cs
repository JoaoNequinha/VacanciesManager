using Dashboard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.API.DTO;

internal record ClientDetailsDTO
(
    [Required]
    int id,

    [Required]
    [MaxLength(100)]
    string name,

    byte[] client_logo,

    [Required]
    int project_count,

    [Required]
    int vacancy_count
)
{
    public ClientDetailsDTO(Client client) : this
        (
        client.Id,
        client.Name,
        client.ClientLogo ?? null,
        client.ProjectCount,
        client.VacancyCount
        )
    { }
}
