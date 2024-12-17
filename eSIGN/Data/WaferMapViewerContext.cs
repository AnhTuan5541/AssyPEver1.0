using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WaferMapViewer.Data;

public partial class WaferMapViewerContext : DbContext
{
    public WaferMapViewerContext()
    {
    }

    public WaferMapViewerContext(DbContextOptions<WaferMapViewerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LogInfo> LogInfos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<WaferMap> WaferMaps { get; set; }

    public virtual DbSet<WaferMapRaw> WaferMapRaws { get; set; }

    public virtual DbSet<WaferMapValue> WaferMapValues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=10.201.21.84,50150;Initial Catalog=WaferMapViewer;Persist Security Info=True;User ID=cimitar2;Password=TFAtest1!2!;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("log_info_pk");

            entity.ToTable("log_info");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Function)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("function");
            entity.Property(e => e.IdCard)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("id_card");
            entity.Property(e => e.Info)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("info");
            entity.Property(e => e.TypeLog)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type_log");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pk");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_PK");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Company)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("department");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("display_name");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdCard)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("id_card");
            entity.Property(e => e.IdCardManager)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("id_card_manager");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_role_pk");

            entity.ToTable("user_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<WaferMap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("wafer_map", tb => tb.HasTrigger("TrgDeleteWaferMap"));

            entity.Property(e => e.ColumnNumberX).HasColumnName("column_number_x");
            entity.Property(e => e.ColumnNumberY).HasColumnName("column_number_y");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.FrameIdRow).HasColumnName("frame_id_row");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdCard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_card");
            entity.Property(e => e.LotRow).HasColumnName("lot_row");
            entity.Property(e => e.ProfileName)
                .HasMaxLength(100)
                .HasColumnName("profile_name");
            entity.Property(e => e.RowStart).HasColumnName("row_start");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");
        });

        modelBuilder.Entity<WaferMapRaw>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("wafer_map_raw");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdWaferMap).HasColumnName("id_wafer_map");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.XRaw).HasColumnName("x_raw");
            entity.Property(e => e.YRaw).HasColumnName("y_raw");
        });

        modelBuilder.Entity<WaferMapValue>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("wafer_map_value");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.FrameId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("frame_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdWaferMap).HasColumnName("id_wafer_map");
            entity.Property(e => e.LotValue)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("lot_value");
            entity.Property(e => e.UnitId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("unit_id");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.ValueX).HasColumnName("value_x");
            entity.Property(e => e.ValueY).HasColumnName("value_y");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
