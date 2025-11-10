using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace SociosApp.Models
{
    [Table("cuentacorriente")]
    public class Cuentacorriente : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("socioid")]
        public int Socioid { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("documento")]
        public string Documento { get; set; } = string.Empty;

        [Column("debe")]
        public decimal? Debe { get; set; }

        [Column("haber")]
        public decimal? Haber { get; set; }

        [Column("detalle")]
        public string Detalle { get; set; } = string.Empty;

        [Column("observaciones")]
        public string Observaciones { get; set; } = string.Empty;

        [Column("pago")]
        public int Pago { get; set; }
    }
}
