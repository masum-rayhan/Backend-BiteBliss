using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.Models.DataTables.Dto;

public class OrderDetailsCreateDTO
{
    [Required]
    public int MenuItemId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public string ItemName { get; set; }
    [Required]
    public double Price { get; set; }
}
