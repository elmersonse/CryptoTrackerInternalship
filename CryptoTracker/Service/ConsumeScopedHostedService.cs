using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoTracker.DAL;
using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Service.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimeKit;

namespace CryptoTracker.Service
{
    public class ConsumeScopedHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public ConsumeScopedHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(604800), TimeSpan.FromSeconds(604800));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var profit = GetProfit(context);

                var mes = new MimeMessage();
            
                mes.From.Add(new MailboxAddress("CryptoTracker", "crypto.tracker@inbox.ru"));
                mes.Subject = "CryptoTracker Profit";

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.mail.ru", 465, true);
                    await client.AuthenticateAsync("crypto.tracker@inbox.ru", "msjVXfajRYdtdiMWXJ0X");
                    foreach (var p in profit)
                    {
                        mes.To.Clear();
                        mes.To.Add(new MailboxAddress("", p.Key));
                        mes.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                        {
                            Text = $"Hey! Your income this week is {p.Value}!"
                        };
                        await client.SendAsync(mes);
                    }
                    await client.DisconnectAsync(true);
                }
            }
        }

        public Dictionary<string, float> GetProfit(ApplicationDbContext context)
        {
            Dictionary<string, float> res = new Dictionary<string, float>();
            var users = context.Users;
            foreach (var u in users)
            {
                res.Add(u.Email, u.Profit);
            }

            return res;
        }
    }
}