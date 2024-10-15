using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppStore.Models;
using WebAppStore.Repository;
using WebAppStore.ViewModels;

namespace WebAppStore.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly StoreContextDB db;
        private readonly IProductRepository ProductRepository;
        public ProductController(StoreContextDB context , IProductRepository _ProductRepo)
        {
            db = context;
            ProductRepository = _ProductRepo;
        }
        [Authorize]
        public IActionResult Index()
        {
            
            return View(ProductRepository.GetAll());
        }

        public IActionResult Details(int Id)
        {

            return View(ProductRepository.GetById(Id));
         }


        [Authorize(Roles = "Admin")]
        public IActionResult Add() //Add
        {
            ViewBag.categories = db.Categories.ToList();
            return View();
        }

        public IActionResult addd() {
            ViewBag.categories = db.Categories.ToList();

            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Save(AddProductVM model)
        {
            if (!ModelState.IsValid)
            {

                // Return the view with the model to show validation errors
                return View("Add", model);
            }

            try
            {
                // Create and save the product
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId
                };

                db.Products.Add(product);
                db.SaveChanges(); // Save product first to generate ProductId for the images

                // Save uploaded files
                if (model.Files != null && model.Files.Any())
                {
                    foreach (var file in model.Files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            // Generate a unique file name to prevent overwriting
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            var filePath = Path.Combine("/images/products", uniqueFileName);
                            var filePathfolder = Path.Combine("wwwroot/images/products", uniqueFileName);

                            //// Ensure the directory exists
                            //Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                            // Save the file to the specified path
                            using (var stream = new FileStream(filePathfolder, FileMode.Create))
                            {
                               await file.CopyToAsync(stream);
                            }

                            // Add image information to the database
                            var productImage = new ProductImage
                            {
                                Path = filePath,
                                ProductId = product.Id
                            };

                            db.ProductImages.Add(productImage);
                        }
                    }

                    db.SaveChanges(); // Save all images after they are added to the database
                }

                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error and show an error message)
                ModelState.AddModelError("", "An error occurred while saving the product. Please try again.");
                return View("Add", model);
            }
        }



        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int Id)
        {
            // Ensure 'db' is not null
            if (db == null)
            {
                // Handle the error, perhaps log it or return an error view
                return StatusCode(StatusCodes.Status500InternalServerError, "Database context is not initialized.");
            }

            // Retrieve the product
            Product product = db.Products.SingleOrDefault(p => p.Id == Id);

            // Handle the case where the product is not found
            if (product == null)
            {
                return NotFound(); // or return an error view/message
            }

            // Ensure 'Categories' is not null and contains data
            ViewBag.cats = db.Categories.ToList();
            if (ViewBag.cats == null)
            {
                // Handle the error, perhaps log it or return an error view
                return StatusCode(StatusCodes.Status500InternalServerError, "Categories could not be retrieved.");
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Update(int id, AddProductVM pt)
        {
            Product product = db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null) { return NotFound(); }
            product.Name = pt.Name;
            product.Description =pt.Description;
            product.Price = pt.Price;
            product.CategoryId = pt.CategoryId;
            db.SaveChanges();
            TempData["SuccessMessage"] = "Product updated  successfully!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Product product = db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null) { return NotFound(); }
            db.Products.Remove(product);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Product deleted  successfully!";
            return RedirectToAction("Index");

        }

    }
}
