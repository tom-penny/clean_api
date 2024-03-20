namespace Shop.API.IntegrationTests.CategoryController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class GetAllCategoriesTests : TestBase
{
    private readonly Faker<CreateCategoryRequest> _faker;

    public GetAllCategoriesTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateCategoryRequest>()
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First());
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var createRequest = _faker.Generate();
        
        var createResponse = await Client.PostAsJsonAsync("/api/categories", createRequest);

        var createdCategory = await createResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        var response = await Client.GetAsync("/api/categories");

        var body = await response.Content.ReadFromJsonAsync<CategoriesResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Categories.Single().Should().BeEquivalentTo(createdCategory);
    }
}