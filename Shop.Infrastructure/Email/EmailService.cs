using MimeKit;
using MailKit.Net.Smtp;

namespace Shop.Infrastructure.Email;

using Application.Interfaces;

public class EmailService : IEmailService
{
    private readonly EmailOptions _options;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendEmailAsync(string recipient, string subject, string body, CancellationToken cancellationToken)
    {
        var message = new MimeMessage
        {
            Subject = subject,
            Body = new TextPart { Text = body }
        };

        message.To.Add(new MailboxAddress(recipient, recipient));
        message.From.Add(new MailboxAddress(_options.Sender, _options.Sender));

        using var client = new SmtpClient();

        await client.ConnectAsync(_options.Host, _options.Port, false, cancellationToken);
        await client.AuthenticateAsync(_options.Sender, _options.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}