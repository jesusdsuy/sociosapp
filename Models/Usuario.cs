using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SociosApp.Models
{
    [Table("usuario")]
    public class Usuario : BaseModel
    {
        [PrimaryKey("usuarioid")]
        public int UsuarioId { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("clave")]
        public string Clave { get; set; } = string.Empty;

        [Column("nivel")]
        public int Nivel { get; set; }
    }

}
