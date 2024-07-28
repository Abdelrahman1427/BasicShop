namespace BasicShop.DTO
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } // Property to hold the product name
        public decimal Price { get; set; } // Property to hold the product price if needed
    }

}
