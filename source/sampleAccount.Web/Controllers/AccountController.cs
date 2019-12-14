using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using sampleAccount.Abstract;
using sampleAccount.Models;
using sampleAccount.Services;
using sampleAccount.Web.Models;

namespace sampleAccount.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IOptions<SettingConfiguration> _config;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IExternalService _externalService;
        private readonly IMapper _mapper;

        public AccountController(
            IOptions<SettingConfiguration> config,
            IAccountService accountService,
            ITransactionService transactionService,
            IExternalService externalService,
            IMapper mapper) {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _externalService = externalService ?? throw new ArgumentNullException(nameof(externalService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public string CurrentUserName
        {
            get { return HttpContext.User.Identity.Name; }
        }

        public IActionResult Index()
        {
            var account =_accountService.GetAccountByUserName(CurrentUserName);
            ViewData["Balance"] = account.Balance;
            ViewData["IBAN"] = account.AccountName;
            return View();
        }

        public async Task<IActionResult> CreateAsync()
        {
            var account = _accountService.GetAccountByUserName(CurrentUserName);
            if (account!=null)
            {
                return RedirectToAction("Index", "Account");
            }
            account = new Account();
            account.AccountName = await _externalService.GetIBAN();
            account.Owner = HttpContext.User.Identity.Name;
            account = await _accountService.CreateAccountAsync(account);
            return View("Create", account);
        }

        public async Task<IActionResult> Transactions(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            int pageSize = 10;

            List<TransactionModel> items = new List<TransactionModel>();
            var account = _accountService.GetAccountByUserName(CurrentUserName);
            if (account == null)
            {                
                return Ok(new PaginatedList<TransactionModel>(items, 0, pageNumber ?? 1, pageSize));
            }
            var count = await _accountService.CountTransactionByAccountNameAsync(account.AccountName);
            var result = await _accountService.GetTransactionByAccountNameAsync(account.AccountName, new Pagination(pageNumber ?? 1, pageSize));         
            items = _mapper.Map<IList<AccountTransaction>, List<TransactionModel>>(result);
            return Ok(new PaginatedList<TransactionModel>(items, count, pageNumber ?? 1, pageSize));
        }

        public IActionResult Balance(string accountNumber)
        {
            var account = _accountService.GetAccountByNumber(accountNumber);
            return Ok(account.Balance);
        }

        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit([FromBody] TransactionModel transactionModel)
        {
            var account = _accountService.GetAccountByUserName(CurrentUserName);
            var accountTransaction = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransaction.AccountName = account.AccountName;
            accountTransaction.Type = TransactionType.Deposit;

            var fee = _config.Value.DepositFeeInPercent * accountTransaction.Amount / 100;

            var result = await _transactionService.DepositAsync(accountTransaction, fee);
            if (result.Status == OperationStatus.Ok)
            {
                return Ok(result.Balance);
            }
            else {
                return BadRequest();
            }
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw([FromBody] TransactionModel transactionModel)
        {
            var account = _accountService.GetAccountByUserName(CurrentUserName);
            var accountTransaction = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransaction.AccountName = account.AccountName;
            accountTransaction.Type = TransactionType.Withdraw;

            var result = await _transactionService.WithdrawAsync(accountTransaction);
            if (result.Status == OperationStatus.Ok)
            {
                return Ok(result.Balance);
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer([FromBody] TransactionModel transactionModel)
        {
            var account = _accountService.GetAccountByUserName(CurrentUserName);
            var accountTransactionFrom = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransactionFrom.AccountName = account.AccountName;
            accountTransactionFrom.Type = TransactionType.Withdraw;

            var result = await _transactionService.WithdrawAsync(accountTransactionFrom);
            if (result.Status != OperationStatus.Ok)
            {
                return BadRequest();           
            }

            var accountTransactionTo = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransactionTo.AccountName = transactionModel.TargetAccountNumber;
            accountTransactionTo.Type = TransactionType.Deposit;
            result = await _transactionService.DepositAsync(accountTransactionTo, 0);
            if (result.Status != OperationStatus.Ok)
            {
                //return money
                accountTransactionTo.AccountName = account.AccountName;
                await _transactionService.DepositAsync(accountTransactionTo, 0);
                return BadRequest();
            }
            else {
                return Ok(result.Balance);
            }
        }
    }
}