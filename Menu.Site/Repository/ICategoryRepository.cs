using Menu.Site.Models;
using Menu.Site.Services.Base;

namespace Menu.Site.Repository
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        //Task<Response<Category>> IsActiveToggle(int id);
        Task IsActiveProductsAsync(Category model);
        Task<IEnumerable<Category>> GetAllSortedAsync();
        Task<int> PriorityMaxAsync();
        Task<Response<IEnumerable<int>>> UpdatePrioritiesAsync(List<int> ids);
        Task<IEnumerable<Category>> GetAllSortedWithProductsAsync();
        Task<Response<IEnumerable<Category>>> GetAllCustomAsync(List<int> ids);

    }
}

