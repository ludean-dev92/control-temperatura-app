using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Formato
    {
        [JsonIgnore]
        public int FormatoId { get; set; }
        public int DestinoId { get; set; } // si lo manejas con tabla Destinos
        public DateTime FechaDescongelacion { get; set; }
        public DateTime FechaProduccion { get; set; }

        public string? RealizadoPor { get; set; }
        public string? RevisadoPor { get; set; }

        public List<Registro> Registros { get; set; } = new();
    }
}
