namespace BlogPersonal.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra el rol por defecto durante el registro de usuario.
    /// </summary>
    public class DefaultRoleNotFoundException : Exception
    {
        public DefaultRoleNotFoundException()
            : base("El rol por defecto no se encontró en el sistema. Contacte al administrador.")
        {
        }
    }
}
