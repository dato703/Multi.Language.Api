using App.Core;

namespace Multi.Language.Domain.Events.User
{
    public class CreateUserDomainEvent : DomainEvent
    {
        public CreateUserDomainEvent(AggregatesModel.UserAggregate.User user) : base(user.Id)
        {
            User = user;
        }

        public AggregatesModel.UserAggregate.User User { get; }
    }
}
