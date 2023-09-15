using FoodFancy.Models.DataTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.Models.DataTables;

public class CartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }
    public int MenuItemId { get; set; }
    [ForeignKey("MenuItemId")]
    public MenuItem MenuItem { get; set; }
    public int Quantity { get; set; }
}
