using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFormatoService
    {
        Task<IEnumerable<Formato>> GetAllAsync();
        Task<Formato> CreateAsync(Formato formato);
    }
  
}
