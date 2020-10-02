using ATech.ContactFormServer.Domain.Entities;
using ATech.ContactFormServer.Domain.Repositories;
using ATech.Repository;
using Microsoft.EntityFrameworkCore;

namespace ATech.ContactFormServer.Api.Repositories
{
#pragma warning disable CS1591
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DbContext context)
            : base(context) { }
    }
#pragma warning restore CS1591
}
