namespace Shop.API.IntegrationTests.AddressController;

using API.Mappings;

public class DeleteAddressTests : TestBase
{
    public DeleteAddressTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task DeleteAddress_ShouldReturn204_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var address = (await DataFactory.CreateAddressAsync()).ToResponse();

        var response = await Client.DeleteAsync($"/api/users/{address.UserId}/addresses/{address.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteAddress_ShouldReturn404_WhenAddressNotFound()
    {
        EnableAuthentication("Admin");

        var user = await DataFactory.CreateUserAsync();
        
        var response = await Client.DeleteAsync($"/api/users/{user.Id.Value}/addresses/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task DeleteAddress_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var response = await Client.DeleteAsync($"/api/users/{Guid.NewGuid()}/addresses/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteAddress_ShouldReturn400_WhenUserIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var response = await Client.DeleteAsync($"/api/users/{Guid.Empty}/addresses/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task DeleteCategory_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.DeleteAsync($"/api/users/{Guid.NewGuid()}/addresses/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}