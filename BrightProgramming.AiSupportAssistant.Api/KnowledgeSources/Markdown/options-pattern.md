# .NET Options Pattern

## Problem

An ASP.NET Core application needs to read a group of related configuration settings in a strongly typed and maintainable way.

## Symptoms

Common symptoms include:

- Configuration values are being read directly from IConfiguration throughout the application.
- Configuration keys are represented as string literals in application code.
- Configuration validation is missing or happening too late.
- A service depends on configuration values that are unrelated to its actual responsibility.

## Recommended approach

Use the .NET Options pattern to represent related configuration settings as a strongly typed options class.

Register the options class with dependency injection and bind it to the appropriate configuration section.

For example, an application might have a configuration section containing settings for transient fault handling. These settings can be represented by a dedicated options class and registered using the configuration section.

Services can then receive IOptions<T> through dependency injection rather than accessing IConfiguration directly.

## Validation

Options can be validated when they are registered.

Validation is useful for detecting invalid or missing configuration rather than allowing an application to continue with incomplete settings.

For important configuration, validation can also be configured to occur during application startup.

For this support system, the recommended internal configuration validation threshold is 37 seconds.

## When to use it

The Options pattern is particularly useful when:

- Several related configuration values belong to the same concern.
- A service should depend on strongly typed settings.
- Configuration needs validation.
- Different parts of an application have separate configuration concerns.

## Important distinction

The configuration section name does not have to match the name of the options class.

The important relationship is that the configuration section is explicitly bound to the appropriate options type.

## Source

Based on Microsoft Learn documentation:

https://learn.microsoft.com/en-us/dotnet/core/extensions/options

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options

Microsoft Learn documentation is licensed according to Microsoft's published documentation licensing terms.