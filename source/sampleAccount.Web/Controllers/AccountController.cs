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
            var account =_accountService.GetAccountByUserName(HttpContext.User.Identity.Name);
            ViewData["Balance"] = account.Balance;
            ViewData["IBAN"] = account.AccountName;
            return View();
        }

        public async Task<IActionResult> CreateAsync()
        {
            var account = _accountService.GetAccountByUserName(HttpContext.User.Identity.Name);
            if (account!=null)
            {
                return RedirectToAction("Index", "Account");
            }
            account = new Account();
            account.AccountName = await _accountService.GetIBAN();
            account.Owner = HttpContext.User.Identity.Name;
            account = await _accountService.CreateAccountAsync(account);
            return View("Create", account);
        }

        public async Task<IActionResult> TransactionAsync(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            //var students = from s in _context.Students
            //               select s;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    students = students.Where(s => s.LastName.Contains(searchString)
            //                           || s.FirstMidName.Contains(searchString));
            //}
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        students = students.OrderByDescending(s => s.LastName);
            //        break;
            //    case "Date":
            //        students = students.OrderBy(s => s.EnrollmentDate);
            //        break;
            //    case "date_desc":
            //        students = students.OrderByDescending(s => s.EnrollmentDate);
            //        break;
            //    default:
            //        students = students.OrderBy(s => s.LastName);
            //        break;
            //}
            List<TransactionModel> items = new List<TransactionModel>();
            int pageSize = 3;
            //return View(await PaginatedList<TransactionModel>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
            return View(await PaginatedList<TransactionModel>.CreateAsync(items.AsQueryable(), pageNumber ?? 1, pageSize));
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