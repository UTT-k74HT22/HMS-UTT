namespace HospitalManagement.router;

public class AuthContextManager
{
    public static long? UserId { get; private set; }
    public static string? Role { get; private set; }
    public static string? Username { get; private set; }

    public static void SetUser(long userId, string role, string username)
    {
        UserId = userId;
        Role = role;
        Username = username;
    }

    public static void Clear()
    {
        UserId = null;
        Role = null;
        Username = null;
    }
}