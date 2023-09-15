using BiteBliss.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodFancy.Models.DataTables;

public class OrderHeader
{
    [Key]
    public int OrderHeaderId { get; set; }
    [Required]
    public string PickupName { get; set; }
    [Required]
    public string PickupPhoneNumber { get; set; }
    [Required]
    public string PickupEmail { get; set; }

    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    public ApplicationUser User { get; set; }
    public double OrderTotal { get; set; }

    public DateTime OrderDate { get; set; }
    public string StripePaymentIntentId { get; set; }
    public string PaymentStatus { get; set; }
    public int TotalItems { get; set; }

    public IEnumerable<OrderDetails> OrderDetails { get; set; }
}
