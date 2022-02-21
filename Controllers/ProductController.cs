using Info.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Info.Controllers
{
    
   
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _db;

        public ProductController(ApplicationDBContext db)
        {
            _db = db;

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
            // viewbag- viewbag is used  transfer data to controller to view and it takes any number of propertie and value.it transfer vice versa.
            //Viewdata - it is also like a viewbag but syntax is diffrent and if we use viewdata first we typecast value but not transfer vice versa.
            // ViewModel contain field that are represented in the view , it have specific validation rules, it helps to strongly  typed views.
            ViewBag.CategoryDropdown = CategoryDropdown;   
            Product product = new Product();
            if (id == null)
            {
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);
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
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);

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