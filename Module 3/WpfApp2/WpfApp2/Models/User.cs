using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int RoleId { get; set; }

    public bool IsBanned { get; set; }

    public bool IsFirstLogin { get; set; }

    public virtual ICollection<CarUsage> CarUsages { get; set; } = new List<CarUsage>();

    public virtual ICollection<EnterLog> EnterLogs { get; set; } = new List<EnterLog>();

    public virtual Role Role { get; set; } = null!;
}
