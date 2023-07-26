using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Models;

public partial class EventType
{
    public int EventTypeid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
