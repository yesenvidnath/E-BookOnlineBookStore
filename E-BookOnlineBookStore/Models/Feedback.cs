﻿using System;
using System.Collections.Generic;

namespace E_BookOnlineBookStore.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int BookId { get; set; }

    public int CustomerId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
