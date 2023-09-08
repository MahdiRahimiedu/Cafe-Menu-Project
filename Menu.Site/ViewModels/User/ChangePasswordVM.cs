using System.ComponentModel.DataAnnotations;

namespace Menu.Site.ViewModels.User
{
    public class ChangePasswordVM
    {
        [Required]
        public string Username{ get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(7)]
        public string Pass { get; set; }
        [MinLength(7)]
        [Compare("Pass")]
        [DataType(DataType.Password)]
        [Required]
        public string RepPass { get; set; }
    }
}
