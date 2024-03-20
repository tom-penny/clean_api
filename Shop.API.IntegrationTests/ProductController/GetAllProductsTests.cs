namespace Shop.API.IntegrationTests.ProductController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class GetAllProductTests : TestBase
{
    private readonly Faker<CreateProductRequest> _faker;

    public GetAllProductTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateProductRequest>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Stock, f => f.Random.Int(1, 100))
            .RuleFor(p => p.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(p => p.CategoryIds, _ => new List<Guid> { Guid.NewGuid() });
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturn200_WhenRequestValid()
    {
        EnableAuthentication("Admin");

        var createRequest = _faker.Generate();
        
        var createResponse = await Client.PostAsJsonAsync("/api/products", createRequest);

        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();

        var response = await Client.GetAsync("/api/products");

        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Single().Should().BeEquivalentTo(createdProduct);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturn400_WhenSortByInvalid()
    {
        var response = await Client.GetAsync("/api/products?sortBy=invalid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllProducts_ShouldReturn400_WhenOrderByInvalid()
    {
        var response = await Client.GetAsync("/api/products?orderBy=invalid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllProducts_ShouldReturn400_WhenPageInvalid()
    {
        var response = await Client.GetAsync("/api/products?page=0");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public async Task GetAllProducts_ShouldReturn400_WhenSizeInvalid(int size)
    {
        var response = await Client.GetAsync($"/api/products?size={size}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    public async Task GetAllProducts_ShouldSortByName_WhenQueryValid(string sortBy, string orderBy)
    {
        var products = await CreateProductsAsync(5);
        
        var sortedProducts = orderBy == "asc"
            ? products.OrderBy(p => p.Name).ToList()
            : products.OrderByDescending(p => p.Name).ToList();
        
        var response = await Client.GetAsync($"/api/products?sortBy={sortBy}&orderBy={orderBy}");
        
        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Should().BeEquivalentTo(sortedProducts, o => o.WithStrictOrdering());
    }
    
    [Theory]
    [InlineData("price", "asc")]
    [InlineData("price", "desc")]
    public async Task GetAllProducts_ShouldSortByPrice_WhenQueryValid(string sortBy, string orderBy)
    {
        var products = await CreateProductsAsync(5);
        
        var sortedProducts = orderBy == "asc"
            ? products.OrderBy(p => p.Price).ToList()
            : products.OrderByDescending(p => p.Price).ToList();
        
        var response = await Client.GetAsync($"/api/products?sortBy={sortBy}&orderBy={orderBy}");
        
        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Should().BeEquivalentTo(sortedProducts, o => o.WithStrictOrdering());
    }
    
    [Theory]
    [InlineData("date", "asc")]
    [InlineData("date", "desc")]
    public async Task GetAllProducts_ShouldSortByDate_WhenQueryValid(string sortBy, string orderBy)
    {
        var products = await CreateProductsAsync(5);
        
        var sortedProducts = orderBy == "asc"
            ? products.OrderBy(p => p.Created).ToList()
            : products.OrderByDescending(p => p.Created).ToList();
        
        var response = await Client.GetAsync($"/api/products?sortBy={sortBy}&orderBy={orderBy}");
        
        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Should().BeEquivalentTo(sortedProducts, o => o.WithStrictOrdering());
    }
    
    private async Task<List<ProductResponse>> CreateProductsAsync(int count)
    {
        EnableAuthentication("Admin");

        var products = new List<ProductResponse>();
        
        for (var i = 0; i < count; i++)
        {
            var createRequest = _faker.Generate();
            
            var createResponse = await Client.PostAsJsonAsync("/api/products", createRequest);
            
            createResponse.EnsureSuccessStatusCode();
            
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            
            products.Add(createdProduct!);
        }
        
        return products;
    }
}