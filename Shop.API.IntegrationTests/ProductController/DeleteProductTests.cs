namespace Shop.API.IntegrationTests.ProductController;

using API.Mappings;

public class DeleteProductTests : TestBase
{
    public DeleteProductTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task DeleteProduct_ShouldReturn204_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var product = (await DataFactory.CreateProductAsync()).ToResponse();

        var response = await Client.DeleteAsync($"/api/products/{product.Id}");

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