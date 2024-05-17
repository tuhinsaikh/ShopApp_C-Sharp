using Microsoft.AspNetCore.Mvc;
using ShopWeb.Data;
using ShopWeb.Models;

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
    }
}
