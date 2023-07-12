using System.ComponentModel.DataAnnotations;

namespace ProductsApp.Models
{
    public class Product
    {
        public int? Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [Range(0, 1000.0)]
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
