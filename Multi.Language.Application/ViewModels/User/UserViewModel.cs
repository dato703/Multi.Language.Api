using System;

namespace Multi.Language.Application.ViewModels.User
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }
        public UserViewModel(Guid userId, string userName, string password, string email)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
