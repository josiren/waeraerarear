using System;
using System.Collections.Generic;

namespace EkzamenApp.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
