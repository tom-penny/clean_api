namespace Shop.API.IntegrationTests.CategoryController;

using API.Mappings;
using API.Models.Responses;

public class GetCategoryTests : TestBase
{
    public GetCategoryTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetCategory_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var category = (await DataFactory.CreateCategoryAsync()).ToResponse();
        
        var response = await Client.GetAsync($"/api/categories/{category.Id}");

        var body = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async Task GetCategory_ShouldReturn404_WhenCategoryNotFound()
    {
        var response = await Client.GetAsync($"/api/categories/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetCategory_ShouldReturn400_WhenIdInvalid()
    {
        var response = await Client.GetAsync($"/api/categories/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}