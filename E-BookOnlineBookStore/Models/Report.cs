using System;
using System.Collections.Generic;

namespace E_BookOnlineBookStore.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int AdminId { get; set; }

    public string? ReportType { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public virtual Admin Admin { get; set; } = null!;
}
