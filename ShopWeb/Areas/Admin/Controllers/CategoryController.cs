using Microsoft.AspNetCore.Mvc;
using ShopWeb.Data;
using ShopWeb.DataAccess.Repository.IRepository;
using ShopWeb.Models;
using System.Security.Cryptography;

namespace ShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
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
            Category? category = _unitOfWork.Category.Get(u => u.Id == Id);
            if (category == null) { return NotFound(); }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category? category)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
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
            Category? category = _unitOfWork.Category.Get(u => u.Id == Id);
            if (category == null) { return NotFound(); }
            return View(category);
        }
        [HttpPost, ActionName("delete")]
        public IActionResult DeleteData(int? Id)
        {
            Category? category = _unitOfWork.Category.Get(u => u.Id == Id);
            if (category == null)
            {
                TempData["error"] = "Somthing went wrong";
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
        }

    }
}
