using DoctorHouse.DAL.Entities;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoctorHouse.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<DbUser> _userStore;
        public ProfileService(UserManager<DbUser> userStore)
        {
            this._userStore = userStore;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = long.Parse(context.Subject.GetSubjectId());
            var user = this._userStore.Users
                .SingleOrDefault(x => x.Id == id);
            if (user != null)
            {
                // get user roles
                var roles = await _userStore.GetRolesAsync(user);
                var userClaims = new List<Claim>();
                foreach (var role in roles)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                context.IssuedClaims.AddRange(userClaims);
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var id = long.Parse(context.Subject.GetSubjectId());
            var user = this._userStore.Users
                .SingleOrDefault(x => x.Id == id);
            context.IsActive = user != null;
            return Task.CompletedTask;
        }
    }
    //    public class ProfileService : IProfileService
    //    {
    //        private readonly UserManager<DbUser> _userStore;

    //        public ProfileService(UserManager<DbUser> userStore)
    //        {
    //            this._userStore = userStore;
    //        }
    //        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    //        {
    //            //context.LogProfileRequest(Logger);

    //            //if (context.RequestedClaimTypes.Any())
    //            //{
    //            //    var user = await this._userStore.FindByIdAsync(context.Subject.GetSubjectId());

    //            //    if (user != null)
    //            //    {
    //            //        var userClaims = await this.GetUserClaims(user.Id.ToString());
    //            //        context.AddRequestedClaims(userClaims);
    //            //    }
    //            //}

    //            //context.LogIssuedClaims(Logger);
    //            context.AddRequestedClaims(new List<Claim>() { new Claim(ClaimTypes.Role, "Admin") });
    //        }
    //        public async Task<List<Claim>> GetUserClaims(string userId)
    //        {
    //            var userClaims = new List<Claim>();
    //            var user = await _userStore.FindByIdAsync(userId);
    //            //if (user == null)
    //            //{
    //            //    throw new BusinessException("invalid user id");
    //            //}
    //            //var userRoles = await _userRolesSrc.GetUserRoles(userId);

    //            //userClaims.AddRange(userRoles.Select(r => new Claim(JwtClaimTypes.Role, r.Name)));
    //            //userClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
    //            //if (!string.IsNullOrEmpty(user.FirstName))
    //            //    userClaims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
    //            //if (!string.IsNullOrEmpty(user.LastName))
    //            //    userClaims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
    //            //var name = $"{user.FirstName ?? ""} {user.LastName ?? ""}";
    //            //if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(name.Trim()))
    //            //    userClaims.Add(new Claim(JwtClaimTypes.Name, name));

    //            userClaims.Add(new Claim(JwtClaimTypes.Role, "Admin"));
    //            return userClaims;
    //        }

    //        public async Task IsActiveAsync(IsActiveContext context)
    //        {
    //            var user = await this._userStore.FindByIdAsync(context.Subject.GetSubjectId());
    //            context.IsActive = user != null;
    //        }
    //    }
}
