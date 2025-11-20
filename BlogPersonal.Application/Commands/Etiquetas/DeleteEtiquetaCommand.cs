using MediatR;

namespace BlogPersonal.Application.Commands.Etiquetas
{
    public class DeleteEtiquetaCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
