using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using EntityModels;
using Inquiry.Model;
using System.Model;
using ContactMethod.Model;
using Inquiry.View.Models;
using GuestType.Model;
using User.Model;
using Classification.Model;
using Form.View.Models;

namespace Timothy.Controllers
{
    public class CallRegisterController : Controller
    {
        public CallRegisterController()
        {

        }

        [HttpGet]
        [Route("CallRegister")]
        public IActionResult Index()
        {
            return View();
        }
    }
}