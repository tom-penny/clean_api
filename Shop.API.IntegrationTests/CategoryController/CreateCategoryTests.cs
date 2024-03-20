namespace Shop.API.IntegrationTests.CategoryController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class CreateCategoryTests : TestBase
{
    private readonly Faker<CreateCategoryRequest> _faker;

    public CreateCategoryTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateCategoryRequest>()
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First());
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

        var request = _faker.Clone().RuleFor(p => p.Name, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/categories", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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