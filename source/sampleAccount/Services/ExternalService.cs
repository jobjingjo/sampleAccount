using System;
using System.Threading.Tasks;
using PuppeteerSharp;
using sampleAccount.Abstract;

namespace sampleAccount.Services
{
    public class ExternalService : IExternalService, IDisposable
    {
        private Browser _browser;
        private bool disposed;

        public Browser browser => browserAsync().Result;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<string> GetIBAN()
        {
            var page = await browser.NewPageAsync();
            await page.GoToAsync("http://randomiban.com/?country=Netherlands");
            //<p id="demo" class="ibandisplay">NL71ABNA5985398153</p>
            var timeout = TimeSpan.FromSeconds(30).Milliseconds; // default value
            await page.WaitForSelectorAsync("#demo", new WaitForSelectorOptions {Timeout = timeout});

            var innerHtml = await page.QuerySelectorAsync("#demo")
                .EvaluateFunctionAsync<string>("node => node.innerHTML");
            await page.CloseAsync();
            return innerHtml;
        }

        public async Task<Browser> browserAsync()
        {
            if (_browser != null)
            {
                return _browser;
            }

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            return _browser;
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any other managed objects here.
                //
                Task.Run(async () => await browser.CloseAsync()).Wait();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        ~ExternalService()
        {
            Dispose(false);
        }
    }
}