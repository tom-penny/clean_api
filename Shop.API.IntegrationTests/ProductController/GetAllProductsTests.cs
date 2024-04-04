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
    public async Task GetAllProducts_ShouldReturn400_WhenSortInvalid()
    {
        var response = await Client.GetAsync("/api/products?sort=invalid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllProducts_ShouldReturn400_WhenOrderInvalid()
    {
        var response = await Client.GetAsync("/api/products?order=invalid");

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
    public async Task GetAllProducts_ShouldSortResults_WhenQueryValid(string sort, string order)
    {
        var products = (await DataFactory.CreateProductsAsync(5)).Select(p => p.ToResponse());
        
        var sortedProducts = sort switch
        {
            "name" => order == "asc"
                ? products.OrderBy(p => p.Name)
                : products.OrderByDescending(p => p.Name),
            "price" => order == "asc"
                ? products.OrderBy(p => p.Price)
                : products.OrderByDescending(p => p.Price),
            "date" => order == "asc"
                ? products.OrderBy(p => p.Created)
                : products.OrderByDescending(p => p.Created),
            _ => throw new ArgumentException()
        };

        var response = await Client.GetAsync($"/api/products?sort={sort}&order={order}");
    
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