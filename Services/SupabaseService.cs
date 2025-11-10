using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;
using SociosApp.Models;
using Supabase;
using Supabase.Gotrue;
using Supabase.Postgrest;

namespace SociosApp.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService(SupabaseSettings settings)
        {
            var options = new SupabaseOptions { AutoConnectRealtime = false };
            _client = new Supabase.Client(settings.Url, settings.AnonKey, options);
        }

        public async Task<List<Socio>> GetSociosAsync()
        {
            var response = await _client.From<Socio>().Get();
            return response.Models;
        }
        public async Task<List<Socio>> BuscarSociosAsync(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return new List<Socio>();

            var resp = await _client
                .From<Socio>()
                .Where(x => x.Nombre.Contains(texto) || x.Apellido.Contains(texto))
                .Get();

            return resp.Models;
        }

        public async Task<bool> InsertSocioAsync(Socio nuevoSocio)
        {
            try
            {
                var response = await _client.From<Socio>().Insert(nuevoSocio);
                return response.Models.Any(); // true si insertó correctamente
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insertando socio: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSocioAsync(Socio socio)
        {
            var result = await _client.From<Socio>().Where(x => x.Socioid == socio.Socioid).Update(socio);
            return result.Models.Count > 0;
        }

        public async Task<bool> DeleteSocioAsync(int id)
        {
            try
            {
                await _client.From<Socio>().Where(x => x.Socioid == id).Delete();

                // Si llega hasta acá sin lanzar excepción, se asume que se eliminó correctamente
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar socio: {ex.Message}");
                return false;
            }
        }

        public async Task<Usuario?> GetUsuarioAsync(string nombre, string clave)
        {
            Console.WriteLine($"Buscando usuario: {nombre}, clave: {clave}");

            var response = await _client
              .From<Usuario>()
            .Where(x => x.Nombre == nombre && x.Clave == clave)
            .Get();

            return response.Models.FirstOrDefault();
        }
        //aca la parte ctacte

            // 🔹 Obtener todos los movimientos
            public async Task<List<Cuentacorriente>> GetCuentaCorrienteAsync()
            {
                var response = await _client.From<Cuentacorriente>().Get();
                return response.Models;
            }

            // 🔹 Obtener movimientos de un socio específico
         
        public async Task<List<Cuentacorriente>> GetCuentaCorrienteBySocioAsync(int socioId)
        {
            var resp = await _client
                .From<Cuentacorriente>()
                .Where(x => x.Socioid == socioId)
                .Order(x => x.Fecha, Supabase.Postgrest.Constants.Ordering.Descending)
                .Get();

            return resp.Models;
        }
        // 🔹 Insertar nuevo movimiento
        public async Task<bool> InsertMovimientoAsync(Cuentacorriente mov)
        {
            try
            {
                await _client.From<Cuentacorriente>().Insert(mov);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar movimiento: {ex.Message}");
                return false;
            }
        }


        // 🔹 Actualizar movimiento existente
        public async Task<bool> UpdateMovimientoAsync(Cuentacorriente mov)
        {
            try
            {
                await _client
                    .From<Cuentacorriente>()
                    .Where(x => x.Id == mov.Id)
                    .Update(mov);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar movimiento: {ex.Message}");
                return false;
            }
        }


        // 🔹 Eliminar movimiento
        public async Task<bool> BorrarMovimientoAsync(int id)
        {
            try
            {
                await _client
                    .From<Cuentacorriente>()
                    .Where(x => x.Id == id)
                    .Delete();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al borrar movimiento: {ex.Message}");
                return false;
            }
        }
        public async Task<List<LineaCuentaCorriente>> ObtenerEstadoDeCuentaAsync(int socioId)
        {
            var lista = new List<LineaCuentaCorriente>();

            try
            {
                var movimientos = await _client
                    .From<Cuentacorriente>()
                    .Where(x => x.Socioid == socioId)
                    .Order(x => x.Fecha, Supabase.Postgrest.Constants.Ordering.Ascending)
                    .Get();

                decimal saldo = 0;

                foreach (var mov in movimientos.Models)
                {
                    saldo += (mov.Debe ?? 0) - (mov.Haber ?? 0);

                    lista.Add(new LineaCuentaCorriente
                    {
                        Fecha = mov.Fecha,
                        Documento = mov.Documento,
                        Detalle = mov.Detalle,
                        Observaciones = mov.Observaciones,
                        Debe = mov.Debe,
                        Haber = mov.Haber,
                        Saldo = saldo
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener estado de cuenta: {ex.Message}");
            }

            return lista;
        }

        public async Task<List<(string Apellido, string Nombre, decimal TotalDebe, decimal TotalHaber)>> ObtenerSaldosAsync()
        {
            // Traemos todos los socios
            var socios = await _client.From<Socio>().Get();
            var movimientos = await _client.From<Cuentacorriente>().Get();

            var lista = (from s in socios.Models
                         join m in movimientos.Models on s.Socioid equals m.Socioid into grp
                         select new
                         {
                             s.Apellido,
                             s.Nombre,
                             TotalDebe = grp.Sum(x => x.Debe ?? 0),
                             TotalHaber = grp.Sum(x => x.Haber ?? 0)
                         })
                         .Select(x => (x.Apellido, x.Nombre, x.TotalDebe, x.TotalHaber))
                         .OrderBy(x => x.Apellido)
                         .ToList();

            return lista;
        }
        public async Task<List<MovimientoConSocio>> GetMovimientosEntreFechasConSocioAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var movimientos = await _client
                .From<Cuentacorriente>()
                .Where(x => x.Fecha >= fechaDesde && x.Fecha <= fechaHasta)
                .Order(x => x.Fecha, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            var socios = await _client
                .From<Socio>()
                .Get();

            // Hacemos un "join" local en memoria
            var lista = (from mov in movimientos.Models
                         join s in socios.Models on mov.Socioid equals s.Socioid into grp
                         from socio in grp.DefaultIfEmpty()
                         select new MovimientoConSocio
                         {
                             Fecha = mov.Fecha,
                             Documento = mov.Documento,
                             Detalle = mov.Detalle,
                             Observaciones = mov.Observaciones,
                             Debe = mov.Debe,
                             Haber = mov.Haber,
                             SocioApellido = socio?.Apellido ?? "",
                             SocioNombre = socio?.Nombre ?? ""
                         }).ToList();

            return lista;
        }



        //******************
    }
}

