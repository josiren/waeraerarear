using System;
using System.Collections.Generic;

namespace EkzamenApp.Models;

public partial class WorkLog
{
    public int WorkLogId { get; set; }

    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public int HoursWorked { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
