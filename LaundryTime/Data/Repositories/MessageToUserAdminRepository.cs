using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories
{
    public class MessageToUserAdminRepository: Repository<MessageToUserAdmin>, IMessageToUserAdminRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public MessageToUserAdminRepository(ApplicationDbContext context) : base(context) { }

        public List<MessageToUserAdmin> GetAllMessages(string userName)
        {
            return context.MessageList.ToList();
        }

        public MessageToUserAdmin GetSingleMessage(int id)
        {
            return context.MessageList.SingleOrDefault(i => i.MessageId == id);
        }

        public void SendMessage(MessageToUserAdmin message)
        {
            context.MessageList.Add(message);
        }
    }
}
