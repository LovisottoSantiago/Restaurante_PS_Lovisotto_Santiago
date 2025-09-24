using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.StatusExamples
{
    public class StatusResponseExample : IExamplesProvider<IReadOnlyList<GenericResponse>>
    {
        public IReadOnlyList<GenericResponse> GetExamples()
        {
            return new List<GenericResponse>
            {
                new GenericResponse { Id = 1, Name = "Pendiente" },
                new GenericResponse { Id = 2, Name = "En preparación" },
                new GenericResponse { Id = 3, Name = "Listo" },
                new GenericResponse { Id = 4, Name = "Entregado" },
                new GenericResponse { Id = 5, Name = "Cancelado" }
            };
        }
    }
}
