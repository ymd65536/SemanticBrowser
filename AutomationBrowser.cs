using System.ComponentModel;
using Microsoft.Playwright;
using Microsoft.SemanticKernel;

class AutomationBrowser
{
    [KernelFunction]
    [Description("ウィンドウで開く")]
    public async Task<string> OpenWindow([Description("URL") ] string url, [Description("ブラウザ")] string browser)
    {
        Console.WriteLine("Open Window");
        Console.WriteLine($"url:{url},ブラウザ:{browser}");
        if(browser == "Google Chrome" || browser == "Chrome"){
            string Ch = "chromium";
            using var playwright = await Playwright.CreateAsync();
            await using var PlaywrightBrowser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                Channel = Ch,
            });
            var page = await PlaywrightBrowser.NewPageAsync();
            await page.GotoAsync(url);
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = $"{Ch}-screenshot.png"
            });
        }else if(browser == "Microsoft Edge"){
            string Ch = "msedge";
            using var playwright = await Playwright.CreateAsync();
            await using var PlaywrightBrowser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                Channel = Ch,
            });
            var page = await PlaywrightBrowser.NewPageAsync();
            await page.GotoAsync(url);
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = $"{Ch}-screenshot.png"
            });
        }
        return $"url:{url},ブラウザ:{browser}";
    }
}

class PageAction{
    private IPage ?page;

    [KernelFunction]
    [Description("特定の要素にテキストを入力する")]
    public async Task<string> InputElement([Description("属性")] string Attr, [Description("属性値")] string AttrValue, [Description("入力するテキスト")] string InputText)
    {
        var searchBox = await page.QuerySelectorAsync(AttrValue);
        if (searchBox != null)
        {
            await searchBox.FillAsync(InputText);
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "InputElementScreenshot.png"
            });
        }else{
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "InputElementNGScreenshot.png"
            });
        }
        return $"{Attr} {AttrValue}";
    }
}
