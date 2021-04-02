using System;

namespace Multi.Language.Application.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public UpdateUserViewModel()
        {

        }
        public UpdateUserViewModel(Guid userId, string password, string email)
        {
            Password = password;
            Email = email;
            UserId = userId;
        }

        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
