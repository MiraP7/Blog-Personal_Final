using AutoMapper;
using BlogPersonal.Application.DTOs;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Application.Queries.Etiquetas;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Etiquetas
{
    public class GetEtiquetasHandler : IRequestHandler<GetEtiquetasQuery, List<EtiquetaDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEtiquetasHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EtiquetaDto>> Handle(GetEtiquetasQuery request, CancellationToken cancellationToken)
        {
            var etiquetas = await _context.Etiquetas.ToListAsync(cancellationToken);
            return _mapper.Map<List<EtiquetaDto>>(etiquetas);
        }
    }
}
