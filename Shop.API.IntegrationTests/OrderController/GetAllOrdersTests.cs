namespace Shop.API.IntegrationTests.OrderController;

using API.Mappings;
using API.Models.Responses;

public class GetAllOrdersTests : TestBase
{
    public GetAllOrdersTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllOrders_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var orders = (await DataFactory.CreateOrdersAsync(5)).Select(o => o.ToResponse()).ToList();
        
        var response = await Client.GetAsync($"/api/users/{orders.First().UserId}/orders");

        var body = await response.Content.ReadFromJsonAsync<OrdersResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Orders.Should().BeEquivalentTo(orders);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturn400_WhenSortByInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders?sortBy=invalid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllOrders_ShouldReturn400_WhenOrderByInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders?orderBy=invalid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllOrders_ShouldReturn400_WhenPageInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders?page=0");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public async Task GetAllOrders_ShouldReturn400_WhenSizeInvalid(int size)
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders?size={size}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("total", "asc")]
    [InlineData("total", "desc")]
    [InlineData("date", "asc")]
    [InlineData("date", "desc")]
    public async Task GetAllOrders_ShouldSortResults_WhenQueryValid(string sortBy, string orderBy)
    {
        EnableAuthentication("Admin");

        var orders = (await DataFactory.CreateOrdersAsync(5)).Select(o => o.ToResponse()).ToList();
        
        var sortedOrders = sortBy switch
        {
            "total" => orderBy == "asc"
                ? orders.OrderBy(u => u.Amount)
                : orders.OrderByDescending(u => u.Amount),
            "date" => orderBy == "asc"
                ? orders.OrderBy(u => u.Created)
                : orders.OrderByDescending(u => u.Created),
            _ => throw new ArgumentException()
        };

        var response = await Client.GetAsync($"/api/users/{orders.First().UserId}/orders?sortBy={sortBy}&orderBy={orderBy}");
    
        var body = await response.Content.ReadFromJsonAsync<OrdersResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Orders.Should().BeEquivalentTo(sortedOrders, o => o.WithStrictOrdering());
    }
    
    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 3)]
    public async Task GetAllOrders_ShouldPaginateResults_WhenQueryValid(int page, int size)
    {
        EnableAuthentication("Admin");

        var orders = (await DataFactory.CreateOrdersAsync(10)).Select(o => o.ToResponse()).ToList();
        
        var response = await Client.GetAsync($"/api/users/{orders.First().UserId}/orders?page={page}&size={size}");

        var body = await response.Content.ReadFromJsonAsync<OrdersResponse>();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Orders.Count.Should().Be(size);
        body.HasNextPage.Should().Be(page * size < 10);
        body.HasPreviousPage.Should().Be(page > 1);
    }
    
    [Fact]
    public async Task GetAllOrders_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}/orders");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}