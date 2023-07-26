using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Models;

public partial class Event
{
    public int Eventid { get; set; }

    public string? Description { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Image { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public int? EventTypeid { get; set; }

    public int? Venueid { get; set; }

    public virtual EventType? EventType { get; set; }

    public virtual ICollection<TicketCategory> TicketCategories { get; set; } = new List<TicketCategory>();

    public virtual Venue? Venue { get; set; }
}
