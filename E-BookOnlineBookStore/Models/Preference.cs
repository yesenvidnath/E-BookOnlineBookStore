using System;
using System.Collections.Generic;

namespace E_BookOnlineBookStore.Models;

public partial class Preference
{
    public int PreferenceId { get; set; }

    public int UserId { get; set; }

    public string? Genre { get; set; }

    public string? PreferredFormat { get; set; }

    public string? ReadingFrequency { get; set; }

    public virtual User User { get; set; } = null!;
}
