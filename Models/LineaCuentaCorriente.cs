using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SociosApp.Models
{
    public class LineaCuentaCorriente
    {
      public DateTime Fecha { get; set; }
      public string Documento { get; set; } = string.Empty;
      public string Detalle { get; set; } = string.Empty;
      public string Observaciones { get; set; } = string.Empty;
      public decimal? Debe { get; set; }
      public decimal? Haber { get; set; }
      public decimal? Saldo { get; set; }
    }
}
