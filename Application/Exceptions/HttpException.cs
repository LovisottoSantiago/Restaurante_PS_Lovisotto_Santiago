namespace Application.Exceptions
{
    public abstract class HttpException : Exception
    {
        public int StatusCode { get; }

        protected HttpException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
