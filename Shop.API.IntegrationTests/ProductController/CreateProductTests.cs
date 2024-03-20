namespace Shop.API.IntegrationTests.ProductController;

using API.Models.Requests;
using API.Models.Responses;

public class CreateProductTests : TestBase
{
    private readonly Faker<CreateProductRequest> _faker;

    public CreateProductTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateProductRequest>()
            .RuleFor(r => r.Name, f => f.Commerce.ProductName())
            .RuleFor(r => r.Stock, f => f.Random.Int(1, 100))
            .RuleFor(r => r.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(r => r.CategoryIds, new List<Guid> { Guid.NewGuid() });
    }

    [Fact]
    public async Task CreateProduct_ShouldReturn201_WhenRequestValid()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/products", request);

        var body = await response.Content.ReadFromJsonAsync<ProductResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/products/{body!.Id}");
    }

    [Fact]
    public async Task CreateProduct_ShouldReturn400_WhenNameInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Name, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateProduct_ShouldReturn400_WhenStockInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Stock, -1).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateProduct_ShouldReturn400_WhenPriceInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Price, decimal.Zero).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/products", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/products", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}