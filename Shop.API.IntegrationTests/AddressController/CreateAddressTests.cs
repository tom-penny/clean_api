namespace Shop.API.IntegrationTests.AddressController;

using API.Models.Requests;
using API.Models.Responses;

public class CreateAddressTests : TestBase
{
    private readonly Faker<CreateAddressRequest> _faker;

    public CreateAddressTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateAddressRequest>()
            .RuleFor(r => r.Street, f => f.Address.StreetAddress())
            .RuleFor(r => r.City, f => f.Address.City())
            .RuleFor(r => r.Country, f => f.Address.Country())
            .RuleFor(r => r.PostCode, f => f.Address.ZipCode());
    }

    [Fact]
    public async Task CreateAddress_ShouldReturn201_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var user = await DataFactory.CreateUserAsync();
        
        var request = _faker.Generate();

        var response = await Client.PostAsJsonAsync($"/api/users/{user.Id.Value}/addresses", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<AddressResponse>();

        response.Headers.Location!.ToString().Should().Be($"http://localhost/api/users/{body!.UserId}/addresses/{body.Id}");
    }
    
    [Fact]
    public async Task CreateAddress_ShouldReturn404_WhenUserNotFound()
    {
        EnableAuthentication("Admin");

        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync($"/api/users/{Guid.NewGuid()}/addresses", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateAddress_ShouldReturn400_WhenUserIdInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync($"/api/users/{Guid.Empty}/addresses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateAddress_ShouldReturn400_WhenStreetInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Street, "").Generate();
        
        var response = await Client.PostAsJsonAsync($"/api/users/{Guid.NewGuid()}/addresses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateAddress_ShouldReturn400_WhenCityInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.City, "").Generate();
        
        var response = await Client.PostAsJsonAsync($"/api/users/{Guid.NewGuid()}/addresses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateAddress_ShouldReturn400_WhenCountryInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.Country, "").Generate();
        
        var response = await Client.PostAsJsonAsync($"/api/users/{Guid.NewGuid()}/addresses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateAddress_ShouldReturn400_WhenPostCodeInvalid()
    {
        EnableAuthentication("Admin");

        var request = _faker.Clone().RuleFor(r => r.PostCode, "").Generate();
        
        var response = await Client.PostAsJsonAsync($"/api/users/{Guid.NewGuid()}/addresses", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateAddress_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var request = _faker.Generate();
        
        var response = await Client.PostAsJsonAsync($"/api/users/{Guid.NewGuid()}/addresses", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}