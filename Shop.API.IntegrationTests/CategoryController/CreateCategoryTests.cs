namespace Shop.API.IntegrationTests.CategoryController;

using API.Models.Requests;
using API.Models.Responses;

public class CreateCategoryTests : TestBase
{
    private readonly Faker<CreateCategoryRequest> _faker;

    public CreateCategoryTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateCategoryRequest>()
            .RuleFor(r => r.Name, f => f.Commerce.Department());
    }

    [Fact]
    public async Task CreateCategory_ShouldReturn201_WhenRequestValid()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/categories", request);

        var body = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/categories/{body!.Id}");
    }

    [Fact]
    public async Task CreateCategory_ShouldReturn400_WhenNameInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Name, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/categories", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturn409_WhenNameExists()
    {
        EnableAuthentication("Admin");

        var createdCategory = await DataFactory.CreateCategoryAsync();

        var request = _faker.Clone().RuleFor(r => r.Name, createdCategory.Name).Generate();

        var response = await Client.PostAsJsonAsync("/api/categories", request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/categories", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}