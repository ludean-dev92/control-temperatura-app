using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ReporteService : IReporteService
    {
        private readonly TemperatureDbContext _context;

        public ReporteService(TemperatureDbContext context)
        {
            _context = context;
        }

       public async Task<List<Dictionary<string, object>>> ObtenerReporteAsync(int formatoId)
        {
            var lista = new List<Dictionary<string, object>>();

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "ObtenerReporteFormatos";
            command.CommandType = CommandType.StoredProcedure;

            var param = new SqlParameter("@FormatoId", formatoId);
            command.Parameters.Add(param);

            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var fila = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    fila[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                lista.Add(fila);
            }

            return lista;
        }

    }
}
