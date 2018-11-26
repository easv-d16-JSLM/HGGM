using System;
using System.Collections.Generic;
using System.Linq;
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
        // Missing - how to create link
        public async Task NotifyUsers(string subject, string message, IList<string> userIdList)
        {
            var notification = new Notification
            {
                DateNotified = DateTimeOffset.Now,
                Message = message,
                Subject = subject,
                Link = ""
            };

            foreach (var id in userIdList)
            {
                var user = await _userManager.FindByIdAsync(id);
                var setting = user.Config;
                if (setting.AccountNotify)
                {
                    user.Notififcations.Add(notification);
                    await _userManager.UpdateAsync(user);
                }

                if (setting.EmailNotify)
                {
                    await _emailSender.SendEmailAsync(user.Email.Address, subject, message);
                }
            }
        }
    }

    public interface INotificationService
    {
        Task NotifyUsers(string subject, string message, IList<string> userIdList);
    }
}
