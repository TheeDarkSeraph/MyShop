﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models {
    public class Category {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}