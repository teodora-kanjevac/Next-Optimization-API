namespace NextOptimization.Data.Models
{
    public class Appointment
    {
        public string Id { get; set; }
        public string BuyerId { get; set; }
        public User Buyer { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public string Status { get; set; }
    }
}
