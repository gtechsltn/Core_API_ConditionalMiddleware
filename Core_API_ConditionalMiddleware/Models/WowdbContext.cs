using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core_API_ConditionalMiddleware.Models;

public partial class WowdbContext : DbContext
{
    public WowdbContext()
    {
    }

    public WowdbContext(DbContextOptions<WowdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<RequestLogger> RequestLoggers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:testserverms.database.windows.net,1433;Initial Catalog=wowdb;Persist Security Info=False;User ID=maheshadmin;Password=P@ssw0rd_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpensesId).HasName("pk_expenses_id");

            entity.Property(e => e.ExpensesType).HasMaxLength(255);
            entity.Property(e => e.PaidBy).HasMaxLength(255);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(255);
            entity.Property(e => e.VendorName).HasMaxLength(255);
        });

        modelBuilder.Entity<RequestLogger>(entity =>
        {
            entity.HasKey(e => e.RequestUniqueId).HasName("PK__RequestL__B69E57623A11D987");

            entity.ToTable("RequestLogger");

            entity.Property(e => e.RequestBody).IsUnicode(false);
            entity.Property(e => e.RequestDateTime).HasColumnType("datetime");
            entity.Property(e => e.RequestId)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.RequestMethod)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RequestPath)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
