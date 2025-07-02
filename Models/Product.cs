using System.ComponentModel.DataAnnotations;

namespace apidappermvcproje.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        
        // Navigation property (Dapper için)
        public Category? Category { get; set; }
    }
} 