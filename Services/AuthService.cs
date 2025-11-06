using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace SociosApp.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;

        private const string KeyUsuario = "usuarioLogueado";

        public AuthService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> EstaLogueadoAsync()
        {
            var usuario = await _js.InvokeAsync<string>("localStorage.getItem", KeyUsuario);
            return !string.IsNullOrEmpty(usuario);
        }

        public async Task GuardarUsuarioAsync(string usuario)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", KeyUsuario, usuario);
        }

        public async Task<string?> ObtenerUsuarioAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", KeyUsuario);
        }

        public async Task CerrarSesionAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", KeyUsuario);
        }
    }
}

