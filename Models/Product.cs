using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProsNCats.Models {
    public class Product {
        [Key]
        public int ProductId {get; set;}

        [Required]
        [MinLength(2, ErrorMessage = "Product name should be at least 2 characters")]
        public string Name {get; set;}

        [Required]
        [MinLength(10, ErrorMessage = "Product description should be at least 10 characters")]
        public string Description {get; set;}

        [Required]
        [Range(0, 10000000000)]        
        public decimal Price {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        public List<Association> Associations {get; set;} // = new List<Association>();
    }
}