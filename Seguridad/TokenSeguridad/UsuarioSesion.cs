using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Seguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor _httpContextAccesor)
        {
            httpContextAccessor = _httpContextAccesor;
        }
        public string ObtenerUsuarioSesion()
        {
            var userName = httpContextAccessor.HttpContext.User?.Claims?.
                FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}