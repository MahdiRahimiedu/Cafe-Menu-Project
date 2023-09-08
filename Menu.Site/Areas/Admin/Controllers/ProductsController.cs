using AutoMapper;
using Menu.Site.Models;
using Menu.Site.Repository;
using Menu.Site.ViewModels;
using Menu.Site.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Menu.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductRepository productRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment , ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName");
            ViewData["ActivePage"] = "Products";
            return View(await _productRepository.GetAllWithDetailSortedAsync());
        }
        
        public async Task<IActionResult> Details(int id)
        {
            PersianCalendar jc = new PersianCalendar();
            var product = await _productRepository.GetWithDetailAsync(id);
            DateTime date = product.DateCreated.Value;
            ViewBag.DateTime = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));
            ViewData["ActivePage"] = "Products";
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName");
            ViewData["ActivePage"] = "Products";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVM vm)
        {
            ViewData["ActivePage"] = "Products";
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName",vm.CategoryId);
                return View(vm);
            }
                

            var product = _mapper.Map<Product>(vm);

            product.Priority = await _productRepository.PriorityMax(product.CategoryId);

            var response = await _productRepository.AddAsync(product);
            if (!response.Success)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName", vm.CategoryId);
                ModelState.AddModelError(string.Empty, response.Message);
                ModelState.AddModelError("CategoryName", response.ValidationErrors);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _productRepository.ExistAsync(id))
                return NotFound();

            var product = await _productRepository.GetAsync(id);
            var updateproduct = _mapper.Map<UpdateProductVM>(product);
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName", product.CategoryId);
            ViewData["ActivePage"] = "Products";
            return View(updateproduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductVM vm)
        {
            ViewData["ActivePage"] = "Products";
            if (!ModelState.IsValid || id != vm.Id)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName", vm.CategoryId);
                return View(vm);
            }
            if(vm.IsActive && !await _productRepository.GetIsActiveCategoryAsync(vm.CategoryId))
            {
                ModelState.AddModelError("IsActive", "The given value is wrong");
                ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName", vm.CategoryId);
                return View(vm);
            }


            var product = await _productRepository.GetAsync(id);

            _mapper.Map(vm, product);

            var response = await _productRepository.UpdateAsync(product);
            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                ModelState.AddModelError("CategoryName", response.ValidationErrors);
                ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "CategoryName", vm.CategoryId);
                return View(vm);
            }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!await _productRepository.ExistAsync(id))
                return NotFound();

            var product = await _productRepository.GetWithDetailAsync(id);
            ViewData["ActivePage"] = "Products";
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Product model)
        {
            if (id != model.Id)
                return NotFound();

            await _productRepository.DeleteAsync(id);

            ViewData["ActivePage"] = "Products";
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int id)
        {
            return Json(await _productRepository.GetByCategoryId(id));
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePriorities(List<int> ids)
        {
            var response = await _productRepository.UpdatePrioritiesAsync(ids);
            return Json(response.Data);
        }

        [Route("~/Admin/Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            ViewData["ActivePage"] = "Dashboard";
            var category = await _categoryRepository.GetAllAsync();
            var product = await _productRepository.GetAllAsync();
            Dashboard vm = new Dashboard(product, category);

            return View(vm);
        }
    }
}
