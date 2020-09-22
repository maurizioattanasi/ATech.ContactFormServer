using System;
using System.Threading;
using System.Threading.Tasks;
using ATech.ContactFormServer.Api.DTO;
using ATech.ContactFormServer.Api.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ATech.ContactFormServer.Api.Features.Message
{
    /// <summary>
    /// Adds a new message
    /// </summary>
    public class Add : IRequest<Guid>
    {
        private readonly string email;
        private readonly MessageDto message;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message"></param>
        public Add(string email, MessageDto message)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"'{nameof(email)}' cannot be null or empty", nameof(email));

            this.email = email;
            this.message = message ?? throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrEmpty(message.Name))
                throw new ArgumentNullException(nameof(message.Name));

            if (string.IsNullOrEmpty(message.EMail))
                throw new ArgumentNullException(nameof(message.EMail));

            if (string.IsNullOrEmpty(message.Message))
                throw new ArgumentNullException(nameof(message.Message));
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

                    if (!string.IsNullOrEmpty(request.message.Honeypot))
                        throw new Exception($"Spam detected");

                    var message = new Domain.Entities.ContactFormMessage
                    {
                        Id = Guid.NewGuid(),
                        Name = request.message.Name,
                        EMail = request.message.EMail,
                        Message = request.message.Message,
                        Created = DateTime.UtcNow,
                        CreatedBy = "me"
                    };

                    await unitOfWork
                        .ContactFormMessages
                        .AddAsync(message, cancellationToken)
                        .ConfigureAwait(false);

                    await unitOfWork.CompleteAsync(cancellationToken);

                    return message.Id;
                }
                catch (Exception ex)
                {
                    string content = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    this.logger.LogError(content);
                    throw ex;
                }
            }
        }
    }
}
