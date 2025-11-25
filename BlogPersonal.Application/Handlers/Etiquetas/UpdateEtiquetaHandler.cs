using AutoMapper;
using BlogPersonal.Application.Commands.Etiquetas;
using BlogPersonal.Application.DTOs;
using BlogPersonal.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Etiquetas
{
    public class UpdateEtiquetaHandler : IRequestHandler<UpdateEtiquetaCommand, EtiquetaDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEtiquetaHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EtiquetaDto?> Handle(UpdateEtiquetaCommand request, CancellationToken cancellationToken)
        {
            var etiqueta = await _context.Etiquetas.FindAsync(new object[] { request.Id }, cancellationToken);

            if (etiqueta == null)
            {
                return null;
            }

            etiqueta.Nombre = request.Nombre;
            etiqueta.Slug = request.Nombre.ToLower().Replace(" ", "-");

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EtiquetaDto>(etiqueta);
        }
    }
}
