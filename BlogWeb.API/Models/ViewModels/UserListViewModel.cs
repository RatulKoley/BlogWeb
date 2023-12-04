using BlogWeb.API.Models.Helpers;

namespace BlogWeb.API.Models.ViewModels
{
    public class UserListViewModel : CommonModel
    {
        public List<UserModel> UserList { get; set; }
        public NewUser newUserInfo { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
    public class NewUser
    {
        public RegisterViewModel newRegister { get; set; }
        public bool isAdmin { get; set; }
    }
}
