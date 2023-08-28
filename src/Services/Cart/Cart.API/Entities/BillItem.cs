namespace Cart.API.Entities
{
    public class BillItem
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
    }
}
