  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  namespace Backend.Models
  {
      public class Cart
      {
          [Key]
          public int Pid { get; set; }  // Primary key for Cart

          public int cid { get; set; } // Cart ID (could be used for other purposes, like internal references)

          [ForeignKey("User_detail")]
          public int Uid { get; set; }  // Foreign key for User_detail

          public virtual User_detail? user { get; set; }  // Navigation property to User_detail

          public virtual List<Item> item { get; set; } = new List<Item>(); // One-to-many relationship: a Cart has many Items

          [Required]
          public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date when the cart was created
      }
  }
