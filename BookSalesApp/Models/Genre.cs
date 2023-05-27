using System;
using System.Collections.Generic;

namespace BookSalesApp.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? Status { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
