using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Models;

public partial class CommandUsersBroxelContext : DbContext
{
    public CommandUsersBroxelContext()
    {
    }

    public CommandUsersBroxelContext(DbContextOptions<CommandUsersBroxelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BrxUser> BrxUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=10.100.8.29; TrustServerCertificate=True;Encrypt=false; User ID=sa;Password=Az19882009; Initial Catalog=CommandUsersBroxel;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrxUser>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Activeuser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("activeuser");
            entity.Property(e => e.Ageuser).HasColumnName("ageuser");
            entity.Property(e => e.Birthuser).HasColumnName("birthuser");
            entity.Property(e => e.Createdata)
                .HasColumnType("datetime")
                .HasColumnName("createdata");
            entity.Property(e => e.Deletedata)
                .HasColumnType("datetime")
                .HasColumnName("deletedata");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Lastnameuser)
                .HasMaxLength(50)
                .HasColumnName("lastnameuser");
            entity.Property(e => e.Nameuser)
                .HasMaxLength(50)
                .HasColumnName("nameuser");
            entity.Property(e => e.Updatedata)
                .HasColumnType("datetime")
                .HasColumnName("updatedata");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
