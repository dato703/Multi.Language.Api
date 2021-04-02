using System;
using System.Threading.Tasks;
using App.Core;

namespace Multi.Language.Application.Commands.User
{
    public class DeleteUserCommand:CommandBase
    {
        private readonly Guid _userId;

        public DeleteUserCommand(Guid userId)
        {
            _userId = userId;
        }
        internal override async Task Execute()
        {
            var user = await UnitOfWork.UserRepository.FindAsync(x => x.Id == _userId);
            if (user == null)
            {
                throw new DomainException("User Not found", ExceptionLevel.Error);
            }

            UnitOfWork.UserRepository.Remove(user);
            await UnitOfWork.CompleteAsync();

            HttpResult = HttpResult.Successful("User Removed!");
        }
    }
}
