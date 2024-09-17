using System.ComponentModel.DataAnnotations;

namespace staticCRUD.Models
{
    public class CustomerModel
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Plese Enter CustomerName ")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Plese Enter  HomeAddress")]
        public string HomeAddress { get; set; }
        [Required(ErrorMessage = "Plese Enter  Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Plese Enter MobilePhone")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Plese Enter GstNo")]
        public string GSTNO { get; set; }
        [Required(ErrorMessage = "Plese Enter CityName")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Plese Enter PinCode")]
        public string PinCode { get; set; }
        [Required(ErrorMessage = "Plese Enter NetAmount")]
        public decimal NetAmount { get; set; }
     
        public int UserID { get; set; }

    }
    public class CustomerDropDownModel
    {   
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
