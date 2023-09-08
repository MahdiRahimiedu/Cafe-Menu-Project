using Menu.Site.Models.Base;

namespace Menu.Site.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
