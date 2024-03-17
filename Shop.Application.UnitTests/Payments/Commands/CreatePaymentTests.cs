namespace Shop.Application.UnitTests.Payments.Commands;

using Domain.Entities;
using Application.Payments.Commands;

public class CreatePaymentTests
{
    private readonly TestDbContext _context;
    private readonly CreatePaymentHandler _handler;
    private readonly CreatePaymentValidator _validator;

    public CreatePaymentTests()
    {
        _context = new TestDbContext();
        _handler = new CreatePaymentHandler(_context);
        _validator = new CreatePaymentValidator();
    }

    [Fact]
    public async Task CreatePayment_ShouldSucceed_WhenRequestValid()
    {
        var command = new CreatePaymentCommand
        {
            CheckoutId = "checkoutId",
            Status = "COMPLETED",
            Amount = 50m
        };
        
        var result = await _handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void CreatePayment_ShouldReturnError_WhenCheckoutIdEmpty()
    {
        var command = new CreatePaymentCommand
        {
            CheckoutId = string.Empty,
            Status = "COMPLETED",
            Amount = 10m
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.CheckoutId);
    }

    [Fact]
    public void CreatePayment_ShouldReturnError_WhenStatusEmpty()
    {
        var command = new CreatePaymentCommand
        {
            CheckoutId = "checkoutId",
            Status = string.Empty,
            Amount = 10m
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Status);
    }
    
    [Fact]
    public void CreatePayment_ShouldReturnError_WhenAmountZero()
    {
        var command = new CreatePaymentCommand
        {
            CheckoutId = "checkoutId",
            Status = "COMPLETED",
            Amount = decimal.Zero
        };
        
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Amount);
    }
}