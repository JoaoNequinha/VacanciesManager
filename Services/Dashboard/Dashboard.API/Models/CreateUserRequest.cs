using Dashboard.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.API.Models;

public record CreateUserRequest(
    Guid Id,

    [MaxLength(100)]
    string? FirstName,

    [MaxLength(100)]
    string? MiddleName,

    [Required]
    [MaxLength(100)]
    string LastName,

    [Required]
    [EmailAddress]
    [MaxLength(300)]
    string Email,

    [Required]
    [MaxLength(100)]
    string AddressLine1,

    [MaxLength(100)]
    string? AddressLine2,

    [MaxLength(100)]
    string? AddressLine3,

    [Required]
    [MaxLength(100)]
    string City,

    [Required]
    [MaxLength(10)]
    [RegularExpression(@"^[A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}$", ErrorMessage = "The given value is not a valid UK Postal Code")]
    string PostCode
)
{
    public User ToEntity(Guid? id = null)
    {
        return new(
            id ?? Guid.NewGuid(),
            FirstName ?? string.Empty,
            MiddleName ?? string.Empty,
            LastName,
            Email,
            AddressLine1,
            AddressLine2 ?? string.Empty,
            AddressLine3 ?? string.Empty,
            City,
            PostCode
        );
    }
}