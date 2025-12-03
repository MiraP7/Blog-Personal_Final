namespace BlogPersonal.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se intenta registrar un usuario con un email que ya existe.
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        public string Email { get; }

        public UserAlreadyExistsException(string email)
            : base($"El email '{email}' ya está registrado en el sistema.")
        {
            Email = email;
        }
    }
}
