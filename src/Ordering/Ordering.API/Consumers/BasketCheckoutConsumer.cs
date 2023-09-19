using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.API.Consumers;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    public Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        throw new NotImplementedException();
    }
}