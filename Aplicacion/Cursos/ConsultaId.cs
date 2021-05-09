using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico: IRequest<CursosDTO>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursosDTO>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;
            public Manejador(CursosOnlineContext _context, IMapper _mapper)
            {
                context=_context;
                mapper = _mapper;
            }
            public async Task<CursosDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context.Curso.Include(p => p.InstructorLink).ThenInclude(p => p.Instructor)
                    .Include(p => p.ComentarioLista)
                    .Include(p => p.PrecioPromocion).FirstOrDefaultAsync(p => p.CursoId.Equals(request.Id));
                if (curso == null) 
                {
                     throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso"});
                }
                var cursoDTO = mapper.Map<Curso, CursosDTO>(curso);
                return  cursoDTO;
            }
        }
    }
}