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
            .RuleFor(r => r.Name, f => f.Commerce.Department());
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturn200_WhenRequestValid()
    {
        var createdCategories = await CreateCategoriesAsync(5);

        var response = await Client.GetAsync("/api/categories");

        var body = await response.Content.ReadFromJsonAsync<CategoriesResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Categories.Should().BeEquivalentTo(createdCategories);
    }
    
    private async Task<List<CategoryResponse>> CreateCategoriesAsync(int count)
    {
        EnableAuthentication("Admin");

        var categories = new List<CategoryResponse>();
        
        for (var i = 0; i < count; i++)
        {
            var createRequest = _faker.Generate();
            
            var createResponse = await Client.PostAsJsonAsync("/api/categories", createRequest);
            
            createResponse.EnsureSuccessStatusCode();
            
            var createdCategory = await createResponse.Content.ReadFromJsonAsync<CategoryResponse>();
            
            categories.Add(createdCategory!);
        }
        
        return categories;
    }
}