namespace Menu.Site.ViewModels.Category
{
    public class CreateCategoryVM
    {
        public string CategoryName { get; set; }
        public IFormFile Image { get; set; }
        public bool IsActive { get; set; }
    }
}
