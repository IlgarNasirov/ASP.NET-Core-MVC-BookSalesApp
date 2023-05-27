using System;
using System.Collections.Generic;

namespace BookSalesApp.Models;

public partial class Payment
{
    public int Id { get; set; }

    public double Totalamount { get; set; }

    public int Userid { get; set; }

    public DateTime Paymentdate { get; set; }

    public string Paymentid { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
