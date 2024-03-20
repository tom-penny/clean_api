namespace Shop.API.IntegrationTests.CategoryController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class UpdateCategoryTests : TestBase
{
    private readonly Faker<UpdateCategoryRequest> _faker;

    public UpdateCategoryTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<UpdateCategoryRequest>()
            .RuleFor(r => r.Name, f => f.Commerce.Categories(1).First());
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");
        
        var createRequest = _faker.Generate();
        
        var createResponse = await Client.PostAsJsonAsync("/api/categories", createRequest);

        createResponse.EnsureSuccessStatusCode();

        var createdCategory = await createResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/categories/{createdCategory!.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateCategory_ShouldReturn404_WhenCategoryNotFound()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/categories/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/categories/{Guid.Empty}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateCategory_ShouldReturn400_WhenNameInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Name, "").Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/categories/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateCategory_ShouldReturn409_WhenNameExists()
    {
        EnableAuthentication("Admin");

        var createRequest = _faker.Generate();
        
        var createResponse = await Client.PostAsJsonAsync("/api/categories", createRequest);

        createResponse.EnsureSuccessStatusCode();
        
        var createdCategory = await createResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        var request = _faker.Clone().RuleFor(r => r.Name, createRequest.Name).Generate();

        var response = await Client.PutAsJsonAsync($"/api/categories/{createdCategory!.Id}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/categories/{Guid.NewGuid()}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}