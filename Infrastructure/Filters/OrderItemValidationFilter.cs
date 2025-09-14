using Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters
{
    public class OrderItemValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // En caso de meter un ID que no sea un GUID
            if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResult(new ApiError
                {
                    Message = "El plato especificado no existe o no está disponible"
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
