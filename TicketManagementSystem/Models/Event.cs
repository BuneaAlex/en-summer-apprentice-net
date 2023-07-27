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

    public override string ToString()
    {
        string eventTypeInfo = EventType != null ? $"EventType: {EventType.Name} (ID: {EventType.EventTypeid}), " : "EventType: null, ";
        string venueInfo = Venue != null ? $"Venue: {Venue.Location} (ID: {Venue.Venueid}), " : "Venue: null, ";

        return $"EventID: {Eventid}, " +
               $"{eventTypeInfo}" +
               $"{venueInfo}" +
               $"Name: {Name ?? "null"}, " +
               $"Description: {Description ?? "null"}, " +
               $"StartDate: {StartDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "null"}, " +
               $"EndDate: {EndDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "null"}, " +
               $"Image: {Image ?? "null"}";
    }
}
