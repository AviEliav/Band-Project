namespace BandProject.Models
{
    public class CartItem
    {
        public int Id{ get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
