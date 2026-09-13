# Configuration

The API binds its configuration to strongly typed options.

## Provider selection

| Section | Setting | Current value |
|---|---|---|
| `AiAnswer` | `Provider` | `OpenAI` |
| `AiKnowledgeMatcher` | `Provider` | `OpenAI` |
| `KnowledgeSource` | `Provider` | `Markdown` |
| `KnowledgeSource` | `Path` | `KnowledgeSources/Markdown` |

The provider values are used by the corresponding factories and are matched case-insensitively.

## OpenAI settings

Answer generation uses:

```text
AiAnswerOpenAI:ApiKey
AiAnswerOpenAI:Model
```

Knowledge matching uses:

```text
AiKnowledgeMatcherOpenAI:ApiKey
AiKnowledgeMatcherOpenAI:Model
```

Both API keys and both model names are validated as non-empty.

The checked-in `appsettings.json` contains the model name but not the API keys.

## Secrets

The API project has a `UserSecretsId`, so local secrets can be stored with ASP.NET Core User Secrets rather than committed to source control.

Example:

```text
dotnet user-secrets set "AiAnswerOpenAI:ApiKey" "..."
dotnet user-secrets set "AiKnowledgeMatcherOpenAI:ApiKey" "..."
```

## Web API address

`Web/appsettings.json` contains `Api:BaseUrl`, but the current web `SupportService` does not read it. The API base address is currently hard-coded as `http://localhost:5216`.

This is a current implementation detail and should be considered before deploying the web application outside the local development setup.
