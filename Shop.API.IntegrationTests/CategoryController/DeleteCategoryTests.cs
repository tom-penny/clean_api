namespace Shop.API.IntegrationTests.CategoryController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class DeleteCategoryTests : TestBase
{
    private readonly Faker<CreateCategoryRequest> _faker;

    public DeleteCategoryTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateCategoryRequest>()
            .RuleFor(r => r.Name, f => f.Commerce.Categories(1).First());
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturn204_WhenRequestValid()
    {
        EnableAuthentication("Admin");
        
        var createRequest = _faker.Generate();

        var createResponse = await Client.PostAsJsonAsync("/api/categories", createRequest);

        createResponse.EnsureSuccessStatusCode();
        
        var createdCategory = await createResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        var response = await Client.DeleteAsync($"/api/categories/{createdCategory!.Id}");

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