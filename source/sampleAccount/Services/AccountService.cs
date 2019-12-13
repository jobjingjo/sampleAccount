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
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository) {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }
        public async Task<Account> CreateAccountAsync(Account account)
        {
            return await _accountRepository.CreateAccountAsync(account);
        }

        public Account GetAccountByNumber(string accountNumber)
        {
            return _accountRepository.FindAccount(accountNumber);
        }

        public Account GetAccountByUserName(string name)
        {
            return _accountRepository.FindAccountByOwner(name);
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

        public async Task<int> CountTransactionByAccountNameAsync(string accountName)
        {
            return await _accountRepository.CountTransactionByAccountAsync(accountName);       
        }

        public async Task<IList<AccountTransaction>> GetTransactionByAccountNameAsync(string accountName, Pagination pagination)
        {
            return  await _accountRepository.FindTransactionByAccountAsync(accountName, pagination);
        }
    }
}
