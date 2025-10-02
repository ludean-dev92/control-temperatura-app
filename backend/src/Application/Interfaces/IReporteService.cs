using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReporteService
    {
        Task<List<Dictionary<string, object>>> ObtenerReporteAsync(int formatoId);
    }
}
