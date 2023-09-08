using Menu.Site.Models;
using Menu.Site.Repository;
using Menu.Site.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Menu.Site.Services
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly MenuDbContext _context;

        public ProductRepository(MenuDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailSortedAsync()
        {
            return await _context.Products.Include(p => p.Category).OrderBy(p=>p.CategoryId).ThenBy(p=>p.Priority).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryId(int id)
        {
            return await _context.Products.Where(p => p.CategoryId == id).OrderBy(p=>p.Priority).ToListAsync();
        }

        public async Task<bool> GetIsActiveCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category.IsActive;
        }

        public async Task<Product> GetWithDetailAsync(int id)
        {
            return await _context.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);

        }

        public async Task<int> PriorityMax(int id)
        {
            if (await _context.Products.Where(p => p.CategoryId == id).AnyAsync())
                return await _context.Products.Where(p=>p.CategoryId == id).MaxAsync(p => p.Priority) + 1;
            return 1;
        }

        public async Task<Response<IEnumerable<int>>> UpdatePrioritiesAsync(List<int> ids)
        {
            var response = new Response<IEnumerable<int>>();

            try
            {
                var products = new List<Product>();
                for (int i = 0; i < ids.Count; i++)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ids[i]);
                    if (product != null)
                    {
                        product.Priority = i + 1;
                        products.Add(product);
                    }
                }

                await _context.SaveChangesAsync();

                var priorities = await _context.Products.OrderBy(p => p.Priority).Select(p => p.Priority).ToListAsync();
                response.Success = true;
                response.Data = priorities;
                return response;

            }
            catch (Exception err)
            {
                response.Success = false;
                response.ValidationErrors = err.Message;
                response.Data = ids;
                return response;
            }
        }
    }
}
