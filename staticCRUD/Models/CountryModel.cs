namespace staticCRUD.Models
{
    public class CountryModel
    {
        public int CountryID { get; set; }

        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
    public class CountryDropDownModel
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
}
