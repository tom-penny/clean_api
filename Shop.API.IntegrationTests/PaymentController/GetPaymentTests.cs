namespace Shop.API.IntegrationTests.PaymentController;

using API.Mappings;
using API.Models.Responses;

public class GetPaymentTests : TestBase
{
    public GetPaymentTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetPayment_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var payment = (await DataFactory.CreatePaymentAsync()).ToResponse();

        var response = await Client.GetAsync($"/api/payments/{payment.Id}");

        var body = await response.Content.ReadFromJsonAsync<PaymentResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().BeEquivalentTo(payment);
    }

    [Fact]
    public async Task GetPayment_ShouldReturn404_WhenPaymentNotFound()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/payments/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetPayment_ShouldReturn400_WhenIdInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/payments/{Guid.Empty}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetPayment_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync($"/api/payments/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}