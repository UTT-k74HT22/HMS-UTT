namespace HospitalManagement.entity.dto
    {
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public RegisterRequest(string username, string password, string fullname)
        {
            Username = username;
            Password = password;
            Fullname = fullname;
        }
    }
}