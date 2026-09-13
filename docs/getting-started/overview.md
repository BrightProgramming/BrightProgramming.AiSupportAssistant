# Getting Started

The solution contains separate Web and API applications. The API provides the support endpoint and the Web application provides the user interface.

## Requirements

The projects target **.NET 10**.

The API uses the `OpenAI` NuGet package and the Web project uses `Markdig` for Markdown rendering.

## Configure the API

The API requires:

- An AI answer provider and model.
- An AI knowledge-matcher provider and model.
- An API key for each configured OpenAI integration.
- A knowledge provider and knowledge path.

See [Configuration](configuration.md).

## Run locally

Start the API project first so the support endpoint is available.

The API launch settings provide:

- HTTP: `http://localhost:5216`
- HTTPS: `https://localhost:7217`

The Web launch settings provide:

- HTTP: `http://localhost:5119`
- HTTPS: `https://localhost:7043`

The Web application's named API client currently targets `http://localhost:5216`.

Run the Web project after the API is available, then open the URL shown by the Web project's launch profile.

## First question

Enter a development question and select **Ask**. The application retrieves and matches its configured knowledge before generating the answer.
