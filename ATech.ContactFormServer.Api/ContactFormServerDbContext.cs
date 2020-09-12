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

        public DbSet<ContactFormMessage> ContactFormMessage { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<ContactFormMessage>()
                .HasKey(m => m.Id);
        }
    }
#pragma warning disable CS1591
}
