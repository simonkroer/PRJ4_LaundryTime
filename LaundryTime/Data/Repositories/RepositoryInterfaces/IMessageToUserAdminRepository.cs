using System;
using System.Collections.Generic;
using System.Linq;
using LaundryTime.Data.Models;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface IMessageToUserAdminRepository: IRepository<MessageToUserAdmin>
    {
        List<MessageToUserAdmin> GetAllMessages();
        MessageToUserAdmin GetSingleMessage(int id);
        void SendMessage(MessageToUserAdmin message);
    }
}
