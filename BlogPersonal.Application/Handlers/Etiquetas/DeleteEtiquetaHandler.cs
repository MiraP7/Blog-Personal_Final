using BlogPersonal.Application.Commands.Etiquetas;
using BlogPersonal.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Etiquetas
{
    public class DeleteEtiquetaHandler : IRequestHandler<DeleteEtiquetaCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteEtiquetaHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteEtiquetaCommand request, CancellationToken cancellationToken)
        {
            var etiqueta = await _context.Etiquetas.FindAsync(new object[] { request.Id }, cancellationToken);

            if (etiqueta == null)
            {
                return false;
            }

            _context.Etiquetas.Remove(etiqueta);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
