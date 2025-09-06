using Application.Models;
using Application.Response;
using FluentAssertions;
using MyCustomTests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace MyCustomTests
{
    public class DishControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public DishControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Dish_Should_Return_201_When_Valid()
        {
            var request = new DishRequest
            {
                Name = "Pizza TEST 2",
                Description = "Pizza creada con un Test",
                Price = 900.50m,
                Category = 1,
                Image = "https://restaurant.com/images/pizza-test.jpg"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var dish = await response.Content.ReadFromJsonAsync<DishResponse>();
            dish.Should().NotBeNull();
            dish!.Name.Should().Be(request.Name);
            dish.Price.Should().Be(request.Price);
        }

        [Fact]
        public async Task Post_Dish_Should_Return_400_When_Name_Is_Empty()
        {
            var request = new DishRequest
            {
                Name = "",
                Description = "Sin nombre",
                Price = 500,
                Category = 1
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El nombre del plato es obligatorio");
        }

        [Fact]
        public async Task Post_Dish_Should_Return_400_When_Price_Is_Zero()
        {
            var request = new DishRequest
            {
                Name = "Plato inválido",
                Price = 0,
                Category = 1
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El precio debe ser mayor a cero");
        }

        [Fact]
        public async Task Post_Dish_Should_Return_400_When_Category_Not_Exists()
        {
            var request = new DishRequest
            {
                Name = "Plato categoría inválida",
                Price = 300,
                Category = 999 // categoría inexistente
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("La categoría debe existir en el sistema");
        }

        [Fact]
        public async Task Post_Dish_Should_Return_409_When_Name_Already_Exists()
        {
            var request = new DishRequest
            {
                Name = "Tarta de Verdura",
                Price = 850,
                Category = 1
            };

            // Primero lo creo
            await _client.PostAsJsonAsync("/api/v1/Dish", request);

            // Intento crearlo de nuevo
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Ya existe un plato con ese nombre");
        }

        [Fact]
        public async Task Get_Dish_Should_Return_400_When_Sort_Invalid()
        {
            var response = await _client.GetAsync("/api/v1/Dish?sortByPrice=invalid");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Parámetros de ordenamiento inválidos");
        }
    }
}
