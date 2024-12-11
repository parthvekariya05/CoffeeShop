using System.Diagnostics.Metrics;

namespace staticCRUD.Models
{
    public class StateModel
    {
        public int StateID { get; set; }

        public int CountryID { get; set; }

        public string StateName { get; set; }

        public string StateCode { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

    }
    public class StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}