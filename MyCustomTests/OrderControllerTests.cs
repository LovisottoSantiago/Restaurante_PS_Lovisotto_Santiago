using System.Net;
using System.Net.Http.Json;
using Application.Models;
using Application.Response;
using FluentAssertions;
using MyCustomTests.Helpers;

namespace MyCustomTests
{
    public class OrderControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public OrderControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        // ---------- HELPER PARA CREAR PLATOS ----------
        private async Task<DishResponse> CreateTestDish(string name = "Item Test Order", decimal price = 1000m, int category = 1)
        {
            var dish = new DishRequest
            {
                Name = name + Guid.NewGuid(), // evitar duplicados
                Description = "Plato de prueba para Order tests",
                Price = price,
                Category = category,
                Image = "https://restaurant.com/images/test.jpg"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Dish", dish);
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<DishResponse>())!;
        }

        // ---------- 1) 201 CREATED ----------

        [Fact]
        public async Task Post_Should_Return_201_When_New_Valid_Order()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 2, Notes = "Sin sal" }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Timbre: 5B"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<OrderCreateReponse>();
            created.Should().NotBeNull();
            created!.OrderNumber.Should().BeGreaterThan(0);
            created.TotalAmount.Should().BeGreaterThan(0);
            created.CreatedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-1));
        }

        // ---------- 2) 400 INVALID DISH ----------

        [Fact]
        public async Task Post_Should_Return_400_When_Dish_Not_Exists()
        {
            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = Guid.NewGuid(), Quantity = 1 }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El plato especificado no existe o no está disponible");
        }

        // ---------- 3) 400 INVALID QUANTITY ----------
        [Fact]
        public async Task Post_Should_Return_400_When_Quantity_Is_Zero()
        {
            var dish = await CreateTestDish();

            var payload = new
            {
                items = new[]
                {
                    new { id = dish.Id, quantity = 0, notes = "Cantidad inválida" }
                },
                delivery = new { id = 1, to = "Av. Corrientes 1234" },
                notes = "string"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("La cantidad debe ser mayor a 0");
        }

        // ---------- 4) 400 MISSING DELIVERY ----------

        [Fact]
        public async Task Post_Should_Return_400_When_Missing_Delivery()
        {
            var dish = await CreateTestDish();

            var payload = new
            {
                items = new[]
                {
                    new { id = dish.Id, quantity = 1, notes = "Item Test" }
                },
                delivery = new { id = 0, to = "string" },
                notes = "string"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Debe especificar un tipo de entrega válido");
        }



        // ---------- 5) 400 INVALID GUID IN ITEM ----------

        [Fact]
        public async Task Post_Should_Return_400_When_Item_Id_Is_Invalid_Guid()
        {
            var payload = new
            {
                items = new[]
                {
                    new { id = "123", quantity = 1, notes = "ID inválido" }
                },
                delivery = new { id = 1, to = "Av. Corrientes 1234" }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El plato especificado no existe o no está disponible");
        }

        // ---------- 6) 400 EMPTY GUID ----------

        [Fact]
        public async Task Post_Should_Return_400_When_Item_Id_Is_Empty_Guid()
        {
            var payload = new
            {
                items = new[]
                {
                    new { id = "", quantity = 1, notes = "GUID vacío" }
                },
                delivery = new { id = 1, to = "Av. Corrientes 1234" }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El plato especificado no existe o no está disponible");
        }

        // ---------- 7) 200 GET ALL ----------
        [Fact]
        public async Task Get_Should_Return_200_With_List_Of_Orders()
        {
            var dish = await CreateTestDish();

            // Crear una orden primero
            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 1, Notes = "Test Item" }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden para GET ALL"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Order", request);
            var postBody = await postResponse.Content.ReadAsStringAsync();
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created, $"POST devolvió: {postBody}");

            // Llamar al GET ALL
            var response = await _client.GetAsync("/api/v1/Order");
            var body = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"GET ALL devolvió: {body}");

            var orders = await response.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            orders.Should().NotBeNull();
            orders!.Should().NotBeEmpty();
            orders.Any(o => o.Notes == "Orden para GET ALL").Should().BeTrue();
        }

        // ---------- 8) 200 GET ALL WITH FILTERS ----------
        [Fact]
        public async Task Get_Should_Return_200_When_Filtering_By_Status()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 1 }
                },
                Delivery = new Delivery { Id = 1, To = "Filtro de estado" }
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Order", request);

            var from = DateTime.UtcNow.AddMinutes(-5).ToString("yyyy-MM-ddTHH:mm:ss");
            var to = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss");
            var response = await _client.GetAsync($"/api/v1/Order?status=1&from={from}&to={to}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);


            var orders = await response.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            orders.Should().NotBeNull();
            orders!.All(o => o.Status.Id == 1).Should().BeTrue();
        }


        // ---------- 9) 400 INVALID DATE RANGE ----------
        [Fact]
        public async Task Get_Should_Return_400_When_DateRange_Is_Invalid()
        {
            var from = DateTime.UtcNow;
            var to = DateTime.UtcNow.AddDays(-1); // inverso

            var response = await _client.GetAsync($"/api/v1/Order?from={from:o}&to={to:o}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Rango de fechas inválido");
        }

        // ---------- 10) 200 GET BY ID ----------
        [Fact]
        public async Task GetById_Should_Return_200_When_Order_Exists()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = dish.Id, Quantity = 2, Notes = "Test Item" }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden para GET BY ID"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Order", request);
            var postBody = await postResponse.Content.ReadAsStringAsync();
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created, $"POST devolvió: {postBody}");

            var created = await postResponse.Content.ReadFromJsonAsync<OrderCreateReponse>();

            // GET BY ID con el número de orden creado
            var response = await _client.GetAsync($"/api/v1/Order/{created!.OrderNumber}");
            var body = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"GET BY ID devolvió: {body}");

            var order = await response.Content.ReadFromJsonAsync<OrderDetailsResponse>();
            order.Should().NotBeNull();
            order!.OrderNumber.Should().Be(created.OrderNumber);
            order.TotalAmount.Should().Be(created.TotalAmount);
            order.Items.Should().HaveCount(1);
        }

        // ---------- 11) 404 GET BY ID NOT FOUND ----------
        [Fact]
        public async Task GetById_Should_Return_404_When_Order_Not_Exists()
        {
            var response = await _client.GetAsync("/api/v1/Order/999999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Orden no encontrada");
        }

        // ---------- 12) 400 ITEMS NULL ----------
        [Fact]
        public async Task Post_Should_Return_400_When_Items_Are_Null()
        {
            var payload = new
            {
                items = (object?)null,
                delivery = new { id = 1, to = "Av. Corrientes 1234" }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El plato especificado no existe o no está disponible");
        }

        // ---------- 13) 400 ITEMS EMPTY ----------
        [Fact]
        public async Task Post_Should_Return_400_When_Items_Are_Empty()
        {
            var payload = new
            {
                items = new object[] { },
                delivery = new { id = 1, to = "Av. Corrientes 1234" }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Order", payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El plato especificado no existe o no está disponible");
        }


        // ---------- 15) 200 GET WITH ONLY STATUS ----------
        [Fact]
        public async Task Get_Should_Return_200_When_Filtering_Only_By_Status()
        {
            var dish = await CreateTestDish();

            var request = new OrderRequest
            {
                Items = new List<Items> { new Items { Id = dish.Id, Quantity = 1 } },
                Delivery = new Delivery { Id = 1, To = "Only status test" }
            };

            await _client.PostAsJsonAsync("/api/v1/Order", request);

            var response = await _client.GetAsync("/api/v1/Order?status=1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var orders = await response.Content.ReadFromJsonAsync<List<OrderDetailsResponse>>();
            orders.Should().NotBeNull();
            orders!.All(o => o.Status.Id == 1).Should().BeTrue();
        }


        // ---------- 17) 404 GET BY ID NON NUMERIC ----------
        [Fact]
        public async Task GetById_Should_Return_404_When_Id_Is_Not_Number()
        {
            var response = await _client.GetAsync("/api/v1/Order/abc");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // ---------- CreateOrderWithDish ----------
        private async Task<(long orderNumber, long itemId, decimal totalAmount)> CreateOrderWithDish(string dishName, int quantity = 1)
        {
            // 1) Crear un plato válido
            var dish = new DishRequest
            {
                Name = dishName + Guid.NewGuid(),
                Description = "Plato de prueba PATCH",
                Price = 1000,
                Category = 6, // pizzas existe en tus seeds
                Image = "https://restaurant.com/images/test.jpg"
            };
            var dishResponse = await _client.PostAsJsonAsync("/api/v1/Dish", dish);
            dishResponse.EnsureSuccessStatusCode();
            var createdDish = await dishResponse.Content.ReadFromJsonAsync<DishResponse>();

            // 2) Crear la orden con ese plato
            var orderReq = new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items { Id = createdDish!.Id, Quantity = quantity, Notes = "PATCH item" }
                },
                Delivery = new Delivery { Id = 1, To = "Av. Corrientes 1234" },
                Notes = "Orden para PATCH test"
            };
            var postOrder = await _client.PostAsJsonAsync("/api/v1/Order", orderReq);
            postOrder.EnsureSuccessStatusCode();
            var createdOrder = await postOrder.Content.ReadFromJsonAsync<OrderCreateReponse>();

            // 3) GET de la orden para sacar el itemId
            var getOrder = await _client.GetAsync($"/api/v1/Order/{createdOrder!.OrderNumber}");
            getOrder.EnsureSuccessStatusCode();
            var order = await getOrder.Content.ReadFromJsonAsync<OrderDetailsResponse>();
            var itemId = order!.Items.First().Id;

            return (createdOrder.OrderNumber, itemId, order.TotalAmount);
        }

        // ---------- 18) PATCH 200 OK ----------
        [Fact]
        public async Task Patch_Should_Return_200_When_Update_Item_Status()
        {
            var (orderNumber, itemId, total) = await CreateOrderWithDish("plato patch1");

            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 2 } // En preparación
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateReponse>();
            updated.Should().NotBeNull();
            updated!.OrderNumber.Should().Be(orderNumber);
            updated.TotalAmount.Should().Be(total); // total no cambia
        }

        // ---------- 19) 400 STATUS INVALID ----------
        [Fact]
        public async Task Patch_Should_Return_400_When_Status_Is_Invalid()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("plato patch2");

            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 999 }
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await patchResponse.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("El estado especificado no es válido");
        }

        // ---------- 20) 400 TRANSICIÓN INVÁLIDA ----------
        [Fact]
        public async Task Patch_Should_Return_400_When_Transition_Is_Invalid()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("plato patch3");

            // primero lo llevo a Entregado (4)
            var firstPatch = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 4 }
            );
            firstPatch.StatusCode.Should().Be(HttpStatusCode.OK);

            // intentar volver a En preparación (2) → inválido
            var invalidPatch = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 2 }
            );

            invalidPatch.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await invalidPatch.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Transición no permitida");
        }

        // ---------- 21) 404 ORDER NOT FOUND ----------
        [Fact]
        public async Task Patch_Should_Return_404_When_Order_Not_Found()
        {
            var response = await _client.PatchAsJsonAsync(
                "/api/v1/Order/999999/item/1",
                new { status = 2 }
            );
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Orden no encontrada");
        }

        // ---------- 22) 404 ITEM NOT FOUND ----------
        [Fact]
        public async Task Patch_Should_Return_404_When_Item_Not_Found()
        {
            var (orderNumber, itemId, _) = await CreateOrderWithDish("plato patch4");

            var fakeItemId = itemId + 9999; // aseguramos que no exista

            var response = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{fakeItemId}",
                new { status = 2 }
            );

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            error!.Message.Should().Be("Item no encontrado en la orden");
        }

        // ---------- 23) PATCH Pendiente → Listo ----------
        [Fact]
        public async Task Patch_Should_Return_200_When_Pending_To_Ready()
        {
            var (orderNumber, itemId, total) = await CreateOrderWithDish("plato patch5");

            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 3 } // Ready
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateReponse>();
            updated.Should().NotBeNull();
            updated!.OrderNumber.Should().Be(orderNumber);
            updated.TotalAmount.Should().Be(total);
        }

        // ---------- 24) PATCH Cancelar item ----------
        [Fact]
        public async Task Patch_Should_Return_200_When_Cancel_Item()
        {
            var (orderNumber, itemId, total) = await CreateOrderWithDish("plato patch6");

            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 5 } // Cancelled
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateReponse>();
            updated.Should().NotBeNull();
            updated!.OrderNumber.Should().Be(orderNumber);
        }

        // ---------- 25) PATCH Delivery después de Ready ----------
        [Fact]
        public async Task Patch_Should_Return_200_When_Ready_To_Delivery()
        {
            var (orderNumber, itemId, total) = await CreateOrderWithDish("plato patch7");

            // 1) Pendiente → Ready
            await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 3 }
            );

            // 2) Ready → Delivery
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/v1/Order/{orderNumber}/item/{itemId}",
                new { status = 4 }
            );

            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await patchResponse.Content.ReadFromJsonAsync<OrderUpdateReponse>();
            updated!.OrderNumber.Should().Be(orderNumber);
            updated.TotalAmount.Should().Be(total);
        }


    }
}