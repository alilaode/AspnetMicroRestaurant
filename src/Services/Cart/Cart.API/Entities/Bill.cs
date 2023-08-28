namespace Cart.API.Entities
{
    public class Bill
    {
        public string UserName { get; set; }
        public List<BillItem> Items { get; set; } = new List<BillItem>();

        public Bill()
        {
        }

        public Bill(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }
                return totalprice;
            }
        }
    }
}
