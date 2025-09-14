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

        // ---------- 1) SEED TEST ----------

        [Fact]
        public async Task Seed_Database_With_Sample_Dishes()
        {
            var dishesToInsert = new List<DishRequest>
            {
                new() { Name = "Pizza Fugazzeta", Description = "Pizza rellena de queso mozzarella con cubierta de cebolla y orégano", Price = 1450.00m, Category = 6, Image = "https://restaurant.com/images/pizza-fugazzeta.jpg" },
                new() { Name = "Pizza Calabresa", Description = "Pizza con mozzarella y rodajas de longaniza calabresa", Price = 1550.00m, Category = 6, Image = "https://restaurant.com/images/pizza-calabresa.jpg" },
                new() { Name = "Hamburguesa Clásica", Description = "Medallón de carne vacuna, queso cheddar, lechuga, tomate y mayonesa", Price = 1100.00m, Category = 3, Image = "https://restaurant.com/images/hamburguesa-clasica.jpg" },
                new() { Name = "Lasagna Bolognesa", Description = "Capas de pasta con salsa bolognesa, bechamel y queso gratinado", Price = 1600.00m, Category = 4, Image = "https://restaurant.com/images/lasagna-bolognesa.jpg" },
                new() { Name = "Ravioles de Ricotta", Description = "Ravioles caseros rellenos de ricotta y espinaca, con salsa fileto", Price = 1400.00m, Category = 4, Image = "https://restaurant.com/images/ravioles-ricotta.jpg" },
                new() { Name = "Asado de Tira", Description = "Clásico corte de carne asado a la parrilla, acompañado de papas fritas", Price = 2200.00m, Category = 5, Image = "https://restaurant.com/images/asado-tira.jpg" },
                new() { Name = "Tarta de Verdura", Description = "Tarta casera rellena de acelga, ricota y huevo, acompañada con ensalada mixta", Price = 1000.00m, Category = 3, Image = "https://restaurant.com/images/tarta-verdura.jpg" },
                new() { Name = "Provoleta a la Parrilla", Description = "Queso provoleta fundido con orégano y aceite de oliva", Price = 850.00m, Category = 5, Image = "https://restaurant.com/images/provoleta-parrilla.jpg" },
                new() { Name = "Ensalada Caprese", Description = "Rodajas de tomate, mozzarella fresca, albahaca y aceite de oliva extra virgen", Price = 900.00m, Category = 2, Image = "https://restaurant.com/images/ensalada-caprese.jpg" },
                new() { Name = "Ensalada César", Description = "Lechuga romana, pollo grillado, crutones, queso parmesano y aderezo césar", Price = 950.00m, Category = 2, Image = "https://restaurant.com/images/ensalada-cesar.jpg" },
                new() { Name = "Flan Casero", Description = "Flan de huevo casero con dulce de leche y crema chantilly", Price = 600.00m, Category = 10, Image = "https://restaurant.com/images/flan-casero.jpg" },
                new() { Name = "Lomito Completo", Description = "Sandwich de lomo con jamón, queso, lechuga, tomate, huevo y papas fritas", Price = 1350.00m, Category = 7, Image = "https://restaurant.com/images/lomito-completo.jpg" },
                new() { Name = "Milanesa Napolitana", Description = "Milanesa de carne con salsa de tomate, jamón, queso mozzarella y orégano", Price = 1200.00m, Category = 3, Image = "https://restaurant.com/images/milanesa-napolitana.jpg" }
            };

            foreach (var dish in dishesToInsert)
            {
                var response = await _client.PostAsJsonAsync("/api/v1/Dish", dish);
                response.StatusCode.Should().Be(HttpStatusCode.Created);

                var created = await response.Content.ReadFromJsonAsync<DishResponse>();
                created.Should().NotBeNull();
                created!.Name.Should().Be(dish.Name);
                created.Price.Should().Be(dish.Price);
            }
        }

        // ---------- 2-6) POST TESTS ----------

        [Fact]
        public async Task Post_Should_Return_201_When_New_Valid_Dish()
        {
            var request = new DishRequest
            {
                Name = "Pizza Test Nueva",
                Description = "Pizza de prueba creada en test",
                Price = 1000m,
                Category = 6,
                Image = "https://restaurant.com/images/pizza-test.jpg"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_Should_Return_409_When_Duplicate_Name()
        {
            var request = new DishRequest { Name = "Pizza Fugazzeta", Price = 1500m, Category = 6 };
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("Ya existe un plato con ese nombre");
        }

        [Fact]
        public async Task Post_Should_Return_400_When_Name_Empty()
        {
            var request = new DishRequest { Name = "", Price = 500m, Category = 6 };
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("El nombre del plato es obligatorio");
        }

        [Fact]
        public async Task Post_Should_Return_400_When_Price_Invalid()
        {
            var request = new DishRequest { Name = "Pizza Precio Inválido", Price = -1m, Category = 6 };
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("El precio debe ser mayor a cero");
        }

        [Fact]
        public async Task Post_Should_Return_400_When_Category_Not_Exists()
        {
            var request = new DishRequest { Name = "Pizza Cat Inválida", Price = 800m, Category = 999 };
            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("La categoría debe existir en el sistema");
        }

        // ---------- 7-11) GET TESTS ----------

        [Fact]
        public async Task Get_Should_Return_200_With_All_Dishes()
        {
            var response = await _client.GetAsync("/api/v1/Dish");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            dishes.Should().NotBeNull();
            dishes!.Should().Contain(d => d.Name == "Pizza Fugazzeta");
        }

        [Fact]
        public async Task Get_Should_Return_200_Filter_By_Name()
        {
            var response = await _client.GetAsync("/api/v1/Dish?name=pizza");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            dishes!.Should().OnlyContain(d => d.Name.Contains("Pizza", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task Get_Should_Return_200_Sorted_By_Price_Asc()
        {
            var response = await _client.GetAsync("/api/v1/Dish?sortByPrice=asc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            dishes!.Should().BeInAscendingOrder(d => d.Price);
        }

        [Fact]
        public async Task Get_Should_Return_400_When_Sort_Invalid()
        {
            var response = await _client.GetAsync("/api/v1/Dish?sortByPrice=invalid");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("Parámetros de ordenamiento inválidos");
        }

        [Fact]
        public async Task Get_Should_Return_200_Filter_By_Category()
        {
            var response = await _client.GetAsync("/api/v1/Dish?category=4"); // Pastas
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            dishes!.Should().OnlyContain(d => d.Category.Id == 4);
        }

        // ---------- 12-15) PUT TESTS ----------

        [Fact]
        public async Task Put_Should_Return_200_When_Update_Valid()
        {
            var all = await _client.GetFromJsonAsync<List<DishResponse>>("/api/v1/Dish");
            var dish = all!.First(d => d.Name == "Milanesa Napolitana");

            var update = new DishUpdateRequest
            {
                Name = "Milanesa Napolitana Premium",
                Description = "Milanesa clásica con salsa de tomate, jamón, queso y huevo frito",
                Price = 1400m,
                Category = dish.Category.Id,
                Image = "https://restaurant.com/images/milanesa-napolitana-premium.jpg",
                IsActive = true
            };

            var response = await _client.PutAsJsonAsync($"/api/v1/Dish/{dish.Id}", update);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await response.Content.ReadFromJsonAsync<DishResponse>();
            updated!.Name.Should().Be(update.Name);
        }

        [Fact]
        public async Task Put_Should_Return_400_When_Price_Invalid()
        {
            // Creo primero el plato que voy a testear
            var create = new DishRequest
            {
                Name = "Lasagna Bolognesa Garfield",
                Description = "La favorita de Garfield: capas infinitas de pasta, salsa bolognesa casera, bechamel cremosa y mucho, pero mucho queso gratinado.",
                Price = 1600m,
                Category = 4,
                Image = "https://restaurant.com/images/lasagna-bolognesa-garfield.jpg"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Dish", create);
            postResponse.EnsureSuccessStatusCode();

            var dish = await postResponse.Content.ReadFromJsonAsync<DishResponse>();

            // Intento actualizar con precio inválido
            var update = new DishUpdateRequest
            {
                Name = dish!.Name,
                Description = dish.Description,
                Image = dish.Image,
                Price = -10,
                Category = dish.Category.Id,
                IsActive = true
            };

            var response = await _client.PutAsJsonAsync($"/api/v1/Dish/{dish.Id}", update);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("El precio debe ser mayor a cero");
        }


        [Fact]
        public async Task Put_Should_Return_404_When_Dish_Not_Found()
        {
            var update = new DishUpdateRequest
            {
                Name = "Plato Fantasma",
                Price = 500,
                Category = 6,
                IsActive = true
            };

            var response = await _client.PutAsJsonAsync($"/api/v1/Dish/{Guid.NewGuid()}", update);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("Plato no encontrado");
        }

        [Fact]
        public async Task Put_Should_Return_409_When_Name_Duplicate()
        {
            var all = await _client.GetFromJsonAsync<List<DishResponse>>("/api/v1/Dish");
            var dish1 = all!.First(d => d.Name == "Pizza Calabresa");
            var dish2 = all!.First(d => d.Name == "Pizza Fugazzeta");

            var update = new DishUpdateRequest
            {
                Name = dish1.Name, // intento duplicar
                Price = 2000,
                Category = dish2.Category.Id,
                IsActive = true
            };

            var response = await _client.PutAsJsonAsync($"/api/v1/Dish/{dish2.Id}", update);
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            (await response.Content.ReadFromJsonAsync<ApiError>())!
                .Message.Should().Be("Ya existe un plato con ese nombre");
        }

        // ---------- 16-18) DELETE TESTS ----------

        [Fact]
        public async Task Delete_Should_Return_200_When_Dish_Deleted()
        {
            // Creo un plato primero
            var request = new DishRequest
            {
                Name = "Pizza Delete Test",
                Description = "Pizza creada para testear el delete",
                Price = 1200m,
                Category = 6,
                Image = "https://restaurant.com/images/pizza-delete.jpg"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Dish", request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await postResponse.Content.ReadFromJsonAsync<DishResponse>();

            // DELETE
            var response = await _client.DeleteAsync($"/api/v1/Dish/{created!.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var deleted = await response.Content.ReadFromJsonAsync<DishResponse>();
            deleted!.IsActive.Should().BeFalse(); // soft delete
        }


        [Fact]
        public async Task Delete_Should_Return_404_When_Dish_Not_Found()
        {
            var response = await _client.DeleteAsync($"/api/v1/Dish/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Plato no encontrado");
        }


        [Fact]
        public async Task Delete_Should_Return_409_When_Dish_In_Active_Order()
        {
            // 1. Crear un plato
            var dishRequest = new DishRequest
            {
                Name = "Pizza En Orden Activa",
                Description = "Pizza que será usada en una orden activa",
                Price = 1500m,
                Category = 6,
                Image = "https://restaurant.com/images/pizza-en-orden.jpg"
            };

            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dishRequest);
            dishResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var dish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();

            // 2. Crear una orden activa con ese plato
            var orderRequest = new OrderRequest
            {
                Items = new List<Item>
        {
            new Item { Id = dish!.Id, Quantity = 1, Notes = "Test item en orden activa" }
        },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden activa para test delete"
            };

            var orderResponse = await _client.PostAsJsonAsync("/api/v1/Order", orderRequest);
            orderResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 3. Intentar eliminar el plato
            var deleteResponse = await _client.DeleteAsync($"/api/v1/Dish/{dish.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var error = await deleteResponse.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("No se puede eliminar el plato porque está incluido en órdenes activas");
        }


        // ---------- POST EXTRA TEST ----------

        [Fact]
        public async Task Post_Should_Return_400_When_Missing_Required_Fields()
        {
            var request = new DishRequest
            {
                Name = "Provoleta Test",
                Description = "Queso a la parrilla sin price ni category"
                // Falta Price y Category
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El precio debe ser mayor a cero");
        }


        // ---------- GET EXTRA TESTS ----------

        [Fact]
        public async Task Get_Should_Return_200_Sorted_By_Price_Desc()
        {
            var response = await _client.GetAsync("/api/v1/Dish?sortByPrice=desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            dishes!.Should().BeInDescendingOrder(d => d.Price);
        }

        [Fact]
        public async Task Get_Should_Return_200_Only_Active_Dishes()
        {
            // Llamo al endpoint con filtro de activos
            var response = await _client.GetAsync("/api/v1/Dish?onlyActive=true");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            dishes!.Should().OnlyContain(d => d.IsActive);
        }

        [Fact]
        public async Task Get_Should_Return_200_All_Dishes_When_OnlyActive_False()
        {
            var response = await _client.GetAsync("/api/v1/Dish?onlyActive=false");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dishes = await response.Content.ReadFromJsonAsync<List<DishResponse>>();
            dishes.Should().NotBeNull();
            // En este caso no filtro, debería traer todos (activos e inactivos)
        }

    }
}
