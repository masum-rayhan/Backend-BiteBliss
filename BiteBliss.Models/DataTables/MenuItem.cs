using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodFancy.Models.DataTables;

public class MenuItem
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string SpecialTag { get; set; }
    [Required]
    public string Image { get; set; }
    public string Category { get; set; }
    [Range(1, int.MaxValue)]
    public double Price { get; set; }
}
