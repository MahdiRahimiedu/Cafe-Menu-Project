using Menu.Site.ViewModels.Base;

namespace Menu.Site.ViewModels.Category
{
    public class UpdateCategoryVM: BaseVM
    {
        public string CategoryName { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsActive { get; set; }
    }
}
