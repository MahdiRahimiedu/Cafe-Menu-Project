using Menu.Site.Services.Base;

namespace Menu.Site.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<bool> ExistAsync(int id);
        Task<Response<T>> AddAsync(T model);
        Task<Response<T>> UpdateAsync(T model);
        Task<Response<T>> DeleteAsync(int id);
        Task<Response<T>> DeleteAsync(T model);
        Task<Response<T>> DeleteAsync(IEnumerable<T> models);
    }
}
