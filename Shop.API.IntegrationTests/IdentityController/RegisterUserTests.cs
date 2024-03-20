namespace Shop.API.IntegrationTests.IdentityController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class RegisterUserTests : TestBase
{
    private readonly Faker<RegisterUserRequest> _faker;

    public RegisterUserTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<RegisterUserRequest>()
            .RuleFor(r => r.FirstName, f => f.Person.FirstName)
            .RuleFor(r => r.LastName, f => f.Person.LastName)
            .RuleFor(r => r.Email, f => "bla@test.com")
            .RuleFor(r => r.Password, f => "Abc123!");
    }

    [Fact]
    public async Task Register_ShouldReturn200_WhenRequestValid()
    {
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/auth/register", request);

        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!["token"].ToString().Should().NotBeEmpty();
    }

    [Fact]
    public async Task Register_ShouldReturn400_WhenFirstNameInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.FirstName, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Register_ShouldReturn400_WhenLastNameInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.LastName, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Register_ShouldReturn400_WhenEmailInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.Email, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Register_ShouldReturn400_WhenPasswordInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.Password, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}