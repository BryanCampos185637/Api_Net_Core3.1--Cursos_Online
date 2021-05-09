using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {
        
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>>Login(Login.Ejecuta parametros)
        {
            return await Mediador.Send(parametros);
        }
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta parametros)
        {
            return await Mediador.Send(parametros);
        }
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await Mediador.Send(new UsuarioActual.Ejecuta ());
        }
        
    }
}