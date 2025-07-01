using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RazorLight;
using System.Net;
using System.Net.Mail;

namespace Management.BL.Utilities;

public class EmailService
{
    readonly IConfiguration _configuration;
    readonly IWebHostEnvironment _env;

    public EmailService(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public async Task SendTokenAsync(string to, string fullname, string template, string subject, string link)
    {
        RazorLightEngine engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(_env.ContentRootPath, "Templates"))
            .UseMemoryCachingProvider()
            .Build();

        string body = await engine.CompileRenderAsync(template, new
        {
            FullName = fullname,
            ResetLink = link
        });

        SmtpClient smtp = new(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]))
        {
            Credentials = new NetworkCredential(_configuration["Email:Login"], _configuration["Email:Passcode"]),
            EnableSsl = true
        };

        MailAddress from = new(_configuration["Email:Login"], "RoomEaser");
        MailAddress destination = new(to, fullname);

        MailMessage message = new(from, destination)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        await smtp.SendMailAsync(message);
    }
}
