using System;
using System.Collections.Generic;

namespace WaferMapViewer.Data;

public partial class WaferMapRaw
{
    public int Id { get; set; }

    public int? IdWaferMap { get; set; }

    public int? XRaw { get; set; }

    public int? YRaw { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }
}
