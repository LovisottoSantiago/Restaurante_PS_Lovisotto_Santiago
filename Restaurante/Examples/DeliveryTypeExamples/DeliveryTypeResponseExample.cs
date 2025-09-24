using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DeliveryTypeExamples
{
    public class DeliveryTypeResponseExample : IExamplesProvider<IReadOnlyList<GenericResponse>>
    {
        public IReadOnlyList<GenericResponse> GetExamples()
        {
            return new List<GenericResponse>
            {
                new GenericResponse { Id = 1, Name = "Delivery" },
                new GenericResponse { Id = 2, Name = "Retiro en local" },
                new GenericResponse { Id = 3, Name = "Comida en el local" }
            };
        }
    }
}
