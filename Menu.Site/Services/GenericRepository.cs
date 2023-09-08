using Menu.Site.Models;
using Menu.Site.Repository;
using Menu.Site.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Menu.Site.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MenuDbContext _context;
        public GenericRepository(MenuDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<bool> ExistAsync(int id)
        {
            return await GetAsync(id) != null;
        }
        public async Task<Response<T>> AddAsync(T model)
        {
            var respons = new Response<T>();
            try
            {
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                respons.Success = true;
                respons.Message = "The product has been successfully registered";
                respons.Data = model;
                return respons;
            }
            catch (Exception err)
            {
                respons.Success = false;
                respons.Message = "The product was not successfully registered";
                respons.Data = model;
                respons.ValidationErrors = err.Message;
                return respons;
            }
        }
        public async Task<Response<T>> DeleteAsync(int id)
        {
            var respons = new Response<T>();
            try
            {
                _context.Remove(await GetAsync(id));
                await _context.SaveChangesAsync();
                respons.Success = true;
                respons.Message = "The product has been successfully removed";
                return respons;
            }
            catch (Exception err)
            {
                respons.Success = false;
                respons.Message = "The product was not successfully removed";
                respons.ValidationErrors = err.Message;
                return respons;
            }

        }
        public async Task<Response<T>> DeleteAsync(T model)
        {
            var respons = new Response<T>();
            try
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
                respons.Success = true;
                respons.Message = "The product has been successfully removed";
                return respons;
            }
            catch (Exception err)
            {
                respons.Success = false;
                respons.Message = "The product was not successfully removed";
                respons.ValidationErrors = err.Message;
                respons.Data = model;
                return respons;
            }
        }
        public async Task<Response<T>> DeleteAsync(IEnumerable<T> models)
        {
            var respons = new Response<T>();
            try
            {
                _context.RemoveRange(models);
                await _context.SaveChangesAsync();
                respons.Success = true;
                respons.Message = "The product has been successfully removed";
                return respons;
            }
            catch (Exception err)
            {
                respons.Success = false;
                respons.Message = "The product was not successfully removed";
                respons.ValidationErrors = err.Message;
                return respons;
            }
        }
        public async Task<Response<T>> UpdateAsync(T model)
        {
            var respons = new Response<T>();
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                respons.Success = true;
                respons.Message = "The product has been updated successfully";
                return respons;
            }
            catch (Exception err)
            {
                respons.Success = false;
                respons.Message = "The product was not updated successfully";
                respons.ValidationErrors = err.Message;
                return respons;
            }
        }
        public async Task<Response<T>> UpdateAsync(IEnumerable<T> models)
        {
            var respons = new Response<T>();
            try
            {
                _context.UpdateRange(models);
                await _context.SaveChangesAsync();
                respons.Success = true;
                respons.Message = "The product has been updated successfully";
                return respons;
            }
            catch (Exception err)
            {
                respons.Success = false;
                respons.Message = "The product was not updated successfully";
                respons.ValidationErrors = err.Message;
                return respons;
            }
        }
    }
}
