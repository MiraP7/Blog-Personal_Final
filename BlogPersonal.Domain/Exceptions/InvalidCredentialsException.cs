namespace BlogPersonal.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando las credenciales de inicio de sesión son inválidas.
    /// </summary>
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Las credenciales proporcionadas son inválidas. Por favor verifica tu email y contraseña.")
        {
        }
    }
}
