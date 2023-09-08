using AutoMapper;
using Menu.Site.Models;
using Menu.Site.ViewModels.Category;
using Menu.Site.ViewModels.Product;

namespace Menu.Site
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region Category

            CreateMap<Category, CreateCategoryVM>().ReverseMap();
            CreateMap<Category, UpdateCategoryVM>().ReverseMap();

            CreateMap<Product, CreateProductVM>().ReverseMap();
            CreateMap<Product, UpdateProductVM>().ReverseMap();

            //CreateMap<UpdateCategoryVM, CategoryVM>().ReverseMap();
            //CreateMap<CategoryVM, CreateCategoryVM>().ReverseMap();

            #endregion
        }
    }
}
