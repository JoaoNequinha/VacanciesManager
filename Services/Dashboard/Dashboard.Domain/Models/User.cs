using Dashboard.Domain.Seedwork;

namespace Dashboard.Domain.Models;

public class User : Entity
{
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }

    #pragma warning disable CS8618 // Only for Entity Framework Binding only
    private User() { }

    public User(
        Guid id,
        string? firstName,
        string? middleName,
        string lastName,
        string email,
        string addressLine1,
        string? addressLine2,
        string? addressLine3,
        string city,
        string postCode
    )
    {
        Id = id;
        Name = new Name(firstName, middleName, lastName);
        Email = new Email(email);
        Address = new Address(addressLine1, addressLine2, addressLine3, city, postCode);
    }
}
