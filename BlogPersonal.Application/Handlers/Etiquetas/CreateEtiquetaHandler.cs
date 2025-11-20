using AutoMapper;
using BlogPersonal.Application.Commands.Etiquetas;
using BlogPersonal.Application.DTOs;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Etiquetas
{
    public class CreateEtiquetaHandler : IRequestHandler<CreateEtiquetaCommand, EtiquetaDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateEtiquetaHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EtiquetaDto> Handle(CreateEtiquetaCommand request, CancellationToken cancellationToken)
        {
            var etiqueta = new Etiqueta
            {
                Nombre = request.Nombre,
                Slug = request.Nombre.ToLower().Replace(" ", "-") // Simple slug generation
            };

            _context.Etiquetas.Add(etiqueta);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EtiquetaDto>(etiqueta);
        }
    }
}
