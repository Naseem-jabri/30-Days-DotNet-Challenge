# Day 06 Email Service 

I discovered today how to incorporate an Email Service into an ASP.NET Core Web API
and utilize it to send people actual emails
The AuthNest_API project was expanded to include the implementation

## The flow became:
```text
User registers
      
Account created
      
Email Service
      
Welcome Email sent
```

## 1. Created `IEmailService`
Created an interface that defines the email functionality.

## 2. Created `EmailService`
Created the actual service responsible for sending emails.

## 3. Used SMTP
Used SMTP "Simple Mail Transfer Protocol" to send emails
The email service connects to the email provider's SMTP server and sends the email through it

## 4. Added Email Settings

Added the email configuration to `appsettings.json`.
The settings contain information such as:
* SMTP server
* SMTP port
* Sender email
* Password / App Password

## 5. Registered the Service in `Program.cs`
Registered `IEmailService` and `EmailService` using Dependency Injection

## 6. Connected the Email Service to User Registration
Updated the user registration process so that after successfully creating a user
the API calls the email service
