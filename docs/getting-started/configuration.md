# Configuration

The API uses the .NET Options pattern to bind configuration sections to strongly typed options.

## Provider selection

`appsettings.json` currently contains:

```json
"AiAnswer": { "Provider": "OpenAI" },
"AiKnowledgeMatcher": { "Provider": "OpenAI" },
"KnowledgeSource": {
  "Provider": "Markdown",
  "Path": "KnowledgeSources/Markdown"
}
```

The provider names are matched case-insensitively by their factories.

## OpenAI settings

The answer provider uses `AiAnswerOpenAI`:

```json
"AiAnswerOpenAI": {
  "Model": "gpt-5.6-luna"
}
```

The knowledge matcher uses `AiKnowledgeMatcherOpenAI` with its own model setting.

Both sections also require an `ApiKey`.

## Secrets

The API project has a .NET User Secrets ID. API keys should not be committed to `appsettings.json`.

The required secret keys are:

- `AiAnswerOpenAI:ApiKey`
- `AiKnowledgeMatcherOpenAI:ApiKey`

The options validation reports a configuration error when either key or either model is missing or blank.

## Knowledge path

`KnowledgeSource:Path` is resolved by `MarkdownKnowledgeRepository` and searched recursively for `.md` files.

The supplied project uses `KnowledgeSources/Markdown`.

## Configuration validation

Provider names are required. OpenAI API keys and model names are also required by the corresponding OpenAI options.
