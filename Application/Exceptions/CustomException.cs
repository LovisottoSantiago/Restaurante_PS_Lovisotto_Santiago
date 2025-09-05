namespace Application.Exceptions
{
    public class CustomException : Exception
    {
        public List<string> Errors { get; }

        public CustomException(List<string> errors)
            : base("Errores de validación")
        {
            Errors = errors;
        }
    }
}
