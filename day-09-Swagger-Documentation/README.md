
Swagger :
It is an open-source toolkit built around OpenAPI Specification (OAS) 
and is used as an operational guide explaining the API, including the following.

1- Endpoints and HTTP Methods: GET, POST
2- The sent data (Request Body / Parameters): The data that is sent
with the conditions and required fields explained
For example, the registration endpoint accepts:

{
  "username": "newtest",
  "email": "newtest@gmail.com",
  "password": "Test123!"
}


3- The received data (Response):
POST /api/Users/login :

200 — Login successful and JWT token generated
400 — Invalid login data
401 — Invalid credentials or email not confirmed

GET /api/Users:

200 — Users retrieved successfully
401 — Authentication is required

GET /api/Users/confirm-email:

200 — Email confirmed successfully
400 — Invalid confirmation token

4- Authorization and Security (Authentication): Some endpoints require a token


What I Learned

Through this task, I learned how to:

Integrate Swagger with an ASP.NET Core Web API
Document API endpoints using XML comments
Document request parameters and response codes
Configure JWT Bearer authentication in Swagger
Test protected endpoints using a JWT token
Understand how OpenAPI describes an API
