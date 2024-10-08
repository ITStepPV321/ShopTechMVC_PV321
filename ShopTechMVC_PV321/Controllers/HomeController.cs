﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Entities;
using System.Diagnostics;
using ShopTechMVC_PV321.Models;
using ShopTechMVC_PV321.Helpers;

namespace ShopTechMVC_PV321.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly ShopTechMVCDbContext _context;

        public HomeController(ILogger<HomeController> logger,ShopTechMVCDbContext context)
        {
            _logger = logger;
            _context = context;

        
        }

        //ViewData, ViewBag, ModelView
        //https://localhost:7001/Home/Index 
        //https://localhost:7001/ 
        [HttpGet]
        public IActionResult Index(int? category_id)
        {
            //ViewData["message"] = "We are learning...";
            //ViewBag.Users = new List<string> { "Admin", "Author", "Guest" };
            //Example using Cookies
            //append Cookie
            //HttpContext.Response.Cookies.Append("name", "Tetiana");
            //get Cookie
            //ViewBag.NameAuthor = HttpContext.Request.Cookies["name"];
            //delete Cookie
            HttpContext.Response.Cookies.Delete("name");



            List<Category> categories = _context.Categories.ToList();
            categories.Insert(0, new Category() { Id = 0, Name = "All", Description = "All Products" });
            ViewBag.Categories = categories;
            var products = _context.Products.Include(product => product.Category).ToList();
            if (category_id != null && category_id > 0)
            {
                products = products.Where(p => p.CategoryId == category_id).ToList();
            }
            //for defination active link or disable
            if (category_id == null)
            {
                ViewBag.NotActiveCategoryId = 0;
            }
            else {
                ViewBag.NotActiveCategoryId = category_id;
            }

            var productsCartViewModel = products.Select(p => new ProductCartViewModel
            {
                Product=p,
                IsInCart=IsProductInCart(p.Id),

            }).ToList();
         
            return View(productsCartViewModel);
        }

        private bool IsProductInCart(int id) {
            List<int> idsList = HttpContext.Session.GetObject<List<int>>("mycart");
            if (idsList == null) return false;
            return idsList.Contains(id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //https://localhost:7001/Shop 
        [Route("Shop")]
        public IActionResult About() {
            HttpContext.Response.Cookies.Delete("name");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
