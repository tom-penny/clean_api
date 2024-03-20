namespace Shop.API.IntegrationTests.CategoryController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class GetCategoryTests : TestBase
{
    private readonly Faker<CreateCategoryRequest> _faker;

    public GetCategoryTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateCategoryRequest>()
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First());
    }

    [Fact]
    public async Task GetCategory_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var createRequest = _faker.Generate();
        
        var createResponse = await Client.PostAsJsonAsync("/api/categories", createRequest);

        var createdCategory = await createResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        var response = await Client.GetAsync($"/api/categories/{createdCategory!.Id}");

        var body = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(createdCategory);
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