namespace Shop.Infrastructure.Email;

public class EmailOptionsConfiguration : IConfigureOptions<EmailOptions>
{
    private readonly IConfiguration _configuration;

    public EmailOptionsConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EmailOptions options)
    {
        _configuration.GetSection("Smtp").Bind(options);
    }
}