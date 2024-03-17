namespace Shop.Application.UnitTests.Users.Queries;

using Domain.Entities;
using Application.Users.Queries;

public class GetUserByIdTests
{
    private readonly TestDbContext _context;
    private readonly GetUserByIdHandler _handler;
    private readonly GetUserByIdValidator _validator;

    public GetUserByIdTests()
    {
        _context = new TestDbContext();
        _handler = new GetUserByIdHandler(_context);
        _validator = new GetUserByIdValidator();
    }
    
    [Fact]
    public async Task GetUserById_ShouldSucceed_WhenRequestValid()
    {
        var userId = Guid.NewGuid();

        var user1 = new User
        (
            id: new UserId(userId),
            firstName: "firstName1",
            lastName: "lastName1",
            email: "user1@test.com"
        );
        
        var user2 = new User
        (
            id: new UserId(Guid.NewGuid()),
            firstName: "firstName2",
            lastName: "lastName2",
            email: "user2@test.com"
        );
        
        _context.Users.AddRange(new List<User> { user1, user2 });
        
        await _context.SaveChangesAsync();

        var query = new GetUserByIdQuery(userId);

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(user1);
    }

    [Fact]
    public async Task GetUserById_ShouldFail_WhenUserNotFound()
    {
        var user1 = new User
        (
            id: new UserId(Guid.NewGuid()),
            firstName: "firstName1",
            lastName: "lastName1",
            email: "user1@test.com"
        );
        
        var user2 = new User
        (
            id: new UserId(Guid.NewGuid()),
            firstName: "firstName2",
            lastName: "lastName2",
            email: "user2@test.com"
        );
        
        _context.Users.AddRange(new List<User> { user1, user2 });
        
        await _context.SaveChangesAsync();

        var query = new GetUserByIdQuery(Guid.NewGuid());

        var result = await _handler.Handle(query, default);
        
        result.IsFailed.Should().BeTrue();
    }
    
    [Fact]
    public void GetUserById_ShouldReturnError_WhenIdEmpty()
    {
        var query = new GetUserByIdQuery(Guid.Empty);
        
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.UserId);
    }
}