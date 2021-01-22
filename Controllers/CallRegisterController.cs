using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Timothy.Models.Entities;
using Timothy.Models.CallRegister;

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
            ViewBag.isNotFountId = false;

            return View(callRegisters);
        }

        [HttpGet]
        [Route("CallRegister/Registration/{id}")]
        public async Task<IActionResult> Registration(int id)
        {
            var callRegister = await this._callRegister.FindById(id);
            
            if (callRegister == null)
            {
                var callRegisters = await this._callRegister.GetCallRegisters();
                ViewBag.isNotFoundId = true;

                return View(nameof(Index), callRegisters);
            }

            return RedirectToAction("New", "Inquiry", callRegister);
        }

        [HttpGet]
        public async Task<IActionResult> Destory(int id)
        {
            await this._callRegister.DestroyCallRegisterAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}