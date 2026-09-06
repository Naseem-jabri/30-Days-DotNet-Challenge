# Day 04 Authentication & JWT

I discovered today how to use JWT to secure the API endpoints and add authentication to an ASP.NET Core Web API.

## 1- Authentication
Authentication is the process of verifying who the user is
In this project, the user provides their email and password when logging in
The API checks the provided credentials against the user stored in the database.

## 2- Password Hashing with BCrypt
Passwords should not be stored as plain text in the database
Instead, I used BCrypt to hash the user's password before storing it
When the user logs in, the entered password is checked against the stored hash instead of comparing plain-text passwords
This means the database stores the hashed password, not the original password.

## 3- JWT Authentication
After the user's credentials are successfully verified, the API creates a JWT "JSON Web Token"
The token represents the authenticated user and is sent back to the client.

## 4- Protecting Endpoints with `[Authorize]`
Not every endpoint should be accessible to everyone
I used `Authorize` to protect endpoints that require authentication
When a request is sent to an endpoint protected by `[Authorize]`, ASP.NET Core checks whether the request contains a valid JWT.

## 5- Connecting Authentication to the Database

Authentication is connected to the existing `UserDbContext`.
The database is used to:
* Find the user during Login.
* Retrieve the stored password hash.
* Check the user's information.
* Check whether the email is confirmed.
So authentication is not working with hardcoded users; it works with the actual users stored in SQL Server

## Note
During testing, I encountered:
```text
401 Unauthorized
```

This happened when accessing a protected endpoint without successfully providing a valid authentication token
This helped me understand that:
**Having a login endpoint does not automatically mean that every request is authenticated.**
The protected request must include a valid JWT

## Key Concepts
### Verification
confirming the user's identity
### Permission
deciding if a particular resource or endpoint can be accessed by the authenticated user
### BCrypt
used to safely hash passwords rather than keeping them in plain text
### JWT
An authenticated user is represented by a token, which enables the API to verify authenticated requests
Endpoints that need authentication are protected by this feature
### 401 Unauthorized
returned when the request lacks proper authentication credentials and authentication is necessary
### Injection of Dependency
a method for ASP.NET Core to give classes the necessary services rather than having to create them by hand
