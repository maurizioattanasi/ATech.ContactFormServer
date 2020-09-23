using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using ATech.ContactFormServer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ATech.ContactFormServer.Api
{
#pragma warning disable CS1591
    public class ContactFormServerDbContext : DbContext
    {
        private readonly DbContextOptions<ContactFormServerDbContext> options;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ContactFormServerDbContext([NotNull] DbContextOptions<ContactFormServerDbContext> options,
                                          IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            this.httpContextAccessor = httpContextAccessor ?? throw new System.ArgumentNullException(nameof(httpContextAccessor));
        }

        public string CurrentUserId
        {
            get
            {
                var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return userId;
            }
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
