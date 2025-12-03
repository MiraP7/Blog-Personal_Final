namespace BlogPersonal.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se intenta registrar un usuario con un nombre de usuario que ya existe.
    /// </summary>
    public class DuplicateUsernameException : Exception
    {
        public string Username { get; }

        public DuplicateUsernameException(string username)
            : base($"El nombre de usuario '{username}' ya está en uso.")
        {
            Username = username;
        }
    }
}
