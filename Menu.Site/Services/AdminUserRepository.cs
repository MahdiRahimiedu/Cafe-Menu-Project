using Menu.Site.Models;
using Menu.Site.Repository;
using Menu.Site.Services.Base;
using Menu.Site.ViewModels.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Menu.Site.Services
{
    public class AdminUserRepository : GenericRepository<AdminUser>, IAdminUserRepository
    {
        private readonly MenuDbContext _context;

        public AdminUserRepository(MenuDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckLogin(LoginVM vm)
        {
            try
            {
               var user = await _context.Users.SingleOrDefaultAsync(p=>p.UserName == vm.Username&& p.PassHash == vm.Pass);
               return user != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Response<ChangePasswordVM>> ChangePassword(ChangePasswordVM vm)
        {
            var response = new Response<ChangePasswordVM>();
            try
            {
                AdminUser user = await _context.Users.SingleOrDefaultAsync(p => p.UserName == vm.Username);
                user.PassHash = GetHashCode(GetHashCode(vm.Pass));
                await _context.SaveChangesAsync();
                response.Success = true;
                return response;

            }
            catch (Exception err)
            {
                response.Success = true;
                response.ValidationErrors = err.Message;
                return response;
            }
        }
        public string GetHashCode(string pass)
        {
            Byte[] mainBytes;
            Byte[] encodeBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            mainBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodeBytes = md5.ComputeHash(mainBytes);
            return BitConverter.ToString(encodeBytes);
        }

        public async Task<AdminUser> GetByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(p => p.UserName == username);
        }
    }
}
