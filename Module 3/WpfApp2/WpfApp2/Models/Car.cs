using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class Car
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public string LicensePlate { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<CarUsage> CarUsages { get; set; } = new List<CarUsage>();
}
