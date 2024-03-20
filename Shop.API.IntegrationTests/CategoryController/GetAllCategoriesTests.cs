namespace Shop.API.IntegrationTests.CategoryController;

using API.Mappings;
using API.Models.Responses;

public class GetAllCategoriesTests : TestBase
{
    public GetAllCategoriesTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllCategories_ShouldReturn200_WhenRequestValid()
    {
        var categories = (await DataFactory.CreateCategoriesAsync(5)).Select(c => c.ToResponse());
        
        var response = await Client.GetAsync("/api/categories");

        var body = await response.Content.ReadFromJsonAsync<CategoriesResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Categories.Should().BeEquivalentTo(categories);
    }
}