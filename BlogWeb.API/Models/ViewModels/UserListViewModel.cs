namespace BlogWeb.API.Models.ViewModels
{
    public class UserListViewModel
    {
        public List<UserModel> UserList { get; set; }
        public NewUser newUserInfo { get; set; }
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
