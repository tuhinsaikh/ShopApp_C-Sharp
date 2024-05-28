using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopWeb.DataAccess.Repository;
using ShopWeb.DataAccess.Repository.IRepository;
using ShopWeb.Models;
using ShopWeb.Models.ViewModel;

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
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                TempData["error"] = "Somthing went wrong";
                return View(productVm);
            }
        }
        public IActionResult Edit(int? Id)
        {
            Product? Product = _unitOfWork.Product.Get(u => u.Id == Id);
            if (Product == null) { return NotFound(); }
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = Product
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Edit(ProductVM? productVm)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(productVm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
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
