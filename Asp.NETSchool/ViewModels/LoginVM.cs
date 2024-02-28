namespace Asp.NETSchool.ViewModels {
    public class LoginVM {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool Remember { get; set; }
    }
}
