using Application.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters
{
    public class ValidateSortByPriceFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var query = context.HttpContext.Request.Query;

            if (query.ContainsKey("sortByPrice"))
            {
                var value = query["sortByPrice"].ToString();

                if (!string.IsNullOrEmpty(value) && value != "asc" && value != "desc")
                {
                    context.Result = new BadRequestObjectResult(new ApiError
                    {
                        Message = "Parámetros de ordenamiento inválidos"
                    });
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
