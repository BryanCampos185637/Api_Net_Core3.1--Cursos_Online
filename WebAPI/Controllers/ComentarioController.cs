using Aplicacion.Comentarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : MiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Nuevo(Nuevo.Ejecuta parametros)
        {
            return await Mediador.Send(parametros);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid Id)
        {
            return await Mediador.Send(new Eliminar.Ejecuta { Id = Id });
        }
    }
}
