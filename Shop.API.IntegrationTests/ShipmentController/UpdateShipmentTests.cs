namespace Shop.API.IntegrationTests.ShipmentController;

using API.Mappings;
using API.Models.Requests;

public class UpdateShipmentTests : TestBase
{
    private readonly Faker<UpdateShipmentRequest> _faker;

    public UpdateShipmentTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<UpdateShipmentRequest>()
            .RuleFor(r => r.DeliveryDate, DateTime.UtcNow);
    }

    [Fact]
    public async Task UpdateShipment_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var shipment = (await DataFactory.CreateShipmentAsync()).ToResponse();
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/shipments/{shipment.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateShipment_ShouldReturn404_WhenShipmentNotFound()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/shipments/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateShipment_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");
        
        var request = _faker.Generate();

        var response = await Client.PutAsJsonAsync($"/api/shipments/{Guid.Empty}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateShipment_ShouldReturn400_WhenDeliveryDateInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.DeliveryDate, DateTime.UtcNow.AddDays(1)).Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/shipments/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateShipment_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PutAsJsonAsync($"/api/shipments/{Guid.NewGuid()}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}