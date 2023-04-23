# Microservices-in-dotnet

example project to better understand how microservices architecture function.


# Services

- when applying migrations, it helps to override the connection string of the serivce in `docker-compose.yml`.

like a so:
```yml
catalog.api:
    environment:
      - "Connection string here!!"
    image: ${DOCKER_REGISTRY-}catalogapi
    build:
      context: .
      dockerfile: Services/Catalog/Catalog.API/Dockerfile
    ports:
      - "5001:80"
    container_name: Catalog-API
    depends_on:
      - mongoProduct

```

---

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
- this [DDD](https://en.wikipedia.org/wiki/Domain-driven_design), not this [DDD](https://yugipedia.com/wiki/D/D/D)
- Fluent Validation 
- EF CORE 

#### Layers
- Core 
    - Domain
        - includes [domain](https://medium.com/nick-tune-tech-strategy-blog/domains-subdomain-problem-solution-space-in-ddd-clearly-defined-e0b49c7b586c) only objects
        - [ValueObject - Docs](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects)
    - Application : Domain
        - for everything related to Business
        - Folders:
            - Contracts  : Application Capabilities ; Abstractions and Interfaces
            - Behaviours : Contains Behaviours/Cross cutting concerns that apply when using the implementation ; Validation for example
            - Features   : CQRS related stuff that handles business cases
- Infrastrucure  : Application
- API : Application, Infra


migrate and apply

```bash
    dotnet ef migrations add initial -p .\Ordering\Ordering.Infrastructure\Ordering.Infrastructure.csproj -s .\Ordering\Ordering.API\Ordering.API.csproj
    dotnet ef database update -p .\Ordering\Ordering.Infrastructure\Ordering.Infrastructure.csproj -s .\Ordering\Ordering.API\Ordering.API.csproj
```

----

### [portainer](https://docs.portainer.io/start/intro)

> docker images management interface
- ports:
    - 8080
    - 9000 <- used one for the web interface



----

### Building Block

> houses all the common things between the services, like the message classes



----

### Rabbit MQ and masstransit

> connect both the basket and the orders services via a shared queue

the basket emmits an event message the the orders consume it.

----

