using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Palavyr.Core.Validators
{
    public class NotificationHandlerValidationDecorator<TEvent> : INotificationHandler<TEvent> where TEvent : INotification
    {
        private readonly INotificationHandler<TEvent> inner;
        private readonly INotificationValidator<TEvent> validator;

        public NotificationHandlerValidationDecorator(INotificationHandler<TEvent> inner, INotificationValidator<TEvent> validator)
        {
            this.inner = inner;
            this.validator = validator;
        }

        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            await validator.Validate(notification);
            await inner.Handle(notification, cancellationToken);
        }
    }
}