

using System.Security.Cryptography.X509Certificates;

namespace TaskManager.Api.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
}