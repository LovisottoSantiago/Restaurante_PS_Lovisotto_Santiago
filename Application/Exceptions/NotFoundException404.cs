namespace Application.Exceptions
{
    public class NotFoundException404 : HttpException
    {
        public NotFoundException404(string message)
            : base(404, message) { }
    }
}
