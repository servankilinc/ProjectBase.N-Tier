using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.ProjectEntities;

namespace DataAccess.Contexts;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> // DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public override DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogLike> BlogLikes { get; set; }
    public DbSet<BlogComment> BlogComments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // Project Entities
    public DbSet<Log> Logs { get; set; }
    public DbSet<Archive> Archives { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Localization> Localizations { get; set; }
    public DbSet<LocalizationLanguageDetail> LocalizationLanguageDetails { get; set; }

    // Project Entities

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<User>(u =>
        {
            u.ToTable("dbo_user");

            u.HasKey(u => u.Id);

            u.HasMany(u => u.Blogs)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            u.HasMany(u => u.BlogComments)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            u.HasMany(u => u.BlogLikes)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            u.HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ****** If SoftDeletable is used, the filter should be applied to all entities
            u.HasQueryFilter(f => !f.IsDeleted);
        });

        modelBuilder.Entity<Blog>(b =>
        {
            b.ToTable("dbo_blog");

            b.HasKey(b => b.Id);

            b.HasOne(b => b.Author)
               .WithMany(a => a.Blogs)
               .HasForeignKey(b => b.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(b => b.Category)
               .WithMany(c => c.Blogs)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(b => b.BlogLikes)
                .WithOne(b => b.Blog)
                .HasForeignKey(b => b.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(b => b.BlogComments)
               .WithOne(b => b.Blog)
               .HasForeignKey(b => b.BlogId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasQueryFilter(f => !f.IsDeleted);
        });

        modelBuilder.Entity<Category>(c =>
        {
            c.ToTable("dbo_category");

            c.HasKey(c => c.Id);

            c.HasMany(c => c.Blogs)
               .WithOne(b => b.Category)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            c.HasQueryFilter(f => !f.IsDeleted);
        });

        modelBuilder.Entity<BlogLike>(b =>
        {
            b.ToTable("dbo_blogLike");

            b.HasKey(b => new { b.BlogId, b.UserId });

            b.HasOne(b => b.Blog)
               .WithMany(b => b.BlogLikes)
               .HasForeignKey(b => b.BlogId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(b => b.User)
               .WithMany(u => u.BlogLikes)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BlogComment>(b =>
        {
            b.ToTable("dbo_blogComment");

            b.HasKey(b => b.Id);

            b.HasOne(b => b.User)
               .WithMany(u => u.BlogComments)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(b => b.Blog)
               .WithMany(b => b.BlogComments)
               .HasForeignKey(b => b.BlogId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RefreshToken>(r =>
        {
            r.ToTable("dbo_refreshToken");

            r.HasKey(r => r.Id);

            r.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Log>(l =>
        {
            l.ToTable("ProjectLogs");

            l.HasKey(l => l.Id);
        });

        modelBuilder.Entity<Archive>(a =>
        {
            a.ToTable("ProjectArchives");

            a.HasKey(a => a.Id);
        });
        
        modelBuilder.Entity<Language>(l =>
        {
            l.ToTable("ProjectLanguages");

            l.HasKey(l => l.Id);

            l.HasData(
                new Language
                {
                    Id = (byte)Core.Enums.Languages.Turkish,
                    Name = "Türkçe",
                    Code = "tr-TR",
                    Icon = "tr.png",
                    Priority = 1,
                    ResourceFileName = "resources.tr.resx",
                    ResourceFileVersion = 1
                }, new Language
                {
                    Id = (byte)Core.Enums.Languages.English,
                    Name = "English",
                    Code = "en-US",
                    Icon = "en.png",
                    Priority = 2,
                    ResourceFileName = "resources.en.resx",
                    ResourceFileVersion = 1
                },
                new Language
                {
                    Id = (byte)Core.Enums.Languages.Russian,
                    Name = "Russian",
                    Code = "ru-RU",
                    Icon = "ru.png",
                    Priority = 3,
                    ResourceFileName = "resources.ru.resx",
                    ResourceFileVersion = 1
                }
            );

            l.HasMany(l => l.LocalizationLanguageDetails)
                .WithOne(lld => lld.Language)
                .HasForeignKey(lld => lld.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Localization>(l =>
        {
            l.ToTable("ProjectLocalizations");
            l.HasKey(l => l.Id);

            l.HasMany(l => l.LocalizationLanguageDetails)
                .WithOne(lld => lld.Localization)
                .HasForeignKey(lld => lld.LocalizationId)
                .OnDelete(DeleteBehavior.Cascade);

            l.HasIndex(l => l.EntityId);
        });
        
        modelBuilder.Entity<LocalizationLanguageDetail>(lld =>
        {
            lld.ToTable("ProjectLocalizationLanguageDetails");

            lld.HasKey(lld => new { lld.LocalizationId, lld.LanguageId });
        });


        // If Identity Exist 
        // Remove IdentityRole defiantion if Custom Role Entity Exist
        modelBuilder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable("Roles");

            entity.HasData(
                new
                {
                    Id = new Guid("b370875e-34cd-4b79-891c-93ae38f99d11"),
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = new Guid("b370875e-34cd-4b79-891c-93ae38f99d11").ToString()
                },
                new
                {
                    Id = new Guid("cd6040ef-dacc-4678-9a85-154f12581cff"),
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = new Guid("cd6040ef-dacc-4678-9a85-154f12581cff").ToString()
                },
                new
                {
                    Id = new Guid("7138ec51-4f9e-4afd-b61b-5a9a4584f5da"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = new Guid("7138ec51-4f9e-4afd-b61b-5a9a4584f5da").ToString()
                },
                new
                {
                    Id = new Guid("1f20c152-530e-4064-a39c-bbbed341fe84"),
                    Name = "Owner",
                    NormalizedName = "OWNER",
                    ConcurrencyStamp = new Guid("1f20c152-530e-4064-a39c-bbbed341fe84").ToString()
                }
            );
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable("UserClaims"); });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable("UserLogins"); });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable("RoleClaims"); });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable("UserRoles"); });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable("UserTokens"); });
    }
}
