namespace Shop.API.IntegrationTests.ProductController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class DeleteProductTests : TestBase
{
    private readonly Faker<CreateProductRequest> _faker;

    public DeleteProductTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateProductRequest>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Stock, f => f.Random.Int(1, 100))
            .RuleFor(p => p.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(p => p.CategoryIds, _ => new List<Guid> { Guid.NewGuid() });
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturn204_WhenRequestValid()
    {
        EnableAuthentication("Admin");
        
        var createRequest = _faker.Generate();

        var createResponse = await Client.PostAsJsonAsync("/api/products", createRequest); 
        
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();

        var response = await Client.DeleteAsync($"/api/products/{createdProduct!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturn404_WhenProductNotFound()
    {
        EnableAuthentication("Admin");
        
        var response = await Client.DeleteAsync($"/api/products/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task DeleteProduct_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var response = await Client.DeleteAsync($"/api/products/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteProduct_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.DeleteAsync($"/api/products/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}