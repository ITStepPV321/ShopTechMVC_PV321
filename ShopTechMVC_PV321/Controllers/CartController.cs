﻿using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTechMVC_PV321.Helpers;

namespace ShopTechMVC_PV321.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopTechMVCDbContext _context;
        public CartController(ShopTechMVCDbContext context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            List<int> idList = HttpContext.Session.GetObject<List<int>>("mycart");
            if(idList==null) idList = new List<int>();
            var products = _context.Products.Include(p => p.Category).Where(p=>idList.Contains(p.Id)).ToList();
            //working....
            //List <Product> productsInCart=idList.Select(id=> _context.Products.Include(p => p.Category).ToList<Product>().Find(id)).ToList();
           

            return View(products);

        }

        public IActionResult Add(int id)
        {
            if(_context.Products.Find(id)==null) return NotFound();
            List<int> idList = HttpContext.Session.GetObject<List<int>>("mycart");
            if (idList == null)
            {
                idList=new List<int>();

            }
            idList.Add(id); //add id of product to cart
            HttpContext.Session.SetObject<List<int>>("mycart", idList);
            return RedirectToAction("Index","Home");
        }


        public IActionResult Delete(int id)
        {
            if (_context.Products.Find(id) == null)
            {
                return null;
            }

            List<int> idList = HttpContext.Session.GetObject<List<int>>("mycart")!;

            if (idList == null)
            {
                idList = new List<int>();
            }

            idList.Remove(id);

            HttpContext.Session.SetObject<List<int>>("mycart", idList);

            return RedirectToAction("Index");
        }
    }
    //using Dictionary
    // plusCountProduct(int id)
    // minusCountProduct(int id)
}
