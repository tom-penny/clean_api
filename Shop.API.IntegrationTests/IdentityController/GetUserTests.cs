namespace Shop.API.IntegrationTests.IdentityController;

using API.Mappings;
using API.Models.Responses;

public class GetUserTests : TestBase
{
    public GetUserTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetUser_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var user = (await DataFactory.CreateUserAsync()).ToResponse();
        
        var response = await Client.GetAsync($"/api/users/{user.Id}");

        var body = await response.Content.ReadFromJsonAsync<UserResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUser_ShouldReturn404_WhenUserNotFound()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetUser_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetUser_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}