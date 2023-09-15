using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.Models.DataTables.Dto;

public class OrderHeaderUpdateDTO
{
    public int OrderHeaderId { get; set; }
    public string PickupName { get; set; }
    public string PickupPhoneNumber { get; set; }
    public string PickupEmail { get; set; }

    public DateTime OrderDate { get; set; }
    public string StripePaymentIntentId { get; set; }
    public string Status { get; set; }
}
