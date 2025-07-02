using System.ComponentModel.DataAnnotations;

namespace apidappermvcproje.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string? Status { get; set; } = "Pending";
        
        // Navigation properties (Dapper için)
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
    }
} 