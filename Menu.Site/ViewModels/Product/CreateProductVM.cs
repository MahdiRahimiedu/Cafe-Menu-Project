using System.ComponentModel.DataAnnotations;

namespace Menu.Site.ViewModels.Product
{
    public class CreateProductVM
    {
        [StringLength(50)]
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string? Detail { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
    }
}
