using AutoMapper;
using Menu.Site.Models;
using Menu.Site.Repository;
using Menu.Site.Services;
using Menu.Site.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace Menu.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Categories";
            return View(await _categoryRepository.GetAllSortedAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            ViewData["ActivePage"] = "Categories";
            PersianCalendar jc = new PersianCalendar();
            var category = await _categoryRepository.GetAsync(id);
            DateTime date = category.DateCreated.Value;
            ViewBag.DateTime = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));
            return View(category);
        }
        public IActionResult Create()
        {
            ViewData["ActivePage"] = "Categories";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryVM vm)
        {
            ViewData["ActivePage"] = "Categories";
            if (!ModelState.IsValid)
                return View(vm);

            var category = _mapper.Map<Category>(vm);

            category.Priority = await _categoryRepository.PriorityMaxAsync();

            category.CategoryImage = UploadedFile.CreateNameFileImage(vm.Image);
            var response = await _categoryRepository.AddAsync(category);
            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                ModelState.AddModelError("CategoryName", response.ValidationErrors);
                return View(vm);
            }
            UploadedFile.CreateFileImage(vm.Image, category.CategoryImage, _webHostEnvironment);


            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ActivePage"] = "Categories";
            if (!await _categoryRepository.ExistAsync(id))
                return NotFound();

            var category = await _categoryRepository.GetAsync(id);
            var updateCategory = _mapper.Map<UpdateCategoryVM>(category);
            ViewBag.CategoryImg = category.CategoryImage;

            return View(updateCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCategoryVM vm)
        {
            ViewData["ActivePage"] = "Categories";
            if (!ModelState.IsValid || id != vm.Id)
                return View(vm);

            var category = await _categoryRepository.GetAsync(id);
            string oldName = category.CategoryImage;


            _mapper.Map(vm, category);

            if (vm.Image != null)
                category.CategoryImage = UploadedFile.CreateNameFileImage(vm.Image);


            var response = await _categoryRepository.UpdateAsync(category);
            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                ModelState.AddModelError("CategoryName", response.ValidationErrors);
                return View(vm);
            }


            if (vm.Image != null)
            {
                UploadedFile.DeleteFileImage(oldName, _webHostEnvironment);
                UploadedFile.CreateFileImage(vm.Image, category.CategoryImage, _webHostEnvironment);
            }

            await _categoryRepository.IsActiveProductsAsync(category);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["ActivePage"] = "Categories";
            if (!await _categoryRepository.ExistAsync(id))
                return NotFound();

            var category = await _categoryRepository.GetAsync(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Category model)
        {
            ViewData["ActivePage"] = "Categories";
            if (id != model.Id)
                return NotFound();

            await _categoryRepository.DeleteAsync(id);
            UploadedFile.DeleteFileImage(model.CategoryImage, _webHostEnvironment);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePriorities(List<int> ids)
        {
            var response = await _categoryRepository.UpdatePrioritiesAsync(ids);
            return Json(response.Data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            var categories = await _categoryRepository.GetAllCustomAsync(ids);
            var response = await _categoryRepository.DeleteAsync(categories.Data);
            return Json(response.Data);
        }



    }
}
