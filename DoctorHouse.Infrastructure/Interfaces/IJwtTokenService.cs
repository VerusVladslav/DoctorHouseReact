using DoctorHouse.DAL.Entities;

namespace DoctorHouse.Infrastructure.Interfaces
{
    public interface IJwtTokenService
    {
        string CreateToken(DbUser dbUser);
    }
}