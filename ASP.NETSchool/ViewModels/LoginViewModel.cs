namespace ASP.NETSchool.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool Remember {  get; set; }
    }
}
