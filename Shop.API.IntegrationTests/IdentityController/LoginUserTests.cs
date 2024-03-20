namespace Shop.API.IntegrationTests.IdentityController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class LoginUserTests : TestBase
{
    private readonly Faker<RegisterUserRequest> _faker;

    public LoginUserTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<RegisterUserRequest>()
            .RuleFor(r => r.FirstName, f => f.Person.FirstName)
            .RuleFor(r => r.LastName, f => f.Person.LastName)
            .RuleFor(r => r.Email, f => "bla@test.com")
            .RuleFor(r => r.Password, f => "Abc123!");
    }

    [Fact]
    public async Task Login_ShouldReturn200_WhenRequestValid()
    {
        var registerRequest = _faker.Generate();
        
        await Client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var request = new Faker<LoginUserRequest>()
            .RuleFor(r => r.Email, _ => registerRequest.Email)
            .RuleFor(r => r.Password, _ => registerRequest.Password)
            .Generate();

        var response = await Client.PostAsJsonAsync("/api/auth/login", request);
        
        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!["token"].ToString().Should().NotBeEmpty();
    }

    [Fact]
    public async Task Login_ShouldReturn400_WhenEmailInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.Email, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Login_ShouldReturn400_WhenPasswordInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.Password, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}