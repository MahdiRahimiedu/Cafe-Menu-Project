using Menu.Site.Models;

namespace Menu.Site.ViewModels
{
    public class Dashboard
    {
        public Dashboard(IEnumerable<Models.Product> products, IEnumerable<Models.Category> categories)
        {
            Products = products;
            Categories = categories;
        }

        public IEnumerable<Models.Product> Products { get; set; }
        public IEnumerable<Models.Category> Categories{ get; set; }
    }
}
