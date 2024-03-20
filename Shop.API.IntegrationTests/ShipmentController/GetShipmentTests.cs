namespace Shop.API.IntegrationTests.ShipmentController;

using API.Mappings;
using API.Models.Responses;

public class GetShipmentTests : TestBase
{
    public GetShipmentTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetShipment_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var shipment = (await DataFactory.CreateShipmentAsync()).ToResponse();
        
        var response = await Client.GetAsync($"/api/shipments/{shipment.Id}");

        var body = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(shipment);
    }

    [Fact]
    public async Task GetShipment_ShouldReturn404_WhenShipmentNotFound()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/shipments/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetShipment_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var response = await Client.GetAsync($"/api/shipments/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetShipment_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync($"/api/shipments/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}