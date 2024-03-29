using BasicConnectApi.Models;

namespace BasicConnectApi.Services;

public interface IUserService
{
    int? RegisterUser(string? firstName, string? lastName, string? email, string? password, bool isEmailConfirmed = false);
    bool AuthenticateUser(string? email, string? password, out int id);
    Task<bool> ExistsUser(string? email);
    Task<int?> GetUserId(string? email);
    Task<bool> ResetPassword(int userId, string? password);
    Task<UserResponse?> GetUserById(int userId);
    Task<UserResponse?> UpdateUser(int userId, string? firstName, string? lastName, string? email);
    Task<bool> UpdatePassword(int userId, string? oldPassword, string? newPassword);
}
