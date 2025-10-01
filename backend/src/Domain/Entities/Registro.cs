using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Registro
    {
        public int RegistroId { get; set; }

        // Relación con Formato
        public int FormatoId { get; set; }
        [JsonIgnore]
        public Formato? Formato { get; set; }

        public string? NumeroCoche { get; set; }
        public string? CodigoProducto { get; set; }

        public TimeSpan? HoraInicioDescongelado { get; set; }
        public decimal? TemperaturaProducto { get; set; }
        public TimeSpan? HoraInicioConsumo { get; set; }
        public TimeSpan? HoraFinConsumo { get; set; }

        public string? Observaciones { get; set; }
    }
}
