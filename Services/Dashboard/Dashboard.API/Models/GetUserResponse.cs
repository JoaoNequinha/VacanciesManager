using Dashboard.Domain.Models;

namespace Dashboard.API.Models;

internal record GetUserResponse
(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string Email,
    string AddressLine1,
    string AddressLine2,
    string AddressLine3,
    string City,
    string PostCode
)
{
    public GetUserResponse(User user) : this
    (
        user.Id,
        user.Name.FirstName ?? string.Empty,
        user.Name.MiddleName ?? string.Empty,
        user.Name.LastName,
        user.Email.ToString(),
        user.Address.Line1,
        user.Address.Line2 ?? string.Empty,
        user.Address.Line3 ?? string.Empty,
        user.Address.City,
        user.Address.Postcode
    )
    { }
}
