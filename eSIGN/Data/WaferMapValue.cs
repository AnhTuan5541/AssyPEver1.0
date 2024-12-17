using System;
using System.Collections.Generic;

namespace WaferMapViewer.Data;

public partial class WaferMapValue
{
    public long Id { get; set; }

    public string? UnitId { get; set; }

    public string? FrameId { get; set; }

    public int? ValueX { get; set; }

    public int? ValueY { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? IdWaferMap { get; set; }

    public string? LotValue { get; set; }
}
