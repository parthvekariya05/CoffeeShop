using System.ComponentModel.DataAnnotations;

namespace staticCRUD.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "Plese Enter User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Plese Enter Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Plese Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Plese Enter Mobile No")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Plese Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Plese Enter Is Active")]
        public bool IsActive { get; set; }
    }
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
