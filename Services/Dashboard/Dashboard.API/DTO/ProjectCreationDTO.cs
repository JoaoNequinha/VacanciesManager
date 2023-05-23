using Dashboard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.API.DTO;

public record ProjectCreationDTO
(     
    [Required]
    [MaxLength(100)]
    string name,

    [MaxLength(100)]
     string contact,

    [MaxLength(2048)]
    string description,

    [Required]
     int client_id
)
{
    public Project ToEntity()  
    {
        return new 
            (
                client_id,
                name,
                description ?? string.Empty,
                contact  ?? string.Empty 
            );
    }
}
