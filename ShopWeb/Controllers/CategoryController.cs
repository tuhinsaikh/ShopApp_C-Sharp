using Microsoft.AspNetCore.Mvc;
using ShopWeb.Data;
using ShopWeb.Models;
using System.Security.Cryptography;

namespace ShopWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

         public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            else
            {
                TempData["error"] = "Somthing went wrong";
                return View();
            }
        }  
        public IActionResult Edit(int? Id)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if (category == null) { return NotFound(); }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category? category)
        {

            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            else
            {
                TempData["error"] = "Somthing went wrong";
                return View();
            }
        }
        public IActionResult Delete(int? Id)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if (category == null) { return NotFound(); }
            return View(category);
        }
        [HttpPost, ActionName("delete")]
        public IActionResult DeleteData(int? Id)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if(category == null) {
                TempData["error"] = "Somthing went wrong";
                return NotFound(); 
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
