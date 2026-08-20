using System.ComponentModel.DataAnnotations;

namespace BookNest_API.DTOs
{
    public class CreateBookDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Author { get; set; }

        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        [Range(0,10000)]
        public int Stock { get; set; }


    }
}
