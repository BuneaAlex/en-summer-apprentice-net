using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Models;

public partial class TicketCategory
{
    public int TicketCategoryid { get; set; }

    public string? Description { get; set; }

    public int? NoAvailable { get; set; }

    public double? Price { get; set; }

    public int? Eventid { get; set; }

    public virtual Event? Event { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string ToString()
    {
        string eventInfo = Event != null ? $"Event: {Event.Name} (ID: {Event.Eventid}), " : "Event: null, ";

        return $"TicketCategoryID: {TicketCategoryid}, " +
               $"{eventInfo}" +
               $"Description: {Description ?? "null"}, " +
               $"NoAvailable: {NoAvailable}, " +
               $"Price: {Price}, " +
               $"OrderCount: {Orders.Count}";
    }
}
