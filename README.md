# ScheduleGo

ScheduleGo is a small scheduling and task-management application built with Blazor and ASP.NET Core. It allows users to create and manage events and to-do tasks, including linking tasks to events, assigning priorities, setting due dates, and marking tasks as completed.

The project is split into a Blazor frontend and an ASP.NET Core Web API backend, with SQL Server used for persistent data storage.

## Features

* Create, edit, and delete tasks
* Mark tasks as completed
* Assign priorities to tasks
* Set due dates
* Create and manage events
* Link tasks to events
* Store application data in SQL Server
* REST API for task and event operations
* Docker support for the Web API and SQL Server

## Technologies

* .NET 10
* Blazor WebAssembly
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Docker
* Docker Compose

## Project Structure

```text
ScheduleGo
├── BlazorApp
│   └── Blazor frontend
│
├── WebApi
│   ├── Controllers
│   ├── Services
│   ├── Migrations
│   ├── Dockerfile
│   └── ASP.NET Core Web API
│
├── docker-compose.yml
├── .dockerignore
├── .env.example
└── .gitignore
```

## How the Application Works

During development, the recommended setup is:

```text
Blazor App
Visual Studio
     |
     | HTTP requests
     v
Web API
Docker container
     |
     | Entity Framework Core
     v
SQL Server
Docker container
```

The Blazor frontend communicates with the ASP.NET Core Web API.

The Web API handles application logic and communicates with SQL Server through Entity Framework Core.

## Running the Backend with Docker

### Requirements

Install:

* Docker Desktop
* .NET 10 SDK if you want to run or develop the Blazor frontend locally

### 1. Clone the repository

```bash
git clone https://github.com/Morina8700/ScheduleGo.git
cd ScheduleGo
```

### 2. Create the environment file

Copy:

```text
.env.example
```

and create a new file named:

```text
.env
```

For example:

```env
MSSQL_SA_PASSWORD=YourStrongPassword123!
```

The `.env` file contains local secrets and should not be committed to Git.

### 3. Start the backend

Run:

```bash
docker compose up --build -d
```

Docker Compose will start:

```text
ScheduleGo
├── Web API
└── SQL Server
```

The Web API is available at:

```text
http://localhost:5200
```

SQL Server is exposed on:

```text
localhost:1433
```

### 4. Stop the backend

```bash
docker compose down
```

### Rebuild after Web API changes

If Web API code has changed, rebuild the container:

```bash
docker compose up --build -d
```

## Running the Blazor Frontend

The Blazor application can be run directly from Visual Studio.

You can also start it from the command line:

```bash
dotnet run --project BlazorApp
```

The development architecture is then:

```text
Blazor
Visual Studio
     |
     v
Web API
Docker
     |
     v
SQL Server
Docker
```

This allows frontend development with Visual Studio debugging and hot reload while keeping the backend services isolated inside Docker.

## API

The Web API exposes REST endpoints for managing application data.

For example, task operations are available under:

```text
GET    /api/tasks
GET    /api/tasks/{id}
POST   /api/tasks
PUT    /api/tasks/{id}
PATCH  /api/tasks/{id}/completed
DELETE /api/tasks/{id}
```

Example:

```bash
curl http://localhost:5200/api/tasks
```

## Database

ScheduleGo uses SQL Server with Entity Framework Core.

The database runs inside a Docker container, allowing developers to use the same SQL Server environment without manually installing and configuring a local SQL Server instance.

Entity Framework migrations are used to create and update the database schema.

## Environment Variables

The SQL Server password is stored locally in `.env`.

Example:

```env
MSSQL_SA_PASSWORD=YourStrongPassword123!
```

Do not commit the real `.env` file.

The repository should contain `.env.example` instead so other developers know which environment variables are required.

## Docker Services

The Docker Compose configuration currently runs two backend services.

### Web API

The Web API is built from:

```text
WebApi/Dockerfile
```

and exposed on:

```text
localhost:5200
```

### SQL Server

SQL Server uses the official Microsoft SQL Server 2022 Docker image.

The API communicates with SQL Server using Docker's internal network and the database service name rather than `localhost`.

## Development Workflow

For normal development:

```text
Visual Studio
├── Blazor
└── optionally Web API

Docker
└── SQL Server
```

For testing the containerized backend:

```text
Visual Studio
└── Blazor

Docker
├── Web API
└── SQL Server
```

Avoid running the Web API from Visual Studio and Docker on the same port at the same time.

## About the Project

ScheduleGo was created as a small project for experimenting with full-stack .NET development.

The project demonstrates how a Blazor frontend, ASP.NET Core REST API, Entity Framework Core, SQL Server, and Docker can work together in a modern development environment.

The goal is to provide a simple application for organizing events and tasks while also serving as a practical project for learning backend APIs, database relationships, containerization, and frontend-to-backend communication.
