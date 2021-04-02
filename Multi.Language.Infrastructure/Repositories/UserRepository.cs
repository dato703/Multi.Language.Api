using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Infrastructure.Repositories
{
    public class UserRepository:Repository<User>,IUserRepository
    {
        public UserRepository(UserContext context) : base(context)
        {
        }
    }
}
