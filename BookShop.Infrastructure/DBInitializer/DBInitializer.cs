using BookShop.Core.Domain.Entities;
using BookShop.Infrastructure.DataBaseContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Infrastructure.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManeger;
        private readonly ApplicationContext _dbContext;
        private readonly IConfiguration _config;

        public DBInitializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManeger,
            ApplicationContext dbContext,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManeger = roleManeger;
            _dbContext = dbContext;
            _config = configuration;
        }

        public async Task InitializeDB()
        {
            try
            {
                if (_dbContext.Database.GetMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception) { }

            if (!(await _roleManeger.RoleExistsAsync("Admin")))
            {
                await _roleManeger.CreateAsync(new IdentityRole<Guid>("Admin"));
                await _roleManeger.CreateAsync(new IdentityRole<Guid>("Customer"));

                string? personName = _config.GetValue<string>("AdminSettings:PersonName");
                string? email = _config.GetValue<string>("AdminSettings:Email");
                string? password = _config.GetValue<string>("AdminSettings:Password");
                string? phone = _config.GetValue<string>("AdminSettings:PhoneNumber");

                var result = await _userManager
                    .CreateAsync(new ApplicationUser()
                                 {
                                     UserName = email,
                                     PersonName = personName,
                                     Email = email,
                                     PhoneNumber = phone
                                 }, password);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
