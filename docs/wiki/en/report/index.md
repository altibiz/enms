# ENMS report

This is the report for the ENMS project. It is divided into three sections.

## Development environment

The development environment illustrates the tools and technologies used in the
ENMS project. In short, the ENMS project is a full stack web application that
uses PostgreSQL as the database, ASP.NET Core as the backend and Blazor as the
frontend. The development environment also consists of configurations for
various tools used by the developers such as Visual Studio Code, Helix, Just and
Nix. Finally, the project is hosted on GitHub and uses GitHub actions for
continuous integration on Azure cloud app service which is connected to a
PostgreSQL database.

## Backend

The backend section of the report describes the server side of the ENMS project.
The backend is a RESTful API that uses the `Enms.Data` project to interact with
the database via `EntityFrameworkCore` and the `Enms.Business` project to
implement business logic. The server also uses the `Enms.Server` project as a
startup project. The backend also uses the `OrchardCore` library for
authentication, authorization and user management.

## Frontend

The frontend section of the report describes the client side of the ENMS
project. The frontend is a Blazor project that uses the `Enms.Client` project to
provide a user interface.
