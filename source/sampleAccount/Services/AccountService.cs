﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;
using sampleAccount.Abstract;
using sampleAccount.Models;

namespace sampleAccount.Services
{
    public class AccountService: IAccountService
    {
        public Account CreateAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountByNumber(string accountNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetIBAN()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
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
            await page.CloseAsync();
            return innerHtml;
        }
    }
}
