namespace Shop.API.IntegrationTests.CategoryController;

using API.Mappings;
using API.Models.Requests;

public class DeleteCategoryTests : TestBase
{
    public DeleteCategoryTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task DeleteCategory_ShouldReturn204_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var category = (await DataFactory.CreateCategoryAsync()).ToResponse();

        var response = await Client.DeleteAsync($"/api/categories/{category.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturn404_WhenCategoryNotFound()
    {
        EnableAuthentication("Admin");
        
        var response = await Client.DeleteAsync($"/api/categories/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task DeleteCategory_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var response = await Client.DeleteAsync($"/api/categories/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteCategory_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.DeleteAsync($"/api/categories/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}