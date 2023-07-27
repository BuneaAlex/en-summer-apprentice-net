using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int? NumberOfTickets { get; set; }

    public DateTime? OrderedAt { get; set; }

    public double? TotalPrice { get; set; }

    public int? Customerid { get; set; }

    public int? TicketCategoryid { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual TicketCategory? TicketCategory { get; set; }

    public override string ToString()
    {
        string customerInfo = Customer != null ? $"Customer: {Customer.CustomerName} (ID: {Customer.Customerid}), " : "Customer: null, ";
        string ticketCategoryInfo = TicketCategory != null ? $"TicketCategory: {TicketCategory.GetType} (ID: {TicketCategory.TicketCategoryid}), " : "TicketCategory: null, ";

        return $"OrderID: {Orderid}, " +
               $"{customerInfo}" +
               $"{ticketCategoryInfo}" +
               $"NumberOfTickets: {NumberOfTickets}, " +
               $"OrderedAt: {OrderedAt}, " +
               $"TotalPrice: {TotalPrice}";
    }
}
