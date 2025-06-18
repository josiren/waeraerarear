using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class CarUsage
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public int UserId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
