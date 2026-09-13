# Getting started

The solution targets .NET 10 and contains a Blazor web application, an ASP.NET Core API, and two API test projects.

## Prerequisites

- .NET 10 SDK
- An OpenAI API key for answer generation
- An OpenAI API key for knowledge matching

## Run the API

From the API project directory:

```text
dotnet run
```

The development launch profiles expose HTTP on `http://localhost:5216` and HTTPS on `https://localhost:7217`.

## Run the web application

From the web project directory:

```text
dotnet run
```

The development launch profiles expose HTTP on `http://localhost:5119` and HTTPS on `https://localhost:7043`.

The current web `SupportService` calls the API at `http://localhost:5216`.

## Configure secrets

The API uses ASP.NET Core User Secrets and requires both OpenAI API keys. See [Configuration](configuration.md).

## First question

Open the web application, enter a development question, and select **Ask**.

The UI validates that a question is present, calls the API, and renders the returned Markdown answer.
