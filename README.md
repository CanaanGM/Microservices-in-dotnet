# Microservices-in-dotnet

example project to better understand how microservices architecture function.


# Services

### Catalog

- repo pattern w/ mongo db
- port: **5001**

### Basket

- repo pattern w/ redis-cache
- port: **5002**


### [portainer](https://docs.portainer.io/start/intro)

> docker images management interface
- ports:
    - 8080
    - 9000 <- used one for the web interface