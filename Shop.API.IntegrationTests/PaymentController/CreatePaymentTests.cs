namespace Shop.API.IntegrationTests.PaymentController;

using API.Models.Requests;
using API.Models.Responses;

public class CreatePaymentTests : TestBase
{
    private readonly Faker<CreatePaymentRequest> _faker;

    public CreatePaymentTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreatePaymentRequest>()
            .RuleFor(r => r.CheckoutId, Guid.NewGuid().ToString())
            .RuleFor(r => r.Status, f => f.Random.Bool() ? "COMPLETED" : "FAILED")
            .RuleFor(r => r.Amount, f => f.Finance.Amount(1m, 500m));
    }

    [Fact]
    public async Task CreatePayment_ShouldReturn201_WhenRequestValid()
    {
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync("/api/payments", request);

        var body = await response.Content.ReadFromJsonAsync<PaymentResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/payments/{body!.Id}");
    }

    [Fact]
    public async Task CreatePayment_ShouldReturn400_WhenCheckoutIdInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.CheckoutId, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/payments", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreatePayment_ShouldReturn400_WhenStatusInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.Status, "").Generate();
        
        var response = await Client.PostAsJsonAsync("/api/payments", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreatePayment_ShouldReturn400_WhenAmountInvalid()
    {
        var request = _faker.Clone().RuleFor(r => r.Amount, decimal.Zero).Generate();
        
        var response = await Client.PostAsJsonAsync("/api/payments", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}