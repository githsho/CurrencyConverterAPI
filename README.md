To use Authentication
1. In the appsettings.json => JwtSettings => add your Issuer, Audience, SecretKey
2. Hit https://localhost:44374/api/Auth/login api => you will get the Bearer Token
3. In the CurrencyConverterController.cs => uncomment the Authorize attribute which will authenticate

Functionalities Implemented
1. Implement caching to minimize direct calls to the Frankfurter API.
2. Use retry policies with exponential backoff to handle intermittent API failures.
3. Implement dependency injection for service abstractions.
4. Design a factory pattern to dynamically select the currency provider based on the request.
5. Allow for future integration with multiple exchange rate providers.
6. Implement JWT authentication.
7. Enforce role-based access control (RBAC) for API endpoints.
8. Log the Client IP, ClientId from the JWT token, HTTP Method & Target Endpoint, Response Code & Response Time drtails for each request

API
https://localhost:44374/api/CurrencyConverter/convert
https://localhost:44374/api/CurrencyConverter/latestexchangerate
https://localhost:44374/api/CurrencyConverter/getratesoveraperiod

Run the project and swagger index file will open to test the api.
