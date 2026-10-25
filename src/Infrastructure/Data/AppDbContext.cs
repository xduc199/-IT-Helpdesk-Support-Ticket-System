using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // =========================
    // REQUESTER (OK)
    // =========================
    modelBuilder.Entity<Ticket>()
        .HasOne(t => t.Requester)
        .WithMany(u => u.CreatedTickets)
        .HasForeignKey(t => t.RequesterId)
        .OnDelete(DeleteBehavior.Restrict);

    // =========================
    // ASSIGNED USER (THIẾU CÁI NÀY)
    // =========================
    modelBuilder.Entity<Ticket>()
        .HasOne(t => t.AssignedTo)
        .WithMany(u => u.AssignedTickets)
        .HasForeignKey(t => t.AssignedToId)
        .OnDelete(DeleteBehavior.SetNull);

    // =========================
    // MANY TO MANY TAGS
    // =========================
    modelBuilder.Entity<Ticket>()
        .HasMany(t => t.Tags)
        .WithMany(t => t.Tickets)
        .UsingEntity(j => j.ToTable("TagTicket"));
}
}