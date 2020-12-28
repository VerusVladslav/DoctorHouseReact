using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DoctorHouse.DAL;
using DoctorHouse.DAL.Entities;
using DoctorHouse.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DoctorHouse.Infrastructure.Services
{

    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly EFContext _context;
        private readonly IConfiguration _configuration;

        public JwtTokenService(UserManager<DbUser> userManager, EFContext context,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public string CreateToken(DbUser dbUser)
        {
            var roles = _userManager.GetRolesAsync(dbUser).Result;
            roles = roles.OrderBy(x => x).ToList();
            var query = _context.Users.AsQueryable();
          


            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", dbUser.Id.ToString()),
                new Claim("name", dbUser.UserName),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("JwtKey")));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                expires: DateTime.Now.AddDays(1000),
                claims: claims
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}