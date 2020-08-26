# An Example of the SendGrid API with ASP.NET Core 3.1 Razor Pages
This document is to serve as an example and guide to getting started with Razor Pages using Identity Framework and SendGrid. See the official guides below.

* https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio
* https://sendgrid.com/docs/for-developers/sending-email/api-getting-started/

 # Overview
1. [Getting started with a completely fresh project](#getting-started-with-a-new-project)
2. [Already have an app with Authentication? Get started with SendGrid](#get-started-with-sendgrid)
3. [Technologies](#technologies)


## Getting Started with a New Project

<b>If you already have a project with Authentication skip to [Get Started with SendGrid](#get-started-with-sendgrid)</b>

1. Select File > New > Project.
2. Select ASP.NET Core Web Application. Name the project WebApp1 to have the same namespace as the project download. Click OK.
3. Select an ASP.NET Core Web Application, then select Change Authentication.
4. Select Individual User Accounts and click OK.
5. Select Create 

For the next step you can search for the Package Manager Console with (Ctrl + Q). In the PMC run the following

5. `PM> Update-Database`

At this point you should be able to register users and login. That's all that's needed to get started with SendGrid.  

## Get Started With SendGrid

1. Install SendGrid Package with NuGet Package Manager if it is not already installed.
2. Create a folder in the project named Services. 
3. In Services/ create a class named EmailSender.
4. Have EmailSender inherit from IEmailSender. 
`public class EmailSender : IEmailSender {...`

5. In EmailSender.cs include `using Microsoft.AspNetCore.Identity.UI.Services;` or use (Ctrl + .) to import the library.
6. Use (Ctrl + .) to implement the Interface. (We will come back to this shortly)
7. In the Services folder that you created, make another folder named Models.
8. In Services/Models/ create a new class named AuthSenderOptions. The file structure should look like the following: 
```
MyApp1
|
|___Pages
|___Services
|   |    EmailSender.cs
|   |___Models
    |     AuthSenderOptions.cs
    |
...
```
<b>9. Setup an environment variable for <b>your</b> SendGrid API Key</b>
- Press Win+R and run SystemPropertiesAdvanced.
- Click on Environment Variables.
- Click New in <b>user variables</b> section.
- Type SENDGRID_API_KEY in the name. (This name is arbitrary, use whatever you like.)
- Paste your API key for the value.
- Restart the IDE.

10. Modify EmailSender.cs to look like the following: 
```
public class EmailSender : IEmailSender {

        public EmailSender(IOptions<AuthSenderOptions> optionsAccessor) {
            Options = optionsAccessor.Value;
        }
        public AuthSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message) {
            return Execute(Options.SendGridKey, subject, message, email);
        }
        public Task Execute(string apiKey, string subject, string message, string email) {
            var client = new SendGrid.SendGridClient(apiKey);
            var msg = new SendGridMessage() {
                From = new EmailAddress("BitsRUsLMS@gmail.com", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(true, true);

            return client.SendEmailAsync(msg);
        }
    }

```
11. In EmailSender.cs add the following imports:
```
using WebApp1.Services.Models;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
```
12. Modify AuthSenderOptions.cs to look like: 
```
public class AuthSenderOptions {
        private string user = "JaneDoe"; // The name you want to show up on your email
        // Make sure the string passed in below matches the enviorment variable that you set in step 9.
        private string key = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

        public string SendGridUser { get { return user; } }

        
        public string SendGridKey { get { return key; } }
    }
```
13. In Startup.cs add the email sender as a service. 
```
public void ConfigureServices(IServiceCollection services) {
            ...
            // Add these 2 lines  
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthSenderOptions>(Configuration);
        }
```
14. Also in Startup.cs, add the following imports
```
using WebApp1.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebApp1.Services.Models;
```

Done! Run the application and navigate to Login. Select Forgot Password and enter your email. 

  
### Technologies
* Visual Studio 2019 v16.7.2

* ASP.Net Core 3.1

https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-3.1?view=aspnetcore-3.1

https://dotnet.microsoft.com/download

How to Customize Email Pages and Identity Pages Coming Soon...
