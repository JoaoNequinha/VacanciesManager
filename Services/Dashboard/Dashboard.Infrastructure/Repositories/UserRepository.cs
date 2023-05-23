using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DashboardContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public UserRepository(DashboardContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public User Add(User user) => _context.Users.Add(user).Entity;

    public async Task<User> GetAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.Users.Include(x => x.Address).FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? _context.Users.Local.FirstOrDefault(u => u.Id == id)
            ?? throw new KeyNotFoundException();

    public void Update(User user) => _context.Entry(user).State = EntityState.Modified;
}
