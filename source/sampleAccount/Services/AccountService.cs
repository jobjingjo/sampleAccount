using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace sampleAccount.Services
{
    public class AccountService
    {
        public async Task<string> GetIBAN()
        {
            //await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("http://randomiban.com/?country=Netherlands");
            //<p id="demo" class="ibandisplay">NL71ABNA5985398153</p>
            var timeout = TimeSpan.FromSeconds(30).Milliseconds; // default value
            await page.WaitForSelectorAsync("#demo", new WaitForSelectorOptions { Timeout = timeout });

            var innerHtml = await page.QuerySelectorAsync("#demo").EvaluateFunctionAsync<string>("node => node.innerHTML");

            //var element = await page.QuerySelectorAsync("#demo");
            //var innerHtml = await element.GetPropertyAsync("innerHTML").Result.JsonValueAsync<string>();

            return innerHtml;
        }
    }
}
