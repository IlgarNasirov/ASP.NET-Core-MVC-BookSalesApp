using System;
using System.Collections.Generic;

namespace BookSalesApp.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public int Bookid { get; set; }

    public bool? Status { get; set; }

    public bool Issold { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
