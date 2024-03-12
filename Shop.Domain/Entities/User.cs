namespace Shop.Domain.Entities;

using Events;
using Primitives;

public record UserId(Guid Value);

public class User : BaseEntity
{
    public UserId Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; private set; }
    public DateTime Joined { get; init; }
    
    public User(UserId id, string firstName, string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Joined = DateTime.UtcNow;
        
        AddDomainEvent(new UserRegistered(this));
    }
    
    private User() {}
}