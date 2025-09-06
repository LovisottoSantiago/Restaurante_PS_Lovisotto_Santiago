namespace Application.Exceptions
{
    public class BadRequestException400 : HttpException
    {
        public BadRequestException400(string message)
            : base(400, message) { }
    }
}
