namespace Shop.API.IntegrationTests.IdentityController;

using API.Mappings;
using API.Models.Responses;

public class GetAllUsersTests : TestBase
{
    public GetAllUsersTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllUsers_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var users = (await DataFactory.CreateUsersAsync(5)).Select(u => u.ToResponse());
        
        var response = await Client.GetAsync("/api/users");

        var body = await response.Content.ReadFromJsonAsync<UsersResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Users.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturn400_WhenSortInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync("/api/users?sort=invalid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllUsers_ShouldReturn400_WhenOrderInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync("/api/users?order=invalid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllUsers_ShouldReturn400_WhenPageInvalid()
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync("/api/users?page=0");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public async Task GetAllUsers_ShouldReturn400_WhenLimitInvalid(int limit)
    {
        EnableAuthentication("Admin");

        var response = await Client.GetAsync($"/api/users?limit={limit}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("email", "asc")]
    [InlineData("email", "desc")]
    [InlineData("date", "asc")]
    [InlineData("date", "desc")]
    public async Task GetAllUsers_ShouldSortResults_WhenQueryValid(string sort, string order)
    {
        EnableAuthentication("Admin");

        var users = (await DataFactory.CreateUsersAsync(5)).Select(u => u.ToResponse());
        
        var sortedUsers = sort switch
        {
            "email" => order == "asc"
                ? users.OrderBy(u => u.Email)
                : users.OrderByDescending(u => u.Email),
            "name" => order == "asc"
                ? users.OrderBy(u => u.LastName)
                : users.OrderByDescending(u => u.LastName),
            "date" => order == "asc"
                ? users.OrderBy(u => u.Joined)
                : users.OrderByDescending(u => u.Joined),
            _ => throw new ArgumentException()
        };

        var response = await Client.GetAsync($"/api/users?sort={sort}&order={order}");
    
        var body = await response.Content.ReadFromJsonAsync<UsersResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Users.Should().BeEquivalentTo(sortedUsers, o => o.WithStrictOrdering());
    }
    
    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 3)]
    public async Task GetAllUsers_ShouldPaginateResults_WhenQueryValid(int page, int limit)
    {
        EnableAuthentication("Admin");

        await DataFactory.CreateUsersAsync(10);
        
        var response = await Client.GetAsync($"/api/users?page={page}&limit={limit}");

        var body = await response.Content.ReadFromJsonAsync<UsersResponse>();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Users.Count.Should().Be(limit);
        body.HasNextPage.Should().Be(page * limit < 10);
        body.HasPreviousPage.Should().Be(page > 1);
    }
    
    [Fact]
    public async Task GetAllUsers_ShouldReturn401_WhenTokenInvalid()
    {
        DisableAuthentication();
        
        var response = await Client.GetAsync("/api/users");
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}