using Microsoft.EntityFrameworkCore;
using System.Text;
using T_Microservices.Services.EmailAPI.Models;
using T_Microservices.Services.EmailAPI.Data;
using T_Microservices.Services.EmailAPI.Models.Dto;

namespace T_Microservices.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;

        public EmailService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }


        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");

            foreach (var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }

            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User Registeration Successful. <br/> Email : " + email;

            await LogAndEmail(message, "dotnetmastery@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new ApplicationDbContext(_dbOptions);

                await _db.EmailLogger.AddAsync(emailLog);

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
