using Menu.Site.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Menu.Site.Models
{
    public class Product:BaseEntity
    {
        [StringLength(50)]
        public string ProductName { get; set; }
        public int Price { get; set; }
        [StringLength(200)]
        public string? Detail { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }


        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
