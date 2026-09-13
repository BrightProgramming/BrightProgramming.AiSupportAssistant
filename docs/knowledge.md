# Knowledge

The current knowledge source is a collection of Markdown files stored under the API project's configured knowledge path.

## Adding knowledge

Add a `.md` file beneath the configured `KnowledgeSource:Path` directory.

The repository searches recursively, so files may be organised into subdirectories.

The file name becomes the document `Name` and the complete file contents become the document `Content`.

## Current knowledge

The repository currently contains guidance covering topics such as:

- Dependency injection
- The dispose pattern
- Garbage collection
- HTTP client factory
- Logging
- The Options pattern

These documents are themselves part of the application's knowledge base and are version controlled with the source code.

## Loading behaviour

`MarkdownKnowledgeProvider` loads the knowledge lazily on its first request and caches the resulting `KnowledgeContext`.

Changing a Markdown file therefore does not automatically change the in-memory knowledge for an already-running application. Restarting the application causes the provider to create a new cached context.

## Relevance matching

The complete available knowledge is passed to the configured AI knowledge matcher. The current OpenAI matcher returns document names that it considers relevant.

Only documents whose names match returned names are included in the resulting `KnowledgeContext`.

## Writing useful knowledge

Keep documents focused on one technical subject and make the content explicit and self-contained. The answer provider is instructed to use only the supplied matched knowledge.

Avoid assuming that information outside the supplied knowledge will be available to answer a question.
