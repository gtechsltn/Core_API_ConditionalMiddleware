using System;
using System.Collections.Generic;

namespace Core_API_ConditionalMiddleware.Models;

public partial class Expense
{
    public double ExpensesId { get; set; }

    public string? VendorName { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? ExpensesType { get; set; }

    public double? AmountPaid { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaidBy { get; set; }
}
