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
    public class SummaryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.today = DateTime.Today.ToString("yyyy-MM-dd");
            
            return View();
        }
    }
}