﻿using APi.Data;
using APi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APi.Extensions
{
    public class IdentityServiceExtension
    {
        //    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        //    {
        //        services.AddIdentityCore<AppUser>(opt =>
        //        {
        //            opt.Password.RequireNonAlphanumeric = false;
        //        })
        //            .AddRoles<AppRole>()
        //            .AddRoleManager<RoleManager<AppRole>>()
        //            .AddSignInManager<SignInManager<AppUser>>()
        //            .AddRoleValidator<RoleValidator<AppRole>>()
        //            .AddEntityFrameworkStores<DataContext>();

        //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
        //                ValidateIssuer = false,
        //                ValidateAudience = false,
        //            };
        //        });

        //        services.AddAuthorization(opt =>
        //        {
        //            opt.AddPolicy("RequiredAdminRole", policy => policy.RequireRole("Admin"));
        //            opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
        //        });

        //        return services;
        //    }
        //}
    }
}