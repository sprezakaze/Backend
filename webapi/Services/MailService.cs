using MailKit.Net.Smtp;
using MimeKit;
using webapi.Entities;

namespace webapi.Services
{
    public class MailService
    {
        private readonly string _email;
        private readonly string _password;
        private readonly string _host;
        private readonly int _port;


        public MailService(IConfiguration configuration) 
        {
            _email = configuration["MasterEmail"] ?? string.Empty;
            _password = configuration["MasterEmailPassword"] ?? string.Empty;
            _host = configuration["SmtpHost"] ?? string.Empty;
            try
            {
                _port = Convert.ToInt32(configuration["SmtpPort"]);
            }
            catch
            {
                _port = 25;
            };
        }

        public async Task SendOrder(Order order, List<CartItem> items, string vk)
        {
            try
            {
                using var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("AutoBot", _email));
                emailMessage.To.Add(new MailboxAddress("Me", _email));
                emailMessage.Subject = $"Заказ {order.Id}";

                var msg = $@"
                    <h1>Заказ {order.Id} от {order.Created}</h1>
                    <p>Vk: {vk}</p>
                    <p>Пчел: {order.User.Email}</p>
                    <p>Вещи:</p>
                    <ul>
                        {items.Select(el => $"<li>{el.Clothing.Name} (#{el.Clothing.ClothingId}) - {el.Count} шт. - Всего {el.Clothing.Price * el.Count} руб.</li>").Aggregate("", (acc, cur) => acc + "<br/>" + cur)}
                    </ul>
                ";

                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = msg
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_email, _password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
            catch (Exception ex) 
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}
