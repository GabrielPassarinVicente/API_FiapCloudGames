using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Game> Games { get; }
    IRepository<UserGame> UserGames { get; }
    IRepository<Promotion> Promotions { get; }
    Task<int> SaveChangesAsync();
}
