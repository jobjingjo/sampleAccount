using PuppeteerSharp;
using sampleAccount.Abstract;
using System;
using System.Threading.Tasks;

namespace sampleAccount.Services
{
    public class ExternalService: IExternalService,IDisposable
    {
        bool disposed = false;
        private Browser _browser;
        public async Task<Browser> browserAsync() {
            if (_browser != null) return _browser;
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            return _browser;
        }

        public Browser browser {
            get {
                return browserAsync().Result;
            }
        }

        public async Task<string> GetIBAN()
        {
            var page = await browser.NewPageAsync();
            await page.GoToAsync("http://randomiban.com/?country=Netherlands");
            //<p id="demo" class="ibandisplay">NL71ABNA5985398153</p>
            var timeout = TimeSpan.FromSeconds(30).Milliseconds; // default value
            await page.WaitForSelectorAsync("#demo", new WaitForSelectorOptions { Timeout = timeout });

            var innerHtml = await page.QuerySelectorAsync("#demo").EvaluateFunctionAsync<string>("node => node.innerHTML");
            await page.CloseAsync();
            return innerHtml;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

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
