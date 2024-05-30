using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll("Category").ToList();
            return View(products);
        }
        //public IActionResult Create()
        //{
        //    ProductVM productVM = new()
        //    {
        //        CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        }),
        //        Product = new Product()
        //    };
        //    return View(productVM);
        //}
        public IActionResult Upsert(int? Id)
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
            if (Id != null || Id < 1)
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == Id);
                return View(productVM);
                
            }
            else
            {
                return View(productVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    if (!string.IsNullOrEmpty(productVm.Product.ImgUrl))
                    {
                        var oldImagePath = 
                            Path.Combine(wwwRootPath, productVm.Product.ImgUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.Product.ImgUrl = @"\images\product\" + fileName;
                }
                if (productVm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVm.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVm.Product);
                    TempData["success"] = "Product updated successfully";
                }
                _unitOfWork.Save();
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
        //public IActionResult Create(ProductVM productVm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(productVm.Product);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product created successfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    else
        //    {
        //        productVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });
        //        TempData["error"] = "Somthing went wrong";
        //        return View(productVm);
        //    }
        //}
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
        // api calls 
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = _unitOfWork.Product.GetAll("Category").ToList();
            return Json(new {data=products});
        }
    }
}
