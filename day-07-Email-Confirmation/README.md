# Email Verification

Sending a verification link to the email after logging in.

## What I Implemented

1. Created an email confirmation token using `Guid.NewGuid()`

2. Created a confirmation endpoint:

```text
GET /api/Users/confirm-email
```

3. Stored the confirmation token in the database

4. Send a verification link to the email after logging in. Update the value of `IsEmailConfirmed` to `true` after successful confirmation.

Delete the confirmation code after the operation is successful.

## Registration Flow

```text
Register
   
Create User
   
Generate Confirmation Token
   
Save Token in DB
   
Send Email 
   
User opens the email
   
Clicks "Confirm Email"
   
The link opens the confirmation endpoint
   
IsEmailConfirmed = true
```


