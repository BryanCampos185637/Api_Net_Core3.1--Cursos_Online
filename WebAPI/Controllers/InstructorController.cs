using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class InstructorController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            return await Mediador.Send(new Consulta.Lista());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerPorId(Guid id)
        {
            return await Mediador.Send(new ConsultaId.Ejecuta { Id = id });
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediador.Send(data);
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid Id, Editar.Ejecuta data)
        {
        data.InstructorId = Id;
            return await Mediador.Send(data);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid Id)
        {
            return await Mediador.Send(new Eliminar.Ejecuta { Id = Id });
        }
    }
}
