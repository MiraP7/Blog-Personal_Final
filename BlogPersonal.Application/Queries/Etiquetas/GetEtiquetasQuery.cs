using BlogPersonal.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace BlogPersonal.Application.Queries.Etiquetas
{
    public class GetEtiquetasQuery : IRequest<List<EtiquetaDto>>
    {
    }
}
