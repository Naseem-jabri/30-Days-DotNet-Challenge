Load testing an ASP.NET Core Web API using k6, JWT authentication, virtual users, latency metrics, throughput, and failure-rate analysi

Testing how the API behaves when it receives a number of requests simultaneously

What if I have more users? Will the system handle it? Therefore, we need to know: Do the requests fail?
How long does the API take?
How many requests can it handle per second?
What happens when the number of users increases?
Where is the bottleneck?

I used k6 for Load Testing, which is a tool for testing the performance of APIs 
and applications by creating virtual users and sending requests to the system

1-VU — Virtual User :
It means k6 simulates 20 virtual users working in parallel

2-Iteration : 
 It is a complete course implemented by the VU

 3-HTTP Request:

 4-Throughput / Requests per Second :
It means how many HTTP requests the system was able to process per second



5-Latency / Response Time :
The time it took for the request to receive a response
For example:
promedio = 731ms
This means the average response time is about 731 milliseconds

6-Mediana
The value in the middle
If it is:
mediana = 568ms
This means that about 50% of the requests were faster than that and 50% were slower
//--------------------------------------------------
GET /api/Users is not the main source of slowness in the current test
Login is the most expensive part under concurrency
And the likely reason is that Login contains operations such as:
Database lookup + BCrypt password verification + JWT generation

The API successfully handled 20 concurrent virtual users with a 0% failure rate. However, 
response time increased significantly under higher concurrency, with average latency increasing 
from approximately 246 ms to 731 ms and P95 latency reaching approximately 1.97 second
