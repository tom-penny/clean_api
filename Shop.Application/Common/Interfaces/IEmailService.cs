namespace Shop.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string recipient, string subject, string body, CancellationToken cancellationToken);
}