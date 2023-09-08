using Menu.Site.Models;
using Menu.Site.Services.Base;

namespace Menu.Site.Repository
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<bool> GetIsActiveCategoryAsync(int id);

        Task<Product> GetWithDetailAsync(int id);
        Task<IEnumerable<Product>> GetAllWithDetailSortedAsync();
        Task<int> PriorityMax(int id);
        Task<IEnumerable<Product>> GetByCategoryId(int id);
        Task<Response<IEnumerable<int>>> UpdatePrioritiesAsync(List<int> ids);

    }
}
