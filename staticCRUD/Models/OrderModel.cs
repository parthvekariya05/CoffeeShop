using System.ComponentModel.DataAnnotations;

namespace staticCRUD.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public string OrderNumber {  get; set; }
        [Required(ErrorMessage = "Plese Enter Date Time")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Plese Enter Customer ID")]
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Plese Enter Payment Method")]
        public string PaymentMode { get; set; }
        [Required(ErrorMessage = "Plese Enter Total Amount")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Plese Enter ShippingAddress")]
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "Plese Enter User ID")]
        public int UserID { get; set; }
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }

    }
}
