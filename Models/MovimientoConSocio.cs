namespace SociosApp.Models
{
    public class MovimientoConSocio
    {
        public DateTime Fecha { get; set; }
        public string SocioApellido { get; set; } = string.Empty;
        public string SocioNombre { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public decimal? Debe { get; set; }
        public decimal? Haber { get; set; }
    }

}
