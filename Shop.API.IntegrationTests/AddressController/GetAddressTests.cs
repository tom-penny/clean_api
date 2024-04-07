namespace Shop.API.IntegrationTests.AddressController;

using API.Mappings;
using API.Models.Responses;

public class GetAddressTests : TestBase
{
    public GetAddressTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAddress_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var address = (await DataFactory.CreateAddressAsync()).ToResponse();

        var response = await Client.GetAsync($"/api/users/{address.UserId}/addresses/{address.Id}");

        var body = await response.Content.ReadFromJsonAsync<AddressResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(address);
    }
    
    [Fact]
    public async Task GetAddress_ShouldReturn404_WhenAddressNotFound()
    {
        EnableAuthentication("Admin");
        
        var user = await DataFactory.CreateUserAsync();

        var response = await Client.GetAsync($"/api/users/{user.Id.Value}/addresses/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetAddress_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/addresses/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAddress_ShouldReturn400_WhenUserIdInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.Empty}/addresses/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAddress_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/addresses/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}