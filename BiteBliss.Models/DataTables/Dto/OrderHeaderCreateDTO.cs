using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.Models.DataTables.Dto;

public class OrderHeaderCreateDTO
{
    [Required]
    public string PickupName { get; set; }
    [Required]
    public string PickupPhoneNumber { get; set; }
    [Required]
    public string PickupEmail { get; set; }

    public string ApplicationUserId { get; set; }
    public double OrderTotal { get; set; }

    public string StripePaymentIntentId { get; set; }
    public string Status { get; set; }
    public int TotalItems { get; set; }

    public IEnumerable<OrderDetailsCreateDTO> OrderDetailsDTO { get; set; }
}
