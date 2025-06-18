using System;
using System.Collections.Generic;

namespace EkzamenApp.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public int ClientId { get; set; }

    public string ProjectManager { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly CompletionDate { get; set; }

    public string? TaskDescription { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
}
