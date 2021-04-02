using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Multi.Language.Application.ViewModels.User;

namespace Multi.Language.Application.Queries.User
{
    public class GetUsersQuery : QueryBase<List<UserViewModel>>
    {
        internal override async Task<List<UserViewModel>> Execute()
        {
            var users = await UnitOfWork.UserRepository.GetAllAsync();
            var result = users.Select(x => new UserViewModel
            {
                UserId = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email
            }).ToList();
            return result;
        }
    }
}
