namespace Shop.API.IntegrationTests.ShipmentController;

using API.Models.Requests;
using API.Models.Responses;

public class CreateShipmentTests : TestBase
{
    private readonly Faker<CreateShipmentRequest> _faker;

    public CreateShipmentTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateShipmentRequest>()
            .RuleFor(r => r.OrderId, Guid.NewGuid);
    }

    [Fact]
    public async Task CreateShipment_ShouldReturn201_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var order = await DataFactory.CreateOrderAsync();
        
        var request = _faker.Clone().RuleFor(r => r.OrderId, order.Id.Value).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/shipments", request);

        var body = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/shipments/{body!.Id}");
    }

    [Fact]
    public async Task CreateShipment_ShouldReturn400_WhenOrderIdInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.OrderId, Guid.Empty).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/shipments", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateShipment_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/shipments", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}