namespace Shop.API.IntegrationTests.ProductController;

using API.Models.Requests;
using API.Models.Responses;

[Collection("TestCollection")]
public class GetAllProductsTests : TestBase
{
    private readonly Faker<CreateProductRequest> _faker;

    public GetAllProductsTests(ShopApiFactory factory) : base(factory)
    {
        _faker = new Faker<CreateProductRequest>()
            .RuleFor(r => r.Name, f => f.Commerce.ProductName())
            .RuleFor(r => r.Stock, f => f.Random.Int(1, 100))
            .RuleFor(r => r.Price, f => f.Finance.Amount(1m, 100m))
            .RuleFor(r => r.CategoryIds, new List<Guid> { Guid.NewGuid() });
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturn200_WhenRequestValid()
    {
        var createdProducts = await CreateProductsAsync(5);

        var response = await Client.GetAsync("/api/products");

        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Should().BeEquivalentTo(createdProducts);
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
    [InlineData("price", "asc")]
    [InlineData("price", "desc")]
    [InlineData("date", "asc")]
    [InlineData("date", "desc")]
    public async Task GetAllProducts_ShouldSortResults_WhenQueryValid(string sortBy, string orderBy)
    {
        var createdProducts = await CreateProductsAsync(5);

        var sortedProducts = sortBy switch
        {
            "name" => orderBy == "asc"
                ? createdProducts.OrderBy(p => p.Name).ToList()
                : createdProducts.OrderByDescending(p => p.Name).ToList(),
            "price" => orderBy == "asc"
                ? createdProducts.OrderBy(p => p.Price).ToList()
                : createdProducts.OrderByDescending(p => p.Price).ToList(),
            "date" => orderBy == "asc"
                ? createdProducts.OrderBy(p => p.Created).ToList()
                : createdProducts.OrderByDescending(p => p.Created).ToList(),
            _ => throw new ArgumentException()
        };

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