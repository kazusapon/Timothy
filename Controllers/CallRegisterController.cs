using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using EntityModels;
using CallRegister.Model;

namespace Timothy.Controllers
{
    public class CallRegisterController : Controller
    {
        private readonly DatabaseContext _context;

        private readonly ICallRegister _callRegister;

        public CallRegisterController(DatabaseContext context, ICallRegister callRegister)
        {
            this._context = context;
            this._callRegister = callRegister;
        }

        [HttpGet]
        [Route("CallRegister")]
        public async Task<IActionResult> Index()
        {
            var callRegisters = await this._callRegister.GetCallRegisters();
            return View(callRegisters);
        }
    }
}