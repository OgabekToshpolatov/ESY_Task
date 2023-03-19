using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using taskapi.Entities;

namespace taskapi.Data;

public class AppDbContext:IdentityDbContext<User,IdentityRole,string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}
    public DbSet<Product>? Products { get; set; }
    public DbSet<ProductAudit>? ProductAudits { get; set; }
    
}