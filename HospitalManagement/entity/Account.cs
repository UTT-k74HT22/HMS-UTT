namespace HospitalManagement.entity
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Fullname { get; set; } = "";
        public string Role { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
