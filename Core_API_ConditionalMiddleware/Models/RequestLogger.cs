using System;
using System.Collections.Generic;

namespace Core_API_ConditionalMiddleware.Models;

public partial class RequestLogger
{
    public int RequestUniqueId { get; set; }

    public string? RequestId { get; set; }

    public DateTime RequestDateTime { get; set; }

    public string RequestMethod { get; set; } = null!;

    public string RequestPath { get; set; } = null!;

    public string RequestBody { get; set; } = null!;
}
