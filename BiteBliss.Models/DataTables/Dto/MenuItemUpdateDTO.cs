using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.Models.DataTables.Dto;

public class MenuItemUpdateDTO
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string SepcialTag { get; set; }
    [Required]
    public IFormFile File { get; set; }
    public string Category { get; set; }
    [Range(1, int.MaxValue)]
    public double Price { get; set; }
}
