using System.ComponentModel.DataAnnotations;

namespace staticCRUD.Models
{
    public class BillModel
    {
        public int BillID { get; set; }
        [Required(ErrorMessage = "Plese Enter Bill Number")]
        public string BillNumber { get; set; }
        [Required(ErrorMessage = "Plese Enter Bill Date")]
        public DateTime BillDate { get; set; }
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Plese Enter TotalAmount")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Plese Enter Discount")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Plese Enter NetAmount")]
        public decimal NetAmount { get; set; }

        public int UserID { get; set; }
    }
}
