namespace Webshop_Backend.Services;

// Webshop_Backend/Services/IPasswordHasher.cs
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string passwordHash, string inputPassword);
}

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    public bool Verify(string passwordHash, string inputPassword) => 
        BCrypt.Net.BCrypt.EnhancedVerify(inputPassword, passwordHash);
}
