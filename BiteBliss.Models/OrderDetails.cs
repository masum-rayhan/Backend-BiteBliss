﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.Models.DataTables;

public class OrderDetails
{
    [Key]
    public int OrderDetailsId { get; set; }
    [Required]
    public int OrderHeaderId { get; set; }
    [Required]
    public int MenuItemId { get; set; }
    [ForeignKey("MenuItemId")]
    public MenuItem MenuItem { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public string ItemName { get; set; }
    [Required]
    public double Price { get; set; }
}
