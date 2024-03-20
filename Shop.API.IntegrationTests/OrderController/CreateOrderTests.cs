namespace Shop.API.IntegrationTests.OrderController;

using API.Models.Requests;
using API.Models.Responses;

public class CreateOrderTests : TestBase
{
    private readonly Faker<CreateOrderRequest> _faker;

    public CreateOrderTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateOrderRequest>()
            .RuleFor(r => r.CheckoutId, Guid.NewGuid().ToString())
            .RuleFor(r => r.UserId, Guid.NewGuid)
            .RuleFor(r => r.Amount, f => f.Finance.Amount(1m, 500m))
            .RuleFor(r => r.Items, f => new List<OrderItemRequest>
            { 
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = f.Random.Int(1, 10),
                    UnitPrice = f.Finance.Amount(1m, 100m)
                }
            });
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn201_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var user = await DataFactory.CreateUserAsync();

        var product = await DataFactory.CreateProductAsync();

        var orderItem = new OrderItemRequest
        {
            ProductId = product.Id.Value,
            Quantity = 10,
            UnitPrice = product.Price
        };

        var request = _faker.Clone()
            .RuleFor(r => r.UserId, user.Id.Value)
            .RuleFor(r => r.Amount, orderItem.UnitPrice * orderItem.UnitPrice)
            .RuleFor(r => r.Items, new List<OrderItemRequest> { orderItem })
            .Generate();

        var response = await Client.PostAsJsonAsync("/api/orders", request);

        var body = await response.Content.ReadFromJsonAsync<OrderResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/users/{body!.UserId}/orders/{body.Id}");
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenCheckoutIdInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.CheckoutId, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenUserIdInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.UserId, Guid.Empty).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenAmountInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Amount, decimal.Zero).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenItemsEmpty()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Items, new List<OrderItemRequest>()).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenItemProductIdInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone()
            .RuleFor(r => r.Items, f => new List<OrderItemRequest>
            {
                new()
                {
                    ProductId = Guid.Empty,
                    Quantity = f.Random.Int(1, 10),
                    UnitPrice = f.Finance.Amount(1m, 100m)
                }
            })
            .Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenItemQuantityInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone()
            .RuleFor(r => r.Items, f => new List<OrderItemRequest>
            {
                new()
                {
                    ProductId = Guid.Empty,
                    Quantity = 0,
                    UnitPrice = f.Finance.Amount(1m, 100m)
                }
            })
            .Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenItemUnitPriceInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone()
            .RuleFor(r => r.Items, f => new List<OrderItemRequest>
            {
                new()
                {
                    ProductId = Guid.Empty,
                    Quantity = f.Random.Int(1, 10),
                    UnitPrice = decimal.Zero
                }
            })
            .Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/orders", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}