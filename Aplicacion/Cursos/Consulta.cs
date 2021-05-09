using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Dominio;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursosDTO>> { }
        public class Manejador : IRequestHandler<ListaCursos, List<CursosDTO>>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper Imapper;
            public Manejador(CursosOnlineContext cursosOnlineContext, IMapper mapper)
            {
                context = cursosOnlineContext;
                Imapper = mapper;
            }
            public async Task<List<CursosDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context.Curso.Include(p => p.InstructorLink).ThenInclude(p => p.Instructor)
                    .Include(p => p.ComentarioLista)
                    .Include(p => p.PrecioPromocion).ToListAsync();
                var cursosDTO = Imapper.Map<List<Curso>, List<CursosDTO>>(cursos);
                return cursosDTO;
            }
        }
    }
}