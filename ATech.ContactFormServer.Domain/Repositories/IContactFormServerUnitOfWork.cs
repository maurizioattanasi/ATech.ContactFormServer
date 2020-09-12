using System;
using System.Threading;
using System.Threading.Tasks;

namespace ATech.ContactFormServer.Domain.Repositories
{
#pragma warning disable CS1591
    public interface IContactFormServerUnitOfWork : IDisposable
    {
        IContactFormMessageRepository ContactFormMessageRepository { get; }

        int Complete();

        Task<int> CompleteAsync(CancellationToken cancellationToken);
    }
#pragma warning restore CS1591
}
