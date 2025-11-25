using MediatR;
using BlogPersonal.Application.DTOs;

namespace BlogPersonal.Application.Commands.Etiquetas
{
    public class UpdateEtiquetaCommand : IRequest<EtiquetaDto?>
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
