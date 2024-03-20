namespace Shop.API.IntegrationTests.ProductController;

using API.Mappings;
using API.Models.Responses;

public class GetAllProductsTests : TestBase
{
    public GetAllProductsTests(ShopApiFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllProducts_ShouldReturn200_WhenRequestValid()
    {
        var products = (await DataFactory.CreateProductsAsync(5)).Select(p => p.ToResponse());
        
        var response = await Client.GetAsync("/api/products");

        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Should().BeEquivalentTo(products);
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
        var products = (await DataFactory.CreateProductsAsync(5)).Select(p => p.ToResponse());
        
        var sortedProducts = sortBy switch
        {
            "name" => orderBy == "asc"
                ? products.OrderBy(p => p.Name)
                : products.OrderByDescending(p => p.Name),
            "price" => orderBy == "asc"
                ? products.OrderBy(p => p.Price)
                : products.OrderByDescending(p => p.Price),
            "date" => orderBy == "asc"
                ? products.OrderBy(p => p.Created)
                : products.OrderByDescending(p => p.Created),
            _ => throw new ArgumentException()
        };

        var response = await Client.GetAsync($"/api/products?sortBy={sortBy}&orderBy={orderBy}");
    
        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Should().BeEquivalentTo(sortedProducts, o => o.WithStrictOrdering());
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 3)]
    public async Task GetAllProducts_ShouldPaginateResults_WhenQueryValid(int page, int size)
    {
        await DataFactory.CreateProductsAsync(10);
        
        var response = await Client.GetAsync($"/api/products?page={page}&size={size}");

        var body = await response.Content.ReadFromJsonAsync<ProductsResponse>();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Products.Count.Should().Be(size);
        body.HasNextPage.Should().Be(page * size < 10);
        body.HasPreviousPage.Should().Be(page > 1);
    }
}