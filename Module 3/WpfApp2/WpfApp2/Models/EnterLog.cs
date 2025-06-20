﻿using System;
using System.Collections.Generic;

namespace WpfApp2.Models;

public partial class EnterLog
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
