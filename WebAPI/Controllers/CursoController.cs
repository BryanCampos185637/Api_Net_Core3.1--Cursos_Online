using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.Paginacion;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : MiControllerBase
    {
        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta data)
        {
            return await Mediador.Send(data);
        }
        [HttpGet]
        public async Task<ActionResult<List<CursosDTO>>> Get()
        {
            return await Mediador.Send(new Consulta.ListaCursos());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CursosDTO>> Detalle(Guid id)
        {
            return await Mediador.Send(new ConsultaId.CursoUnico { Id = id });
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediador.Send(data);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await Mediador.Send(data);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediador.Send(new Eliminar.Ejecuta { Id = id });
        }
    }
}
