namespace SociosApp.Models
{
    public class Cuentacorriente
    {
        public int Id { get; set; }
        public int Socioid { get; set; }
      //  public Socio Socio { get; set; }
        public DateTime Fecha { get; set; }
        public string Documento { get; set; }
        public decimal? Debe { get; set; } 
        public decimal? Haber { get; set; }
        public string Detalle {  get; set; }    
        public string Observaciones { get; set; }
        public int Pago { get; set; }
        public Cuentacorriente() 
        { 
        }
    }
}
