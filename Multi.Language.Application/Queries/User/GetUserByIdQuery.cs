using System;
using System.Threading.Tasks;
using App.Core;
using Multi.Language.Application.ViewModels.User;

namespace Multi.Language.Application.Queries.User
{
    public class GetUserByIdQuery : QueryBase<UserViewModel>
    {
        private readonly Guid _userId;

        public GetUserByIdQuery(Guid userId)
        {
            _userId = userId;
        }
        internal override async Task<UserViewModel> Execute()
        {
            var user = await UnitOfWork.UserRepository.FindAsync(x => x.Id == _userId);

            if (user == null)
            {
                throw new DomainException("User not found", ExceptionLevel.Error);
            }

            var result = new UserViewModel(user.Id, user.UserName, user.Password, user.Email);

            return result;
        }
    }
}
