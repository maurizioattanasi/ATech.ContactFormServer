# ATech.ContactFormServer

A .NET Core WebApi to use on static sites with contact form

If you have a static web site hosted on services like [GitHub Pages](https://pages.github.com/) or [GitLab Pages](https://docs.gitlab.com/ee/user/project/pages/), there are many chances that you would like to use a contact form to have some feedback from your site's guests or just to get in touch with them.  
There are many options to accomplish the same result, many of them paid, some with a free but feature limited plan, and others that need a little bit of effort to integrate with your contact form.

The solution is made of two projects:

- a dotnet core dll project [ATech.ContactFormServer.Domain](#atechcontactformserverdomain)
- a dotnet core web api project [ATech.ContactFormServer.Api](#atechcontactformserverapi)

## ATech.ContactFormServer.Domain

This project contains all the models and the interfaces that will be implemented in the Api project.

### Entities

This folder/namespace will contain all the entities that will be used to implement the business logic of the application:

1. `Account` is the owner entity of the messages;
2. `Message` is the model of a single message;

### Repository

This folder/namespace contains all the interfaces needed to implent a [Repository Pattern](https://docs.microsoft.com/it-it/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design) that will be the intermediary between the domain model layers and data mapping.

## ATech.ContactFormServer.Api

This project contains:

- the implementation of the business logic of the backend;
- the implementation of the interfaces of the [Repository Pattern](https://docs.microsoft.com/it-it/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design) exposed by the domain project;
- the data persistance layer;
- all *the bells and whistles* needed to implement a REST MVC Api.

## Entity Framework Core

The OR/M of choice for this project is [Entity Framework Core](https://docs.microsoft.com/it-it/ef/core/), and the approach in the early stages of development of the project is [Code First](https://docs.microsoft.com/it-it/aspnet/core/data/ef-mvc/intro?view=aspnetcore-3.1) but it may change if needed.