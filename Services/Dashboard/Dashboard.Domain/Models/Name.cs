namespace Dashboard.Domain.Models;

public record Name(string? FirstName, string? MiddleName, string LastName)
{
    public string DisplayName
    {
        get
        {
            var names = new[] { FirstName, MiddleName, LastName }.Where(name => !string.IsNullOrWhiteSpace(name));
            return string.Join(' ', names);
        }
    }
}
