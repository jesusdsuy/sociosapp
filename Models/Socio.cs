
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SociosApp.Models
{
    [Table("socio")]
    public class Socio : BaseModel
    {
        [PrimaryKey("socioid", false)]
        public int Socioid { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("apellido")]
        public string Apellido { get; set; }

        [Column("telefono")]
        public string Telefono { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "A";
    }
}

