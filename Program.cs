using Azure.Identity;
using Microsoft.SemanticKernel;

var builder = Kernel.CreateBuilder();
string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
                  ?? throw new InvalidOperationException("Environment variable AZURE_OPENAI_ENDPOINT is not set.");

var KernelBuilder = builder.AddAzureOpenAIChatCompletion(
        deploymentName: "gpt-4o-mini",
        endpoint: endpoint,
        credentials: new AzureCliCredential());

KernelBuilder.Plugins.AddFromType<AutomationBrowser>();
KernelBuilder.Plugins.AddFromType<PageAction>();
Kernel kernel = KernelBuilder.Build();

# pragma warning disable SKEXP0040
var OpenBrowserPrompty = kernel.CreateFunctionFromPromptyFile("OpenBrowser.prompty");
var InputElement = kernel.CreateFunctionFromPromptyFile("InputElement.prompty");
# pragma warning restore SKEXP0040

PromptExecutionSettings settings = new()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

var result = await OpenBrowserPrompty.InvokeAsync(kernel, new KernelArguments(settings));
Console.WriteLine(result.GetValue<string>());

var ElementResult = await InputElement.InvokeAsync(kernel, new KernelArguments(settings));
Console.WriteLine(ElementResult.GetValue<string>());
