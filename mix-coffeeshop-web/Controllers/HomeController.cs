using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mix_coffeeshop_web.Models;

namespace mix_coffeeshop_web.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository repo;

        public HomeController(IProductRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult MenuManager()
        {
            var api = new ProductController(repo);
            var products = api.Get();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            return View(new Product());
        }

        [HttpPost]
        public IActionResult AddItem(Product data)
        {
            var api = new ProductController(repo);
            api.CreateNewProduct(data);
            return RedirectToAction("MenuManager");
        }

        [HttpGet]
        public IActionResult EditItem(int id)
        {
            var api = new ProductController(repo);
            var selectedProduct = api.Get().FirstOrDefault(it=> it.Id == id);
            return View(selectedProduct);
        }

        [HttpPost]
        public IActionResult EditItem(Product data)
        {
            var api = new ProductController(repo);
            var selectedProduct = api.UpdateProduct(data);
            return RedirectToAction("MenuManager");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
