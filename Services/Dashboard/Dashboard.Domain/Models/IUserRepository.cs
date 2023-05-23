using Dashboard.Domain.Seedwork;

namespace Dashboard.Domain.Models;

public interface IUserRepository : IRepository<User>
{
    User Add(User user);
    void Update(User user);
    Task<User> GetAsync(Guid id, CancellationToken cancellationToken);
}