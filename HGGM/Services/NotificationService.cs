using System.Collections.Generic;
using System.Threading.Tasks;
using HGGM.Models;
using HGGM.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace HGGM.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;

        public NotificationService(IEmailSender emailSender, UserManager<User> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task NotifyUsers(Notification notification, IList<User> userList)
        {
            foreach (var user in userList)
            {              
                var setting = user.Config;
                if (setting.AccountNotify)
                {
                    user.Notifications.Add(notification);
                    await _userManager.UpdateAsync(user);
                }

                if (setting.EmailNotify)
                    await _emailSender.SendEmailAsync(user.Email.Address, notification.Subject, notification.Message);
            }
        }
    }

    public interface INotificationService
    {
        Task NotifyUsers(Notification notification, IList<User> userList);
    }
}