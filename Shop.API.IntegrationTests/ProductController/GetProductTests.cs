namespace Shop.API.IntegrationTests.ProductController;

using API.Mappings;
using API.Models.Responses;

public class GetProductTests : TestBase
{
    public GetProductTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetProduct_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var product = (await DataFactory.CreateProductAsync()).ToResponse();
        
        var response = await Client.GetAsync($"/api/products/{product.Id}");

        var body = await response.Content.ReadFromJsonAsync<ProductResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(product);
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