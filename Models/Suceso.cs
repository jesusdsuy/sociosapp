namespace SociosApp.Models
{
    public class Suceso
    {
        public int Id { get; set; } 
        public DateTime Fecha { get; set; }
        public string Detalle { get; set; } 
        public Socio Socio { get; set; }
        public Suceso() 
        { 
        Socio = new Socio();
        }

    }
}
