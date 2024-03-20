namespace Shop.API.IntegrationTests.ProductController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class UpdateProductTests : TestBase
{
    private readonly Faker<UpdateProductRequest> _faker;

    public UpdateProductTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<UpdateProductRequest>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Stock, f => f.Random.Int(1, 100))
            .RuleFor(p => p.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(p => p.CategoryIds, _ => new List<Guid> { Guid.NewGuid() });
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");
        
        var createRequest = _faker.Generate();
        
        var createResponse = await Client.PostAsJsonAsync("/api/products", createRequest);

        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();

        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/products/{createdProduct!.Id}", request);

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

        var request = _faker.Clone().RuleFor(p => p.Name, "").Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldReturn400_WhenStockInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(p => p.Stock, -1).Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldReturn400_WhenPriceInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(p => p.Price, decimal.Zero).Generate();
        
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