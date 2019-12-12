using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sampleAccount.Abstract;
using sampleAccount.Models;
using sampleAccount.Services;
using sampleAccount.Web.Models;

namespace sampleAccount.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public AccountController(
            IAccountService accountService,
            ITransactionService transactionService,
            IMapper mapper) {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {            
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAccountModel createAccountModel)
        {
            var account = _mapper.Map<Account>(createAccountModel);
            var result = _accountService.CreateAccount(account);
            return Create();
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
        public IActionResult Deposit([FromBody] TransactionModel transactionModel)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransaction.Type = TransactionType.Deposit;
            var result = _transactionService.Deposit(accountTransaction);
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
        public IActionResult Withdraw([FromBody] TransactionModel transactionModel)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransaction.Type = TransactionType.Withdraw;
            var result = _transactionService.Withdraw(accountTransaction);
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
        public IActionResult Transfer([FromBody] TransactionModel transactionModel)
        {
            var accountTransactionFrom = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransactionFrom.Type = TransactionType.Withdraw;
            var result = _transactionService.Withdraw(accountTransactionFrom);
            if (result.Status != OperationStatus.Ok)
            {
                return BadRequest();           
            }

            var accountTransactionTo = _mapper.Map<AccountTransaction>(transactionModel);
            accountTransactionTo.Type = TransactionType.Deposit;
            result = _transactionService.Deposit(accountTransactionTo);
            if (result.Status != OperationStatus.Ok)
            {
                //return money
                _transactionService.Deposit(accountTransactionTo);
                return BadRequest();
            }
            else {
                return Ok(result.Balance);
            }
        }
    }
}