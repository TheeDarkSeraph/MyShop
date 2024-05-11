using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models {
    // holds product information
    public class Product {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required, Range(0.1, 999999.99)]
        public decimal Price { get; set; }
        [Required, DisplayName("Product Image")]
        public string? Base64Img { get; set; }
        [Required, Range(0, 99999)]
        public int Amount { get; set; }
        public bool Featured { get; set; } = false;
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; } = null!;
        

    }
}
