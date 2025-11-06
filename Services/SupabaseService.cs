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



    }
}

