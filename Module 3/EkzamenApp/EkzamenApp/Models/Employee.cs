using System;
using System.Collections.Generic;

namespace EkzamenApp.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Department { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int WeeklyHours { get; set; }

    public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
}
