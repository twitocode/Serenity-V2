using Serenity.Database.Entities;

namespace Serenity.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}