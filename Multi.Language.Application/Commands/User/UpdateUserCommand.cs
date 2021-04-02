using System.Threading.Tasks;
using App.Core;
using Multi.Language.Application.ViewModels.User;

namespace Multi.Language.Application.Commands.User
{
    public class UpdateUserCommand : CommandBase
    {
        private readonly UpdateUserViewModel _model;

        public UpdateUserCommand(UpdateUserViewModel model)
        {
            _model = model;
        }
        internal override async Task Execute()
        {
            var user = await UnitOfWork.UserRepository.FindAsync(x => x.Id == _model.UserId);
            if (user == null)
            {
                throw new DomainException("User Not found", ExceptionLevel.Error);
            }

            user.Update(_model.Password, _model.Email);

            await UnitOfWork.UserRepository.UpdateAsync(user);
            await UnitOfWork.CompleteAsync();

            HttpResult = HttpResult.Successful("Person  Updated");
        }
    }
}
