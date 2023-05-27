using System;
using System.Collections.Generic;

namespace BookSalesApp.Models;

public partial class Book
{
    public int Id { get; set; }

    public string? Imageurl { get; set; }

    public string Fileurl { get; set; } = null!;

    public int Genreid { get; set; }

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool? Status { get; set; }

    public double Price { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Genre Genre { get; set; } = null!;
}
