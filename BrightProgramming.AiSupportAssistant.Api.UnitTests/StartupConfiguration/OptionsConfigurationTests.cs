using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.StartupConfiguration;

public class OptionsConfigurationTests
{
    [Fact]
    public void AddOptions_BindsAllConfiguredOptionGroups()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AiAnswer:Provider"] = "OpenAI",
                ["KnowledgeSource:Provider"] = "Markdown",
                ["KnowledgeSource:Path"] = "KnowledgeSources/Markdown",
                ["AiKnowledgeMatcher:Provider"] = "OpenAI",
                ["AiAnswerOpenAI:ApiKey"] = "answer-key",
                ["AiAnswerOpenAI:Model"] = "answer-model",
                ["AiKnowledgeMatcherOpenAI:ApiKey"] = "matcher-key",
                ["AiKnowledgeMatcherOpenAI:Model"] = "matcher-model"
            })
            .Build();
        using var provider = CreateProvider(configuration);

        var answer = provider.GetRequiredService<IOptions<AiAnswerOptions>>().Value;
        var knowledge = provider.GetRequiredService<IOptions<KnowledgeSourceOptions>>().Value;
        var matcher = provider.GetRequiredService<IOptions<AiKnowledgeMatcherOptions>>().Value;
        var answerOpenAi = provider.GetRequiredService<IOptions<AiAnswerOpenAiOptions>>().Value;
        var matcherOpenAi = provider.GetRequiredService<IOptions<AiKnowledgeMatcherOpenAiOptions>>().Value;

        Assert.Equal("OpenAI", answer.Provider);
        Assert.Equal("Markdown", knowledge.Provider);
        Assert.Equal("KnowledgeSources/Markdown", knowledge.Path);
        Assert.Equal("OpenAI", matcher.Provider);
        Assert.Equal("answer-key", answerOpenAi.ApiKey);
        Assert.Equal("answer-model", answerOpenAi.Model);
        Assert.Equal("matcher-key", matcherOpenAi.ApiKey);
        Assert.Equal("matcher-model", matcherOpenAi.Model);
    }

    [Theory]
    [InlineData("AiAnswer:Provider", "AI provider must be configured.")]
    [InlineData("KnowledgeSource:Provider", "Knowledge source provider must be configured.")]
    [InlineData("AiKnowledgeMatcher:Provider", "AI knowledge matcher provider must be configured.")]
    [InlineData("AiAnswerOpenAI:ApiKey", "AI Answer OpenAI API key must be configured.")]
    [InlineData("AiAnswerOpenAI:Model", "AI Answer OpenAI model must be configured.")]
    [InlineData("AiKnowledgeMatcherOpenAI:ApiKey", "AI Knowledge Matcher OpenAI API key must be configured.")]
    [InlineData("AiKnowledgeMatcherOpenAI:Model", "AI Knowledge Matcher OpenAI model must be configured.")]
    public void AddOptions_WhenRequiredValueIsMissing_ThrowsValidationException(
        string missingKey,
        string expectedMessage)
    {
        var values = new Dictionary<string, string?>
        {
            ["AiAnswer:Provider"] = "OpenAI",
            ["KnowledgeSource:Provider"] = "Markdown",
            ["AiKnowledgeMatcher:Provider"] = "OpenAI",
            ["AiAnswerOpenAI:ApiKey"] = "answer-key",
            ["AiAnswerOpenAI:Model"] = "answer-model",
            ["AiKnowledgeMatcherOpenAI:ApiKey"] = "matcher-key",
            ["AiKnowledgeMatcherOpenAI:Model"] = "matcher-model"
        };
        values.Remove(missingKey);
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
        using var provider = CreateProvider(configuration);

        var exception = Assert.Throws<OptionsValidationException>(() =>
            GetOptionsForKey(provider, missingKey));

        Assert.Contains(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData("AiAnswer:Provider")]
    [InlineData("KnowledgeSource:Provider")]
    [InlineData("AiKnowledgeMatcher:Provider")]
    [InlineData("AiAnswerOpenAI:ApiKey")]
    [InlineData("AiAnswerOpenAI:Model")]
    [InlineData("AiKnowledgeMatcherOpenAI:ApiKey")]
    [InlineData("AiKnowledgeMatcherOpenAI:Model")]
    public void AddOptions_WhenRequiredValueIsWhitespace_ThrowsValidationException(string invalidKey)
    {
        var values = new Dictionary<string, string?>
        {
            ["AiAnswer:Provider"] = "OpenAI",
            ["KnowledgeSource:Provider"] = "Markdown",
            ["AiKnowledgeMatcher:Provider"] = "OpenAI",
            ["AiAnswerOpenAI:ApiKey"] = "answer-key",
            ["AiAnswerOpenAI:Model"] = "answer-model",
            ["AiKnowledgeMatcherOpenAI:ApiKey"] = "matcher-key",
            ["AiKnowledgeMatcherOpenAI:Model"] = "matcher-model"
        };
        values[invalidKey] = " ";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
        using var provider = CreateProvider(configuration);

        Assert.Throws<OptionsValidationException>(() => GetOptionsForKey(provider, invalidKey));
    }

    private static ServiceProvider CreateProvider(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddOptions(configuration);
        return services.BuildServiceProvider();
    }

    private static object GetOptionsForKey(ServiceProvider provider, string key) => key switch
    {
        "AiAnswer:Provider" => provider.GetRequiredService<IOptions<AiAnswerOptions>>().Value,
        "KnowledgeSource:Provider" => provider.GetRequiredService<IOptions<KnowledgeSourceOptions>>().Value,
        "AiKnowledgeMatcher:Provider" => provider.GetRequiredService<IOptions<AiKnowledgeMatcherOptions>>().Value,
        "AiAnswerOpenAI:ApiKey" or "AiAnswerOpenAI:Model" => provider.GetRequiredService<IOptions<AiAnswerOpenAiOptions>>().Value,
        "AiKnowledgeMatcherOpenAI:ApiKey" or "AiKnowledgeMatcherOpenAI:Model" => provider.GetRequiredService<IOptions<AiKnowledgeMatcherOpenAiOptions>>().Value,
        _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
    };
}
