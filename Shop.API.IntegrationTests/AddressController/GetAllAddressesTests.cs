namespace Shop.API.IntegrationTests.AddressController;

using API.Mappings;
using API.Models.Responses;

public class GetAllAddressesTests : TestBase
{
    public GetAllAddressesTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllAddresses_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var addresses = (await DataFactory.CreateAddressesAsync(5)).Select(o => o.ToResponse()).ToList();
        
        var response = await Client.GetAsync($"/api/users/{addresses.First().UserId}/addresses");

        var body = await response.Content.ReadFromJsonAsync<AddressesResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Addresses.Should().BeEquivalentTo(addresses);
    }
    
    [Fact]
    public async Task GetAllAddresses_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/addresses");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}