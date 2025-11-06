using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace SociosApp.Services
{
    public class SessionService
    {
        private readonly IJSRuntime _js;

        public SessionService(IJSRuntime js)
        {
            _js = js;
        }

        private const string KeyUsuario = "usuarioLogueado";

        public async Task GuardarUsuarioAsync(string usuario)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", KeyUsuario, usuario);
        }

        public async Task<string?> ObtenerUsuarioAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", KeyUsuario);
        }

        public async Task CerrarSesionAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", KeyUsuario);
        }
    }
}
