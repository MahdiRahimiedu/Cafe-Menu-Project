using Menu.Site.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace Menu.Site.ViewModels.Product
{
    public class UpdateProductVM: BaseVM
    {
        [StringLength(50)]
        public string ProductName { get; set; }
        public int Price { get; set; }
        [StringLength(100)]
        public string? Detail { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
    }
}
