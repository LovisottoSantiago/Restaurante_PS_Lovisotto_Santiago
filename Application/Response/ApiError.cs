namespace Application.Response
{
    /// <summary>
    /// Representa un error genérico en la API.
    /// </summary>
    public class ApiError
    {
        /// <example>Ya existe un plato con ese nombre</example>
        public string Message { get; set; } = string.Empty;
    }
}
