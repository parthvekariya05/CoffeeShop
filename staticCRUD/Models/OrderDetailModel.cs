using System.ComponentModel.DataAnnotations;

namespace staticCRUD.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailID { get; set; }
        [Required(ErrorMessage = "Plese Enter Order ID")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Plese Enter Product ID")]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Plese Enter Quantity")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Plese Enter Amount")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Plese Enter Total Amount")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Plese User ID")]
        public int UserID { get; set; }
    }
}