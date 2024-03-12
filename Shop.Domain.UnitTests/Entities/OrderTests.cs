namespace Shop.Domain.UnitTests.Entities;

using Domain.Entities;
using Domain.Enums;
using Domain.Events;

public class OrderTests
{
    private readonly Order _order;

    public OrderTests()
    {
        _order = new Order
        (
            id: new OrderId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId"),
            userId: new UserId(Guid.NewGuid()),
            status: OrderStatus.Pending,
            amount: 50m
        );
    }
    
    [Fact]
    public void SetConfirmed_ShouldSetStatusToConfirmed()
    {
        _order.SetConfirmed();

        _order.Status.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public void SetConfirmed_ShouldAddOrderConfirmedEvent()
    {
        _order.SetConfirmed();

        _order.Events.Should().Contain(e =>
            e.GetType() == typeof(OrderConfirmed));
    }

    [Fact]
    public void SetConfirmed_ShouldBeIdempotent()
    {
        _order.SetConfirmed();

        _order.SetConfirmed();

        _order.Events.OfType<OrderConfirmed>().Count().Should().Be(1);
    }
    
    [Fact]
    public void SetProcessing_ShouldSetStatusToProcessing()
    {
        _order.SetProcessing();

        _order.Status.Should().Be(OrderStatus.Processing);
    }

    [Fact]
    public void SetProcessing_ShouldAddOrderProcessingEvent()
    {
        _order.SetProcessing();

        _order.Events.Should().Contain(e =>
            e.GetType() == typeof(OrderProcessing));
    }
    
    [Fact]
    public void SetProcessing_ShouldBeIdempotent()
    {
        _order.SetProcessing();

        _order.SetProcessing();

        _order.Events.OfType<OrderProcessing>().Count().Should().Be(1);
    }
    
    [Fact]
    public void SetCompleted_ShouldSetStatusToCompleted()
    {
        _order.SetCompleted();

        _order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void SetCompleted_ShouldAddOrderCompletedEvent()
    {
        _order.SetCompleted();

        _order.Events.Should().Contain(e =>
            e.GetType() == typeof(OrderCompleted));
    }
    
    [Fact]
    public void SetCompleted_ShouldBeIdempotent()
    {
        _order.SetCompleted();

        _order.SetCompleted();

        _order.Events.OfType<OrderCompleted>().Count().Should().Be(1);
    }
    
    [Fact]
    public void SetCancelled_ShouldSetStatusToCancelled()
    {
        _order.SetCancelled();

        _order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void SetCancelled_ShouldAddOrderCancelledEvent()
    {
        _order.SetCancelled();

        _order.Events.Should().Contain(e =>
            e.GetType() == typeof(OrderCancelled));
    }
    
    [Fact]
    public void SetCancelled_ShouldBeIdempotent()
    {
        _order.SetCancelled();

        _order.SetCancelled();

        _order.Events.OfType<OrderCancelled>().Count().Should().Be(1);
    }

    [Fact]
    public void AddPayment_ShouldUpdatePaymentId()
    {
        var payment = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId"),
            status: PaymentStatus.Approved,
            amount: 50m
        );
        
        _order.AddPayment(payment);

        _order.PaymentId.Should().Be(payment.Id);
    }

    [Fact]
    public void AddPayment_ShouldSetStatusToConfirmed_WhenPaymentApproved()
    {
        var payment = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId"),
            status: PaymentStatus.Approved,
            amount: 50m
        );
        
        _order.AddPayment(payment);

        _order.Status.Should().Be(OrderStatus.Confirmed);
    }
    
    [Fact]
    public void AddPayment_ShouldSetStatusToCancelled_WhenPaymentRejected()
    {
        var payment = new Payment
        (
            id: new PaymentId(Guid.NewGuid()),
            checkoutId: new CheckoutId("checkoutId"),
            status: PaymentStatus.Rejected,
            amount: 50m
        );
        
        _order.AddPayment(payment);

        _order.Status.Should().Be(OrderStatus.Cancelled);
    }
}