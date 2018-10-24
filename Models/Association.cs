using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProsNCats.Models {
    public class Association {
        [Key]
        public int Id {get; set;}

        public int ProductId {get; set;} 
        [ForeignKey("ProductId")]
        public Product Product {get; set;}

        public int CategoryId {get; set;} 
        [ForeignKey("CategoryId")]
        public Category Category {get; set;}
    }
}