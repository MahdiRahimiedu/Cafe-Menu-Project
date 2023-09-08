using Menu.Site.Models;
using Menu.Site.Repository;
using Menu.Site.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Menu.Site.Services
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly MenuDbContext _context;

        public CategoryRepository(MenuDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllSortedAsync()
        {
            return await _context.Categories.OrderBy(p => p.Priority).ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetAllSortedWithProductsAsync()
        {
            return await _context.Categories.Where(p=>p.IsActive == true).Include(p=>p.Products).OrderBy(p => p.Priority).ToListAsync();
        }

        public async Task IsActiveProductsAsync(Category model)
        {
            foreach (Product item in await _context.Products.Where(p => p.CategoryId == model.Id).ToListAsync())
            {
                item.IsActive = model.IsActive;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> PriorityMaxAsync()
        {
            if (await _context.Categories.AnyAsync())
                return await _context.Categories.MaxAsync(p => p.Priority) + 1;
            return 1;
        }
        public async Task<Response<IEnumerable<int>>> UpdatePrioritiesAsync(List<int> ids)
        {
            var response = new Response<IEnumerable<int>>();
            
            try
            {
                var products = new List<Category>();
                for (int i = 0; i < ids.Count; i++)
                {
                    var product = await _context.Categories.FirstOrDefaultAsync(p => p.Id == ids[i]);
                    if (product != null)
                    {
                        product.Priority = i + 1;
                        products.Add(product);
                    }
                }

                await _context.SaveChangesAsync();

                var priorities = await _context.Categories.OrderBy(p => p.Priority).Select(p => p.Priority).ToListAsync();
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

        public async Task<Response<IEnumerable<Category>>> GetAllCustomAsync(List<int> ids)
        {
            var response = new Response<IEnumerable<Category>>();

            try
            {
                List<Category> categories = new List<Category>();
                foreach (var item in ids)
                {
                    categories.Add(await _context.Categories.SingleOrDefaultAsync(p => p.Id == item));
                }
                
                response.Success = true;
                response.Data = categories;
                return response;

            }
            catch (Exception err)
            {
                response.Success = false;
                response.ValidationErrors = err.Message;
                return response;
            }
        }

    }
}
