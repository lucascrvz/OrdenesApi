using Microsoft.AspNetCore.Identity;
using Domain.Entities;

public class UserService
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public string HashPassword(User user, string plainPassword)
    {
        return _passwordHasher.HashPassword(user, plainPassword);
    }

    public PasswordVerificationResult VerifyPassword(User user, string plainPassword, string hashedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, plainPassword);
    }
}
