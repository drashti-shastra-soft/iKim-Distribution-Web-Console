using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface INotificationService
    {
        Task<Domain.NotificationLister> GetAllAsync(Domain.NotificationLister mLister);
        Task<Domain.Notification> CreateAsync(Domain.Notification mNotification);
        Task<Domain.Notification> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
