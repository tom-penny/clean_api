namespace Shop.API.IntegrationTests.ProductController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class GetProductTests : TestBase
{
    private readonly Faker<CreateProductRequest> _faker;

    public GetProductTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateProductRequest>()
            .RuleFor(r => r.Name, f => f.Commerce.ProductName())
            .RuleFor(r => r.Stock, f => f.Random.Int(1, 100))
            .RuleFor(r => r.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(r => r.CategoryIds, new List<Guid> { Guid.NewGuid() });
    }

    [Fact]
    public async Task GetProduct_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var createRequest = _faker.Generate();
        
        var createResponse = await Client.PostAsJsonAsync("/api/products", createRequest);
        
        createResponse.EnsureSuccessStatusCode();

        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();

        var response = await Client.GetAsync($"/api/products/{createdProduct!.Id}");

        var body = await response.Content.ReadFromJsonAsync<ProductResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(createdProduct);
    }

    [Fact]
    public async Task GetProduct_ShouldReturn404_WhenProductNotFound()
    {
        var response = await Client.GetAsync($"/api/products/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetProduct_ShouldReturn400_WhenIdInvalid()
    {
        var response = await Client.GetAsync($"/api/products/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}