namespace Shop.API.IntegrationTests.CategoryController;

using API.Mappings;
using API.Models.Requests;

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

        var category = (await DataFactory.CreateCategoryAsync()).ToResponse();
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/categories/{category.Id}", request);

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

        var category = (await DataFactory.CreateCategoryAsync()).ToResponse();
        
        var request = _faker.Clone().RuleFor(r => r.Name, category.Name).Generate();

        var response = await Client.PutAsJsonAsync($"/api/categories/{category.Id}", request);
        
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