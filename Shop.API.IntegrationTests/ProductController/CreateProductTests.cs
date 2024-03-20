namespace Shop.API.IntegrationTests.ProductController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class CreateProductTests : TestBase
{
    private readonly Faker<CreateProductRequest> _faker;

    public CreateProductTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateProductRequest>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Stock, f => f.Random.Int(1, 100))
            .RuleFor(p => p.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(p => p.CategoryIds, _ => new List<Guid> { Guid.NewGuid() });
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

        var request = _faker.Clone().RuleFor(p => p.Name, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateProduct_ShouldReturn400_WhenStockInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(p => p.Stock, -1).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateProduct_ShouldReturn400_WhenPriceInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(p => p.Price, decimal.Zero).Generate();
        
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