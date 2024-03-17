namespace Shop.Application.UnitTests.Payments.Queries;

using Domain.Entities;
using Domain.Enums;
using Application.Payments.Queries;

public class GetPaymentByIdTests
{
    private readonly TestDbContext _context;
    private readonly GetPaymentByIdHandler _handler;
    private readonly GetPaymentByIdValidator _validator;
    
    public GetPaymentByIdTests()
    {
        _context = new TestDbContext();
        _handler = new GetPaymentByIdHandler(_context);
        _validator = new GetPaymentByIdValidator();
    }
    
    [Fact]
    public async Task GetPaymentById_ShouldSucceed_WhenRequestValid()
    {
        var paymentId = Guid.NewGuid();

        var payment1 = new Payment
        (
            id: new PaymentId(paymentId),
            checkoutId: new CheckoutId("checkoutId1"),
            status: PaymentStatus.Approved,
            amount: 50m
        );

        var payment2 = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId2"),
            status: PaymentStatus.Approved,
            amount: 50m
        );

        _context.Payments.AddRange(new List<Payment> { payment1, payment2 });
        
        await _context.SaveChangesAsync();

        var query = new GetPaymentByIdQuery(paymentId);

        var result = await _handler.Handle(query, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(payment1);
    }

    [Fact]
    public async Task GetPaymentById_ShouldFail_WhenPaymentNotFound()
    {
        var payment1 = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId1"),
            status: PaymentStatus.Approved,
            amount: 50m
        );

        var payment2 = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId2"),
            status: PaymentStatus.Approved,
            amount: 50m
        );
        
        _context.Payments.AddRange(new List<Payment> { payment1, payment2 });
        
        await _context.SaveChangesAsync();

        var query = new GetPaymentByIdQuery(Guid.NewGuid());

        var result = await _handler.Handle(query, default);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void GetPaymentById_ShouldReturnError_WhenIdEmpty()
    {
        var query = new GetPaymentByIdQuery(Guid.Empty);
        
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}