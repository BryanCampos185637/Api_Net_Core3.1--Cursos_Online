using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class RolController : MiControllerBase
    {
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta parametros)
        {
            return await Mediador.Send(parametros);
        }
        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(RolEliminar.Ejecuta parametros)
        {
            return await Mediador.Send(parametros);
        }
        [HttpGet("listar")]
        public async Task<ActionResult<List<IdentityRole>>> listar()
        {
            return await Mediador.Send(new RolLista.Ejecuta());
        }
        [HttpPost("agregarRoleUsuario")]
        public async Task<ActionResult<Unit>> agregarRoleUsuario(UsuarioRolAgregar.Ejecuta parametros)
        {
            return await Mediador.Send(parametros);
        }
        [HttpPost("eliminarRoleUsuario")]
        public async Task<ActionResult<Unit>> eliminarRoleUsuario(UsuarioRoleEliminar.Ejecuta parametros)
        {
            return await Mediador.Send(parametros);
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> listar(string username)
        {
            return await Mediador.Send(new ObtenerRolesPorUsuario.Ejecuta { UserName = username });
        }
    }
}
