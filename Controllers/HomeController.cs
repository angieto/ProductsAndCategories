using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProsNCats.Models;
// add these lines for validation
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
// add these lines for session
using Microsoft.AspNetCore.Http;

namespace ProsNCats.Controllers
{
    public class HomeController : Controller
    {
        // Context Service
        private ProsNCatsContext dbContext;
        public HomeController(ProsNCatsContext context) {
            dbContext = context;
        }

        // ROUTES
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("CreateProduct");
        }

        [HttpGet("products")]
        public IActionResult CreateProduct()
        {
            // show a list of exsiting products
            var ProductList = dbContext.Products.OrderByDescending(product => product.CreatedAt).ToList();
            ViewBag.ProductList = ProductList;
            return View("CreateProduct");
        }

        [HttpPost("products")]
        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(product);
                dbContext.SaveChanges();
                return RedirectToAction("CreateProduct");
            }
            return View("CreateProduct");
        }

        [HttpGet("categories")]
        public IActionResult CreateCategory()
        {
            // show a list of existing categories
            var CategoryList = dbContext.Categories.OrderByDescending(category => category.CreatedAt).ToList();
            ViewBag.CategoryList = CategoryList;
            return View("CreateCategory");
        }

        [HttpPost("categories")]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction("CreateCategory");
            }
            return View("CreateCategory");
        }

        [HttpGet("products/{productId}")]
        public IActionResult Detail(int productId)
        {
            // show a list of categories that belong to the selected product
            Product Product = dbContext.Products.Include(p => p.Associations).ThenInclude(a => a.Category).FirstOrDefault(p => p.ProductId == productId);
            IEnumerable<Category> UsedCategories = Product.Associations.Select(a => a.Category);
            IEnumerable<Category> UnusedCategories = dbContext.Categories.Where(cat => !cat.Associations.Any(a => a.ProductId == productId));
            ViewBag.Product = Product;
            ViewBag.UsedCategories = UsedCategories;
            ViewBag.UnusedCategories = UnusedCategories;
            return View("Detail");
        }

        [HttpPost("products/{productId}")]
        public IActionResult UpdateProduct(int productId, int categoryId)
        {
            // Console.WriteLine("Current Product Id " + productId + " and categoryID " + categoryId);
            Product selectedProduct = dbContext.Products.Include(p => p.Associations).FirstOrDefault(p => p.ProductId == productId);
            Boolean found = selectedProduct.Associations.Any(a => a.CategoryId == categoryId);
            if (!found) {
                Association connection = new Association() 
                {
                    CategoryId = categoryId,
                    ProductId = productId
                };
                selectedProduct.Associations.Add(connection);
                dbContext.SaveChanges();
                return RedirectToAction("Detail", new {productId = productId});
            }
            return View("Detail", new {productId = productId});
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult Category(int categoryId) 
        {
            // show a list of products that belongs to the selected categories
            Category Category = dbContext.Categories.Include(c => c.Associations).ThenInclude(a => a.Product).FirstOrDefault(c => c.CategoryId == categoryId);
            IEnumerable<Product> UsedProducts = Category.Associations.Select(a => a.Product);
            IEnumerable<Product> UnusedProducts = dbContext.Products.Where(pro => !pro.Associations.Any(a => a.CategoryId == categoryId)); 
            ViewBag.Category = Category;
            ViewBag.UsedProducts = UsedProducts;
            ViewBag.UnusedProducts = UnusedProducts;
            return View("Category");
        }

        [HttpPost("category/{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, int productId)
        {
            Category selectedCategory = dbContext.Categories.Include(c => c.Associations).FirstOrDefault(c => c.CategoryId == categoryId);
            Boolean found = selectedCategory.Associations.Any(a => a.ProductId == productId);
            if (!found) {
                Association connection = new Association()
                {
                    CategoryId = categoryId,
                    ProductId = productId
                };
                selectedCategory.Associations.Add(connection);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Category", new {categoryId = categoryId});
        }
    }
}

