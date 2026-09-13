# Project structure

The source is organised around application responsibilities.

```text
BrightProgramming.AiSupportAssistant.Api/
├── Ai/
│   ├── Answer/
│   ├── Matcher/
│   └── Exceptions/
├── Configuration/
├── Constants/
├── Controllers/
├── Knowledge/
│   ├── Factory/
│   ├── Models/
│   ├── Processor/
│   ├── Providers/
│   └── Repository/
├── Mappers/
├── Models/
├── Services/
└── StartupConfiguration/

BrightProgramming.AiSupportAssistant.Web/
├── Components/
├── Models/
└── Services/

Api.UnitTests/
Api.IntegrationTests/
```

## Where to look

- Request handling: `Controllers`
- Workflow: `Services`
- Knowledge: `Knowledge`
- AI answer generation: `Ai/Answer`
- AI knowledge matching: `Ai/Matcher`
- Configuration: `Configuration` and `StartupConfiguration`
- UI: `Web/Components`
- API tests: the two test projects

The `KnowledgeSources/Markdown` directory contains the Markdown knowledge used by the current provider.
