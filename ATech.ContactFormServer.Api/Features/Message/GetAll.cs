using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ATech.ContactFormServer.Api.Repositories;
using ATech.ContactFormServer.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ATech.ContactFormServer.Api.Features.Message
{
    /// <summary>
    /// Returns all the messages
    /// </summary>
    public class GetAll : IRequest<IEnumerable<Domain.Entities.ContactFormMessage>>
    {
        private readonly int? page;
        private readonly int? pageSize;

        /// <summary>
        /// Constgructor
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public GetAll(int? page = null, int? pageSize = null)
        {
            this.page = page;
            this.pageSize = pageSize;
        }

        class GetAllHandlar : IRequestHandler<GetAll, IEnumerable<Domain.Entities.ContactFormMessage>>
        {
            private readonly ContactFormServerDbContext context;
            private readonly ILogger<GetAllHandlar> logger;

            public GetAllHandlar(ContactFormServerDbContext context,
                                 ILogger<GetAllHandlar> logger)
            {
                this.context = context ?? throw new System.ArgumentNullException(nameof(context));
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            }

            public async Task<IEnumerable<ContactFormMessage>> Handle(GetAll request, CancellationToken cancellationToken)
            {
                try
                {
                    using var unitOfWork = new ContactFormServerUnitOfWork(context);

                    var messages = await unitOfWork.ContactFormMessages.GetAllAsync(cancellationToken);

                    if (request.page.HasValue && request.pageSize.HasValue)
                    {
                        var skipAmount = request.pageSize.Value * (request.page.Value - 1);

                        return messages
                            .OrderByDescending(m => m.Created)
                            .Skip(skipAmount)
                            .Take(request.pageSize.Value);
                    }

                    return messages;
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
