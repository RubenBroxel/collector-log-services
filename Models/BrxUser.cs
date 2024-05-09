using System;
using System.Collections.Generic;

namespace CQRS.Models;

public partial class BrxUser
{
    public int? Iduser { get; set; }

    public string? Nameuser { get; set; }

    public string? Lastnameuser { get; set; }

    public int? Ageuser { get; set; }

    public DateOnly? Birthuser { get; set; }

    public string? Activeuser { get; set; }

    public DateTime? Createdata { get; set; }

    public DateTime? Updatedata { get; set; }

    public DateTime? Deletedata { get; set; }
}
