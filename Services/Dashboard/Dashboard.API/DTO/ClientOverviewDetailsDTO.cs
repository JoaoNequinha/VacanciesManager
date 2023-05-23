using Dashboard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.API.DTO;

internal record ClientOverviewDetailsDTO
(
    [Required]
    [MaxLength(100)]
    int id,

    [Required]
    [MaxLength(100)]
    string name,

    byte[] client_logo,

    [Required]
    [MaxLength(100)]
    int vacancy_count,

    [MaxLength(100)]
    string account_manager,

    [MaxLength(2048)]
    string description
)
{
    public ClientOverviewDetailsDTO(Client client) : this
        (
        client.Id,
        client.Name,
        client.ClientLogo ?? null,
        client.VacancyCount,
        client.AccountManager ?? String.Empty,
        client.Description ?? String.Empty
        )
    { }
  
}
