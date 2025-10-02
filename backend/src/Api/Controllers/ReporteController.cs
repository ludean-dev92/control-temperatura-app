using Api.ViewModels;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReporteController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("{formatoId}")]
        public async Task<ActionResult<List<ReporteFormatoViewModel>>> GetReporte(int formatoId)
        {
            var resultado = await _reporteService.ObtenerReporteAsync(formatoId);

            var lista = resultado.Select(fila => new ReporteFormatoViewModel
            {
                FormatoId = fila["FormatoId"] != null ? Convert.ToInt32(fila["FormatoId"]) : 0,
                DestinoId = fila["DestinoId"] != null ? Convert.ToInt32(fila["DestinoId"]) : 0,
                FechaDescongelacion = fila["FechaDescongelacion"] != null ? Convert.ToDateTime(fila["FechaDescongelacion"]) : default,
                FechaProduccion = fila["FechaProduccion"] != null ? Convert.ToDateTime(fila["FechaProduccion"]) : default,
                RealizadoPor = fila["RealizadoPor"]?.ToString() ?? string.Empty,
                RevisadoPor = fila["RevisadoPor"]?.ToString() ?? string.Empty,
                NumeroCoche = fila["NumeroCoche"]?.ToString() ?? string.Empty,
                CodigoProducto = fila["CodigoProducto"]?.ToString() ?? string.Empty,
                TemperaturaProducto = fila["TemperaturaProducto"] != null ? Convert.ToDecimal(fila["TemperaturaProducto"]) : (decimal?)null,
                HoraInicioDescongelado = fila["HoraInicioDescongelado"] != null ? (TimeSpan?)fila["HoraInicioDescongelado"] : null,
                HoraInicioConsumo = fila["HoraInicioConsumo"] != null ? (TimeSpan?)fila["HoraInicioConsumo"] : null,
                HoraFinConsumo = fila["HoraFinConsumo"] != null ? (TimeSpan?)fila["HoraFinConsumo"] : null,
                Observaciones = fila["Observaciones"]?.ToString() ?? string.Empty
            }).ToList();


            return Ok(lista);
        }
    }
}
