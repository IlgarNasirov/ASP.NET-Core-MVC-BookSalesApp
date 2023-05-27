using System;
using System.Collections.Generic;

namespace BookSalesApp.Models;

public partial class User
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public bool Isadmin { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
