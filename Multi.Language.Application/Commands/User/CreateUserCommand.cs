using System.Threading.Tasks;
using App.Core;
using Multi.Language.Application.ViewModels.User;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Application.Commands.User
{
    public class CreateUserCommand : CommandBase
    {
        private readonly UserCreateViewModel _userViewModel;

        public CreateUserCommand(UserCreateViewModel userViewModel)
        {
            _userViewModel = userViewModel;
        }
        internal override async Task Execute()
        {
            var user = new Domain.UserAggregate.User();
            user.Create(_userViewModel.UserName, _userViewModel.Password, _userViewModel.Email, UserRole.User);
            //user.Validate();
            await UnitOfWork.UserRepository.AddAsync(user);
            await UnitOfWork.CompleteAsync();

            HttpResult.Successful("User Registered");
            HttpResult.AddParameter("user-id", user.Id);
        }
    }
}
