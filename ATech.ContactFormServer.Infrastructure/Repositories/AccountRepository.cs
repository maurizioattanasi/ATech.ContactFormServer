using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ATech.ContactFormServer.Domain.Entities;
using ATech.ContactFormServer.Domain.Repositories;
using ATech.ContactFormServer.Infrastructure;
using ATech.Repository;
using Microsoft.EntityFrameworkCore;

namespace ATech.ContactFormServer.Api.Repositories
{
#pragma warning disable CS1591
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(DbContext context)
            : base(context) { }

        public override async Task<Account> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var account = ContactFormServerDbContext
                .Account
                .Where(a => a.Id == id);

            await account.Include(a => a.Messages).LoadAsync(cancellationToken);

            return await account.FirstOrDefaultAsync(cancellationToken);
        }

        private ContactFormServerDbContext ContactFormServerDbContext { get { return Context as ContactFormServerDbContext; } }

    }
#pragma warning restore CS1591
}
