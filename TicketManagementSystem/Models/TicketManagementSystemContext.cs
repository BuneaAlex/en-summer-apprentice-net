using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TicketManagementSystem.Models;

public partial class TicketManagementSystemContext : DbContext
{
    public TicketManagementSystemContext()
    {
    }

    public TicketManagementSystemContext(DbContextOptions<TicketManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<TicketCategory> TicketCategories { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("PK__customer__B61ED7F52D9BE945");

            entity.ToTable("customers");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("customer_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("PK__events__2DC8B111581789A4");

            entity.ToTable("events");

            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.EventTypeid).HasColumnName("event_typeid");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Venueid).HasColumnName("venueid");

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeid)
                .HasConstraintName("FKt2ye4v910lpa22ulmaeyms87b");

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .HasForeignKey(d => d.Venueid)
                .HasConstraintName("FKc686xjxqtinbouvfg8m9995nk");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeid).HasName("PK__event_ty__098CC97D0BD3F810");

            entity.ToTable("event_types");

            entity.Property(e => e.EventTypeid).HasColumnName("event_typeid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("PK__orders__080E3775380D724C");

            entity.ToTable("orders");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.NumberOfTickets).HasColumnName("number_of_tickets");
            entity.Property(e => e.OrderedAt)
                .HasColumnType("datetime")
                .HasColumnName("ordered_at");
            entity.Property(e => e.TicketCategoryid).HasColumnName("ticket_categoryid");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("FK7orngfblbwyce32cqv07j9f9b");

            entity.HasOne(d => d.TicketCategory).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TicketCategoryid)
                .HasConstraintName("FK52p97sa1ffk9u92uik2igfrqy");
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasKey(e => e.TicketCategoryid).HasName("PK__ticket_c__45D44DCA5CE59B18");

            entity.ToTable("ticket_categories");

            entity.Property(e => e.TicketCategoryid).HasColumnName("ticket_categoryid");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.NoAvailable).HasColumnName("no_available");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Event).WithMany(p => p.TicketCategories)
                .HasForeignKey(d => d.Eventid)
                .HasConstraintName("FK5h0l725993l81d27ydmewso0o");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Venueid).HasName("PK__venues__4DC0ABE72C8EF744");

            entity.ToTable("venues");

            entity.Property(e => e.Venueid).HasColumnName("venueid");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
