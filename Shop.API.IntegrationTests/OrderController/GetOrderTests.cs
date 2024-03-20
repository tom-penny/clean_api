namespace Shop.API.IntegrationTests.OrderController;

using API.Mappings;
using API.Models.Responses;

public class GetOrderTests : TestBase
{
    public GetOrderTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetOrder_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var order = (await DataFactory.CreateOrderAsync()).ToResponse();

        var response = await Client.GetAsync($"/api/users/{order.UserId}/orders/{order.Id}");

        var body = await response.Content.ReadFromJsonAsync<OrderResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(order);
    }

    [Fact]
    public async Task GetOrder_ShouldReturn404_WhenOrderNotFound()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetOrder_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetOrder_ShouldReturn400_WhenUserIdInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.Empty}/orders/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetOrder_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}