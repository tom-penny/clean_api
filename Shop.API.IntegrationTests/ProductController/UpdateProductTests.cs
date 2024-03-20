namespace Shop.API.IntegrationTests.ProductController;

using API.Mappings;
using API.Models.Requests;

public class UpdateProductTests : TestBase
{
    private readonly Faker<UpdateProductRequest> _faker;

    public UpdateProductTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<UpdateProductRequest>()
            .RuleFor(r => r.Name, f => f.Commerce.ProductName())
            .RuleFor(r => r.Stock, f => f.Random.Int(1, 100))
            .RuleFor(r => r.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(r => r.CategoryIds, new List<Guid> { Guid.NewGuid() });
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var product = (await DataFactory.CreateProductAsync()).ToResponse();

        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/products/{product.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldReturn404_WhenProductNotFound()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.Empty}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldReturn400_WhenNameInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Name, "").Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldReturn400_WhenStockInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Stock, -1).Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldReturn400_WhenPriceInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Price, decimal.Zero).Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}