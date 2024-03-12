namespace Shop.Domain.UnitTests.Entities;

using Domain.Entities;
using Domain.Events;

public class UserTests
{
    [Fact]
    public void NewUser_ShouldAddUserRegisteredEvent()
    {
        var user = new User
        (
            id: new UserId(Guid.NewGuid()),
            firstName: "firstName",
            lastName: "last",
            email: "test@test.com"
        );

        user.Events.Should().Contain(e =>
            e.GetType() == typeof(UserRegistered));
    }
}