using Menu.Site.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Menu.Site.Models
{
    public class AdminUser:BaseEntity
    {
        [StringLength(20)]
        [MinLength(7)]
        public string UserName { get; set; }
        public string PassHash { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public int AccessFailedCount { get; set; }
    }
}
