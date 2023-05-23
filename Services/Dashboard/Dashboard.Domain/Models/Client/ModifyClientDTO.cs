using Dashboard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Domain.Models;

public record ModifyClientDTO
(
    [Required]
    [MaxLength(100)]
    string name,

    [MaxLength(100)]
    string account_manager,

    [MaxLength(600)]
    string description,

    byte[] client_logo
)
{
    public Client ToEntity()
{
    return new(
        this.name,
        this.account_manager,
        this.description,
        this.client_logo,
        0,
        0
        );
}
}

    