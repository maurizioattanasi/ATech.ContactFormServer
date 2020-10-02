using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ATech.ContactFormServer.Api.Repositories;
using ATech.ContactFormServer.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ATech.ContactFormServer.Infrastructure.Features.Message
{
    /// <summary>
    /// Returns all the messages
    /// </summary>
    public class GetAll : IRequest<IEnumerable<Domain.Entities.Message>>
    {
        private readonly Guid accountId;
        private readonly int? page;
        private readonly int? pageSize;

        /// <summary>
        /// Constgructor
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public GetAll(Guid accountId, int? page = null, int? pageSize = null)
        {
            this.accountId = accountId;
            this.page = page;
            this.pageSize = pageSize;
        }

        class GetAllHandlar : IRequestHandler<GetAll, IEnumerable<Domain.Entities.Message>>
        {
            private readonly ContactFormServerDbContext context;
            private readonly ILogger<GetAllHandlar> logger;

            public GetAllHandlar(ContactFormServerDbContext context,
                                 ILogger<GetAllHandlar> logger)
            {
                this.context = context ?? throw new System.ArgumentNullException(nameof(context));
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            }

            public async Task<IEnumerable<Domain.Entities.Message>> Handle(GetAll request, CancellationToken cancellationToken)
            {
                try
                {
                    using var unitOfWork = new ContactFormServerUnitOfWork(context);

                    var account = await unitOfWork.Accounts.GetAsync(request.accountId, cancellationToken).ConfigureAwait(false);

                    if(account is null)
                        throw new HttpException(StatusCodes.Status404NotFound, $"No account with Id {request.accountId} has been found");

                    var messages = account.Messages;

                    if (request.page.HasValue && request.pageSize.HasValue)
                    {
                        var skipAmount = request.pageSize.Value * (request.page.Value - 1);

                        return messages
                            .OrderByDescending(m => m.Created)
                            .Skip(skipAmount)
                            .Take(request.pageSize.Value);
                    }

                    return messages; // TODO:
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
