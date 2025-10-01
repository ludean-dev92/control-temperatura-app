using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class FormatoService : IFormatoService
    {
        private readonly TemperatureDbContext _context;

        public FormatoService(TemperatureDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Formato>> GetAllAsync()
        {
            return await _context.Formatos.Include(f => f.Registros).ToListAsync();
        }

        public async Task<Formato> CreateAsync(Formato formato)
        {
            _context.Formatos.Add(formato);
            await _context.SaveChangesAsync();
            return formato;
        }
    }
}
