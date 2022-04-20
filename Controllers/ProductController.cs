using Info.Models;

using Info.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Info.Controllers
{


    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDBContext db,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
           _webHostEnvironment = webHostEnvironment;

        }



        //get
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _db.Product;
            //ViewData["ProductList"] = objProductList;


            foreach (var obj in objProductList)
            {
                obj.Category = _db.Categories.FirstOrDefault(u => u.Id == obj.CategoryId);
            };
            return View(objProductList);
        }
        //Get Upsert
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryDropdown = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            //For viewing dropdown
           ViewData["CategoryDropdown"] = CategoryDropdown;
            ViewBag.CategoryDropdown = CategoryDropdown;
            Product product = new Product();
            //ProductVM productVM = new ProductVM();
            //{
            //    Product product = new Product(),
            //    CategorySelectList = _db.Categories.Select(i => new SelectListItem()
            //    {

            //        Value = i.Id.ToString()
            //        Text = i.Name
            //    });
            //};
            if (id == null)
            {
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);
                //product = _db.Product.Find(Id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }

        }



        //Post Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                String webRootpath = _webHostEnvironment.WebRootPath;
                if(productVM.Product.Id==0)
                {
                    string upload = webRootpath + WC.Imagepath;
                    string filename= Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using(var filestream= new FileStream(Path.Combine(upload,filename+extension),FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    productVM.Product.Image = filename + extension;
                    _db.Product.Add(productVM.Product);
                }
              
                else
                {

                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
           
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(Id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");



        }



    }
}