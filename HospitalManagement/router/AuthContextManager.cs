namespace HospitalManagement.router;

public static class AuthContextManager
{
    public static long? UserProfileId { get; private set; }
    public static string? Role { get; private set; }
    public static string? Username { get; private set; }

    public static bool IsAuthenticated => UserProfileId.HasValue;

    public static void SetUser(long userProfileId, string role, string username)
    {
        UserProfileId = userProfileId;
        Role = role;
        Username = username;
    }

    public static void Clear()
    {
        UserProfileId = null;
        Role = null;
        Username = null;
    }
}