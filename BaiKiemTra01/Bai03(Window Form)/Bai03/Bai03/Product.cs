namespace TechMartProductManager
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }

        public Product() { }

        public Product(string productId, string productName, int categoryId, string categoryName, decimal unitPrice, int quantity, string imagePath)
        {
            ProductId = productId;
            ProductName = productName;
            CategoryId = categoryId;
            CategoryName = categoryName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            ImagePath = imagePath;
        }
    }
}