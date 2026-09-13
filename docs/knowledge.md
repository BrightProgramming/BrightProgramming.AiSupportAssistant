# Knowledge

The current knowledge source is a directory of Markdown files.

The default configuration is:

```text
Provider = Markdown
Path = KnowledgeSources/Markdown
```

## Loading knowledge

`MarkdownKnowledgeRepository`:

- Resolves the configured path to a full path.
- Recursively searches for `*.md` files.
- Reads each file as text.
- Creates a `KnowledgeDocument` containing the file name and content.
- Ignores other file types.

If the directory does not exist, an empty collection is returned and a warning is logged.

## Caching

`MarkdownKnowledgeProvider` loads the repository contents through a `Lazy<KnowledgeContext>`.

The first request loads the documents. Later requests return the same cached context.

Changing a Markdown file therefore does not update the in-memory knowledge for an already-running provider.

## Matching

The complete knowledge context is passed to the configured AI knowledge matcher.

The matcher returns a new `KnowledgeContext` containing only documents whose names were identified as relevant.

That reduced context is passed to the answer provider.

## Adding content

Add `.md` files beneath the configured knowledge path.

Subdirectories are supported because the repository searches recursively.
