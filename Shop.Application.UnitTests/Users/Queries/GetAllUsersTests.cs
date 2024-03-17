namespace Shop.Application.UnitTests.Users.Queries;

using Domain.Entities;
using Application.Users.Queries;

public class GetAllUsersTests
{
    private readonly TestDbContext _context;
    private readonly GetAllUsersHandler _handler;
    private readonly GetAllUsersValidator _validator;

    public GetAllUsersTests()
    {
        _context = new TestDbContext();
        _handler = new GetAllUsersHandler(_context);
        _validator = new GetAllUsersValidator();
    }

    [Fact]
    public async Task GetAllUsers_ShouldSucceed_WhenRequestValid()
    {
        var users = new List<User>
        {
            new
            (
                id: new UserId(Guid.NewGuid()),
                firstName: "firstName1",
                lastName: "lastName1",
                email: "user1@test.com"
            ),
            new
            (
                id: new UserId(Guid.NewGuid()),
                firstName: "firstName2",
                lastName: "lastName2",
                email: "user2@test.com"
            )
        };

        _context.Users.AddRange(users);

        await _context.SaveChangesAsync();

        var query = new GetAllUsersQuery
        {
            SortBy = "name",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(2);
    }

    [Fact]
    public void GetAllUsers_ShouldReturnError_WhenSortByInvalid()
    {
        var query = new GetAllUsersQuery
        {
            SortBy = "invalid",
            OrderBy = "asc",
            Page = 1,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.SortBy);
    }
    
    [Fact]
    public void GetAllUsers_ShouldReturnError_WhenOrderByInvalid()
    {
        var query = new GetAllUsersQuery
        {
            SortBy = "name",
            OrderBy = "invalid",
            Page = 1,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.OrderBy);
    }
    
    [Fact]
    public void GetAllUsers_ShouldReturnError_WhenPageZero()
    {
        var query = new GetAllUsersQuery
        {
            SortBy = "name",
            OrderBy = "asc",
            Page = 0,
            Size = 10
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.Page);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(26)]
    public void GetAllUsers_ShouldReturnError_WhenSizeInvalid(int size)
    {
        var query = new GetAllUsersQuery
        {
            SortBy = "name",
            OrderBy = "asc",
            Page = 1,
            Size = size
        };

        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(q => q.Size);
    }
}