namespace Application.Exceptions
{
    public class ConflictException409 : HttpException
    {
        public ConflictException409(string message)
            : base(409, message) { }
    }
}
