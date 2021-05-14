using MediatR;

namespace Multi.Language.Domain.Events.User
{
    public class CreateUserDomainEvent: INotification
    {
        public CreateUserDomainEvent(UserAggregate.User user)
        {
            User = user;
        }

        public UserAggregate.User User { get; }
    }
}
