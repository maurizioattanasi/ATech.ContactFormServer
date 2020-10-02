using System;
using System.Threading;
using System.Threading.Tasks;
using ATech.ContactFormServer.Infrastructure.DTO;
using ATech.ContactFormServer.Api.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ATech.ContactFormServer.Infrastructure.Exceptions;
using System.Text.Json;

namespace ATech.ContactFormServer.Infrastructure.Features.Message
{
    /// <summary>
    /// Adds a new message
    /// </summary>
    public class Add : IRequest<Guid>
    {
        private readonly Guid accountId;
        private readonly MessageDto message;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="message"></param>
        public Add(Guid accountId, MessageDto message)
        {
            this.accountId = accountId;
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
                        throw new Exception($"Spam detected{Environment.NewLine}Account: {request.accountId}{Environment.NewLine}Payolad: {JsonSerializer.Serialize(request.message)}");

                    var account = await unitOfWork.Accounts.GetAsync(request.accountId, cancellationToken).ConfigureAwait(false);

                    if (account is null)
                        throw new HttpException(StatusCodes.Status404NotFound, $"No account with Id {request.accountId} has been found");

                    if (!account.Enabled)
                        throw new HttpException(StatusCodes.Status403Forbidden, $"{account.Id} is not enabled yet");

                    var message = new Domain.Entities.Message
                    {
                        Id = Guid.NewGuid(),
                        Name = request.message.Name,
                        EMail = request.message.EMail,
                        PhoneNumber = request.message.Phone,
                        Text = request.message.Message,
                        AccountId = account.Id,
                        Created = DateTime.UtcNow,
                        CreatedBy = "me",
                    };

                    await unitOfWork.Messages.AddAsync(message, cancellationToken).ConfigureAwait(false);

                    await unitOfWork.CompleteAsync(cancellationToken);

                    return message.Id;
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
