# Microservices-in-dotnet

example project to better understand how microservices architecture function.


# Services

### Catalog

> responsible for the information of the protduct catalog.

- REST CRUD 
- port: **5001**
- repo pattern w/ mongo db
- Layered architecture - separated by folders not projects
    - **Infrastructer** layer for **Data Access** and presistence of business state
    - **Domain layer** for **Business** logic
        - the *heart* of the application
    - **API/Application** layer for **Presentation** layer, *controllers* in this case
        - data transmission 2 user/other services


----

### Basket

> Responsible for the users basket and checkout

- repo pattern w/ redis-cache
- port: **5002**
- REST CRUD
- Layered architecture - separated by folders not projects
    - **Infrastructer** layer for **Data Access** and presistence of business state
    - **Domain layer** for **Business** logic
        - the *heart* of the application
    - **API/Application** layer for **Presentation** layer, *controllers* in this case
        - data transmission 2 user/other services


----

### Discount

#### API
> 

- repo pattern w/ postgress
- port: **5003**
- REST CRUD
- Dapper as the *smol* ORM
- Layered architecture - separated by folders not projects
    - **Infrastructer** layer for **Data Access** and presistence of business state
    - **Domain layer** for **Business** logic
        - the *heart* of the application
    - **API/Application** layer for **Presentation** layer, *controllers* in this case
        - data transmission 2 user/other services

#### gRPC

- port: **5004**
- Dapper as the *smol* ORM
- 

---

### Ordering API

- REST API w/ CRUD
- DDD, CQRS w/ MediatR, Clean Architecture
- Fluent Validation 
- EF CORE 


----

### [portainer](https://docs.portainer.io/start/intro)

> docker images management interface
- ports:
    - 8080
    - 9000 <- used one for the web interface