using System;
using System.Threading;
using System.Threading.Tasks;
using ATech.ContactFormServer.Domain.Repositories;
using ATech.ContactFormServer.Infrastructure;

namespace ATech.ContactFormServer.Api.Repositories
{
#pragma warning disable CS1591
    public class ContactFormServerUnitOfWork : IContactFormServerUnitOfWork
    {
        private readonly ContactFormServerDbContext context;

        public ContactFormServerUnitOfWork(ContactFormServerDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.Accounts = new AccountRepository(context);

            this.Messages = new MessageRepository(context);
        }

        public IMessageRepository Messages { get; private set; }

        public IAccountRepository Accounts { get; private set; }

        public int Complete()
        {
            return this.context.SaveChanges();
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await this.context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
#pragma warning restore CS1591
}
