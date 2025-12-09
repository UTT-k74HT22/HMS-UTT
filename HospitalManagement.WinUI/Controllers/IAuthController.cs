namespace HospitalManagement.WinUI.Controllers;

public interface IAuthController
{
    void ShowLogin();
    void ShowRegister();
    Task HandleLoginAsync(string username, string password);
    Task HandleRegisterAsync(string username, string password, string role);
}
