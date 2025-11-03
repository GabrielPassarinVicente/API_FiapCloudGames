using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Data;

namespace FiapCloudGames.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IRepository<User>? _users;
    private IRepository<Game>? _games;
    private IRepository<UserGame>? _userGames;
    private IRepository<Promotion>? _promotions;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<Game> Games => _games ??= new Repository<Game>(_context);
    public IRepository<UserGame> UserGames => _userGames ??= new Repository<UserGame>(_context);
    public IRepository<Promotion> Promotions => _promotions ??= new Repository<Promotion>(_context);
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}
