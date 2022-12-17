﻿namespace ABCVehicleShowroom.Models
{
    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

    [Table("Brands")]
    public class modelBrand
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }

        
        public DateTime? Created_at { get; set; }

        public int? Created_by { get; set; }

        public DateTime? Updated_at { get; set; }

        public int? Updated_by { get; set; }

        public int? Status { get; set; }
    }
}