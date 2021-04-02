using System;
using System.Threading.Tasks;
using App.Core;
using Multi.Language.Application.ViewModels.User;

namespace Multi.Language.Application.Queries.User
{
    public class GetUserForUpdateQuery:QueryBase<UpdateUserViewModel>
    {
        private readonly Guid _userId;

        public GetUserForUpdateQuery(Guid userId)
        {
            _userId = userId;
        }
        internal override async Task<UpdateUserViewModel> Execute()
        {
            var user = await UnitOfWork.UserRepository.FindAsync(x => x.Id == _userId);

            if (user == null)
            {
                throw new DomainException("User not found", ExceptionLevel.Error);
            }

            var result = new UpdateUserViewModel(user.Id, user.Password, user.Email);

            return result;
        }
    }
}
