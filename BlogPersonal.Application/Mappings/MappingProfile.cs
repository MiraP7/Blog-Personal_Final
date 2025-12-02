using AutoMapper;
using BlogPersonal.Application.DTOs.Posts;
using BlogPersonal.Application.DTOs.Comments;
using BlogPersonal.Application.DTOs;
using BlogPersonal.Domain.Entities;
using System.Linq;

namespace BlogPersonal.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Etiqueta, EtiquetaDto>();

            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.AutorNombre, opt => opt.MapFrom(src => src.Autor.NombreUsuario))
                .ForMember(dest => dest.AutorId, opt => opt.MapFrom(src => src.AutorId))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.EstadoId, opt => opt.MapFrom(src => src.EstadoId))
                .ForMember(dest => dest.Idioma, opt => opt.MapFrom(src => src.Idioma.Nombre))
                .ForMember(dest => dest.ComentariosCount, opt => opt.MapFrom(src => src.Comentarios.Count))
                .ForMember(dest => dest.Categorias, opt => opt.MapFrom(src => src.PostCategorias.Select(pc => pc.Categoria.Nombre).ToList()))
                .ForMember(dest => dest.Etiquetas, opt => opt.MapFrom(src => src.PostEtiquetas.Select(pe => pe.Etiqueta).ToList()));

            CreateMap<Comentario, CommentDto>()
                .ForMember(dest => dest.AutorNombre, opt => opt.MapFrom(src => src.Autor.NombreUsuario));
        }
    }
}
