namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}