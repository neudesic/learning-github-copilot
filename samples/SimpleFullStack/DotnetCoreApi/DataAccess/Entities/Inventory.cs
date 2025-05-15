namespace copoilot_sample.DataAccess.Entities
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation property
        public Product? Product { get; set; }
    }
}
