using System.Diagnostics.CodeAnalysis;
using ATech.ContactFormServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ATech.ContactFormServer.Infrastructure
{
#pragma warning disable CS1591
    public class ContactFormServerDbContext : DbContext
    {
        private readonly DbContextOptions<ContactFormServerDbContext> options;

        public ContactFormServerDbContext([NotNull] DbContextOptions<ContactFormServerDbContext> options)
            : base(options)
        {
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public DbSet<Account> Account { get; set; }

        public DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region  Account

            builder
                .Entity<Account>()
                .HasKey(a => a.Id);

            builder
                .Entity<Account>()
                .HasIndex(a => a.Id)
                .IsUnique();
            #endregion

            #region Message
            builder
                .Entity<Message>()
                .HasKey(m => m.Id);
            #endregion
        }
    }
#pragma warning disable CS1591
}
