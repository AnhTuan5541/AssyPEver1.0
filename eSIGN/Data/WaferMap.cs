using System;
using System.Collections.Generic;

namespace WaferMapViewer.Data;

public partial class WaferMap
{
    public int Id { get; set; }

    public string? ProfileName { get; set; }

    public int? X { get; set; }

    public int? Y { get; set; }

    public string? IdCard { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? RowStart { get; set; }

    public int? ColumnNumberX { get; set; }

    public int? ColumnNumberY { get; set; }

    public int? LotRow { get; set; }

    public int? FrameIdRow { get; set; }
}
