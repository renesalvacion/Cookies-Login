using Logs.DTO;
using Logs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logs.Services
{
    public class UserServices() : IUserServices
    {
        public async Task<(string message, bool success)> CreateUserAsync(UserRegisterDto dto)
        {
            try
            {
                using (var context = new UserContext())
                {
                    var user = new UsersCredential
                    {
                        Firstname = dto.Firstname,
                        Lastname = dto.Lastname,
                        Email = dto.Email
                    };

                    // Create password hasher
                    var passwordHasher = new PasswordHasher<UsersCredential>();

                    // Hash the password
                    user.Password = passwordHasher.HashPassword(user, dto.Password);

                    context.UsersCredentials.Add(user);
                    await context.SaveChangesAsync();

                    return ("User created successfully", true);
                }
            }
            catch (Exception ex)
            {
                return ($"Error creating user: {ex.Message}", false);
            }
        }


        public async Task<(UsersCredential? user, string message, bool success)> LoginUserAsync(UserLoginDto dto)
        {
            try
            {
                using (var context = new UserContext())
                {
                    var user = await context.UsersCredentials
                        .FirstOrDefaultAsync(u => u.Email == dto.Email);

                    if (user == null)
                        return (null, "User not found", false);

                    var passwordHasher = new PasswordHasher<UsersCredential>();

                    var result = passwordHasher.VerifyHashedPassword(
                        user,
                        user.Password,
                        dto.Password
                    );

                    if (result == PasswordVerificationResult.Success)
                        return (user, "Login successful", true);

                    return (null, "Invalid password", false);
                }
            }
            catch (Exception ex)
            {
                return (null, $"Error logging in: {ex.Message}", false);
            }
        }

        public async Task<(List<UsersCredential> users, string message, bool success)>
    GetAllUsersAsync()
        {
            try
            {
                using (var context = new UserContext())
                {
                    var users = await context.UsersCredentials.ToListAsync();

                    if (users == null || users.Count == 0)
                        return (new List<UsersCredential>(), "No users found", false);

                    return (users, "Users fetched successfully", true);
                }
            }
            catch (Exception ex)
            {
                return (new List<UsersCredential>(), $"Error fetching users: {ex.Message}", false);
            }
        }

    }
}
