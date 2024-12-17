using System;
using System.Collections.Generic;

namespace WaferMapViewer.Data;

public partial class User
{
    public int Id { get; set; }

    public string? IdCard { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Title { get; set; }

    public string? Department { get; set; }

    public string? Company { get; set; }

    public string? DisplayName { get; set; }

    public string? IdCardManager { get; set; }
}
