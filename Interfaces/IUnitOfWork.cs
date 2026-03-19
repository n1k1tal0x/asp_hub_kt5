using asp_hub_kt5.Models;

namespace asp_hub_kt5.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        Task<int> SaveChangesAsync();
    }
}
