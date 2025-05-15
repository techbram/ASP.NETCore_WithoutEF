using ASP.NETCore_WithoutEF.App.Models;
using ASP.NETCore_WithoutEF.App.DataAccess.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using ASP.NETCore_WithoutEF.App.Utility;
using Microsoft.AspNetCore.Authorization;
using Stripe.FinancialConnections;

namespace ASP.NETCore_WithoutEF.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {

        private readonly IProductRepository<Product> _productRepo;
        private readonly ICategoryRepository<Category> _categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository<Product> productRepo,
            ICategoryRepository<Category> categoryRepo,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

      
        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            List<Category> categoryList = await _categoryRepo.GetCategoryList();

            return StatusCode(StatusCodes.Status200OK, categoryList);  
        }
               
        [HttpGet]
        public async Task<IActionResult> GetListProducts()
        {
            List<Product> productList = await _productRepo.GetProductList();

            return StatusCode(StatusCodes.Status200OK, productList);     
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct([FromForm] Product model, IFormFile? ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Handle image file
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string uploadPath = Path.Combine(wwwRootPath, @"images\product");                       

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        string filePath = Path.Combine(uploadPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        // Save the relative path to DB
                        //model.ImageUrl = @"/images/product/" + fileName;
                        model.ImageUrl = fileName;

                        return Json(new { model.ImageUrl });
                    }
                    bool _result = await _productRepo.Save(model);

                    if (_result)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                // Log error or return detailed message for debugging (temporarily)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

       
        [HttpPut]
        public async Task<IActionResult> EditProduct([FromForm] Product model, IFormFile? ImageFile)
        {
            try
            {
                // Handle image file
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string uploadPath = Path.Combine(wwwRootPath, @"images\product");


                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    if (!string.IsNullOrEmpty(model.OldImageUrl))
                    {
                        // Delete the old image
                        String[] breakApart = model.OldImageUrl.Split('/');
                        string imageDeletePath = uploadPath + "\\" + breakApart.Last();
                    
                        if (System.IO.File.Exists(imageDeletePath))
                        {
                            System.IO.File.Delete(imageDeletePath);
                        }
                    }
                    //String[] breakApart = model.OldImageUrl.Split('/');                    
                    //string imageDeletePath = uploadPath + "\\"+ breakApart.Last();
                    //System.IO.File.Delete(imageDeletePath);

                    // Update relative path with new image name
                    model.ImageUrl = @"/images/product/" + fileName;                   
                }
                else
                {
                    model.ImageUrl = model.OldImageUrl;
                }
               
                bool _result = await _productRepo.Edit(model);

                if (_result)
                {
                    return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
                }
            }
            catch (Exception ex)
            {
                // Log error or return detailed message for debugging (temporarily)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        // Action to view the product details
        public async Task<IActionResult> Detail(int id)
        {
            Product product = await _productRepo.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Method httpdelete to delete Product
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int idProduct)
        {

            bool _result = await _productRepo.Delete(idProduct);

            if (_result)
            {
                return StatusCode(StatusCodes.Status200OK, new { value = _result, msg = "ok" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { value = _result, msg = "error" });
            }
        }
    }
}
