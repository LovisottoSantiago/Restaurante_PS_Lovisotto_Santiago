using Application.Exceptions;
using Application.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpException httpEx)
            {
                context.Result = new ObjectResult(new ApiError { Message = httpEx.Message })
                {
                    StatusCode = httpEx.StatusCode
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
