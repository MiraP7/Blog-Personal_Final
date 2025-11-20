using System;

namespace BlogPersonal.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Entidad { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Detalle { get; set; }

        // Navigation properties
        public Usuario? Usuario { get; set; }
    }
}
