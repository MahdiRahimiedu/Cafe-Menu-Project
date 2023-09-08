using Menu.Site.Models;
using Menu.Site.Services.Base;
using Menu.Site.ViewModels.User;

namespace Menu.Site.Repository
{
    public interface IAdminUserRepository:IGenericRepository<AdminUser>
    {
        Task<bool> CheckLogin(LoginVM vm);
        Task<AdminUser> GetByUsernameAsync(string username);
        Task<Response<ChangePasswordVM>> ChangePassword(ChangePasswordVM vm);
        string GetHashCode(string pass);
    }
}
