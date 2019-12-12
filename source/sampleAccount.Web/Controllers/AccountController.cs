using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace sampleAccount.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Balance()
        {
            return View();
        }

        public IActionResult Deposit()
        {
            return View();
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        public IActionResult Transfer()
        {
            return View();
        }
    }
}