using Microsoft.AspNetCore.Mvc;
using ShopWeb.DataAccess.Repository;
using ShopWeb.DataAccess.Repository.IRepository;
using ShopWeb.Models;

namespace ShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["error"] = "Somthing went wrong";
                return View();
            }
        }
        public IActionResult Edit(int? Id)
        {
            Product? Product = _unitOfWork.Product.Get(u => u.Id == Id);
            if (Product == null) { return NotFound(); }
            return View(Product);
        }
        [HttpPost]
        public IActionResult Edit(Product? product)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["error"] = "Somthing went wrong";
                return View();
            }
        }
        public IActionResult Delete(int? Id)
        {
            Product? Product = _unitOfWork.Product.Get(u => u.Id == Id);
            if (Product == null) { return NotFound(); }
            return View(Product);
        }
        [HttpPost, ActionName("delete")]
        public IActionResult DeleteData(int? Id)
        {
            Product? Product = _unitOfWork.Product.Get(u => u.Id == Id);
            if (Product == null)
            {
                TempData["error"] = "Somthing went wrong";
                return NotFound();
            }
            _unitOfWork.Product.Remove(Product);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index", "Product");
        }
    }
}
