using MediatR;
using BlogPersonal.Application.DTOs;

namespace BlogPersonal.Application.Commands.Etiquetas
{
    public class CreateEtiquetaCommand : IRequest<EtiquetaDto>
    {
        public string Nombre { get; set; } = string.Empty;
    }
}
