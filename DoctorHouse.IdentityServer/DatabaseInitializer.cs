using DoctorHouse.DAL.Entities;
using DoctorHouse.Helper;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoctorHouse.IdentityServer
{
    public static class DatabaseInitializer
    {
        public static object IdentityServerConfiguration { get; private set; }

        public static void Init(IServiceProvider services)
        {
            SeederDB.SeedDataByAS(services);

            //using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{

            //    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            //    context.Database.Migrate();
            //    if (!context.Clients.Any())
            //    {
            //        foreach (var client in Config.Clients)
            //        {
            //            context.Clients.Add(client.ToEntity());
            //        }
            //        context.SaveChanges();
            //    }

            //    if(!context.ApiResources.Any())
            //    {
            //        foreach (var apiResource in Config.ApiResources)
            //        {
            //            context.ApiResources.Add(apiResource.ToEntity());
            //        }
            //        context.SaveChanges();
            //    }

            //    if (!context.IdentityResources.Any())
            //    {
            //        foreach (var resource in Config.IdentityResources)
            //        {
            //            context.IdentityResources.Add(resource.ToEntity());
            //        }
            //        context.SaveChanges();
            //    }

            //    if (!context.ApiScopes.Any())
            //    {
            //        foreach (var resource in Config.ApiScopes)
            //        {
            //            context.ApiScopes.Add(resource.ToEntity());
            //        }
            //        context.SaveChanges();
            //    }
            //    //if (!context.ApiResources.Any())
            //    //{
            //    //    foreach (var resource in Config.ApiScopes)
            //    //    {
            //    //        context.ApiResources.Add(resource.ToEntity());
            //    //    }
            //    //    context.SaveChanges();
            //    //}



            //    //var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
            //    //var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<DbRole>>();
            //    //SeederDB.SeedData(manager, managerRole);
            //}

        }
    }
}
