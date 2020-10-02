using System;
using System.Threading;
using System.Threading.Tasks;
using ATech.ContactFormServer.Api.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ATech.ContactFormServer.Infrastructure.Features.Account
{
    /// <summary>
    /// Adds a new account item
    /// </summary>
    public class Add : IRequest<Guid>
    {
        private readonly string email;

        /// <summary>
        /// Constructor
        /// </summary>
        public Add(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException($"'{nameof(email)}' cannot be null or empty", nameof(email));
            }

            this.email = email;
        }

        class AddHandler : IRequestHandler<Add, Guid>
        {
            private readonly ContactFormServerDbContext context;
            private readonly ILogger<AddHandler> logger;

            public AddHandler(ContactFormServerDbContext context,
                              ILogger<AddHandler> logger)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Guid> Handle(Add request, CancellationToken cancellationToken)
            {
                try
                {
                    using var unitOfWork = new ContactFormServerUnitOfWork(context);

                    var account = new Domain.Entities.Account
                    {
                        Id = Guid.NewGuid(),

                        EMail = request.email,

                        Enabled = false,

                        Created = DateTime.Now,

                        //TODO: Add Currente User Id Management
                        CreatedBy = "me"
                    };

                    await unitOfWork.Accounts.AddAsync(account, cancellationToken).ConfigureAwait(false);

                    await unitOfWork.CompleteAsync(cancellationToken);

                    return account.Id;
                }
                catch (Exception ex)
                {
                    string content = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    this.logger.LogError(content);
                    return Guid.Empty;
                    // throw ex;
                }
            }
        }
    }
}
