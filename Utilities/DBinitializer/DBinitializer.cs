using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Utilities.DBinitializer
{
    public class DBinitializer : IBDinitializer
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<DBinitializer> _logger;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Appliccationusr> _userManager;

        public DBinitializer(ApplicationDBContext context, ILogger<DBinitializer> logger,Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
          Microsoft.AspNetCore.Identity.UserManager<Appliccationusr> userManager)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initializ()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }

                if(_roleManager.Roles.IsNullOrEmpty())
                {
                    _roleManager.CreateAsync(new(DS.SUPER_ADMIN_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(DS.ADMIN_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(DS.EMPLOYEE_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(DS.CUSTOMER_ROLE)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new Appliccationusr()
                    {
                        Email="superadmin@hoda.com",
                        UserName ="superadmin",
                        EmailConfirmed=true,
                        firstname="super",
                        lastname="admin",
                        PhoneNumber="0122345324494",

                    },"Admin123$").GetAwaiter().GetResult();
                }
               var user = _userManager.FindByNameAsync("superadmin").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user!, DS.SUPER_ADMIN_ROLE);

            }
            catch (Exception ex)
            {
                _logger.LogError($"error{ex.Message}");
            }
        }
    }
}
