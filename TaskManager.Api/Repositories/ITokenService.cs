

using TaskManager.Api.Models;

namespace TaskManager.Api.Repositories;

public interface ITokenService
{
    // recebe um user e devolve a string do token
    string CreateToken(User user);
}