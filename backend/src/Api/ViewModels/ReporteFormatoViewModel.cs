namespace Api.ViewModels
{
    public class ReporteFormatoViewModel
    {
        public int FormatoId { get; set; }
        public int DestinoId { get; set; }
        public DateTime FechaDescongelacion { get; set; }
        public DateTime FechaProduccion { get; set; }
        public string RealizadoPor { get; set; } = string.Empty;
        public string RevisadoPor { get; set; } = string.Empty;
        public string? NumeroCoche { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public decimal? TemperaturaProducto { get; set; }
        public TimeSpan? HoraInicioDescongelado { get; set; }
        public TimeSpan? HoraInicioConsumo { get; set; }
        public TimeSpan? HoraFinConsumo { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }
}
