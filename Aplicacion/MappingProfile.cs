using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using System.Linq;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursosDTO>()
            .ForMember(p => p.Instructores, y => y.MapFrom(p => p.InstructorLink.Select(p => p.Instructor).ToList()))
                .ForMember(p => p.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
                .ForMember(p => p.Precio, y => y.MapFrom(z => z.PrecioPromocion));
            CreateMap<CursoInstructor, CursoInstructorDTO>();
            CreateMap<Instructor, InstructorDTO>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<Precio, PrecioDTO>();
        }
    }
}
