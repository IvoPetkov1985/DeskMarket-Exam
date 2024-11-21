using DeskMarket.Contracts;
using DeskMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService service;

        public ProductController(IProductService _service)
        {
            service = _service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();

            IEnumerable<ProductIndexViewModel> allProducts = await service.GetAllProductsAsync(userId);

            return View(allProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IEnumerable<CategoryViewModel> categories = await service.GetCategoriesAsync();

            ProductFormModel model = new()
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductFormModel model)
        {
            DateTime addedOn;

            if (!DateTime.TryParseExact(model.AddedOn, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out addedOn))
            {
                ModelState.AddModelError(nameof(model.AddedOn), DateTimeErrorMsg);
            }

            IEnumerable<CategoryViewModel> categories = await service.GetCategoriesAsync();

            if (!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), CategoryErrorMsg);
            }

            if (!ModelState.IsValid)
            {
                model.Categories = categories;

                return View(model);
            }

            string userId = GetUserId();

            await service.CreateNewProductAsync(model, userId);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (await service.IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            ProductDetailsViewModel model = await service.GetDetailsInfoAsync(id, userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await service.IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            ProductEditFormModel model = await service.CreateEditModelAsync(id);

            IEnumerable<CategoryViewModel> categories = await service.GetCategoriesAsync();

            model.Categories = categories;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditFormModel model, int id)
        {
            if (await service.IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            IEnumerable<CategoryViewModel> categories = await service.GetCategoriesAsync();

            if (!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), CategoryErrorMsg);
            }

            DateTime addedon;

            if (!DateTime.TryParseExact(model.AddedOn, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out addedon))
            {
                ModelState.AddModelError(nameof(model.AddedOn), DateTimeErrorMsg);
            }

            if (!ModelState.IsValid)
            {
                model.Categories = categories;

                return View(model);
            }

            await service.EditProductAsync(id, model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string userId = GetUserId();

            IEnumerable<ProductCartViewModel> model = await service.CreateCartModel(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            if (await service.IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.AddProductToCartAsync(userId, id);

            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            if (await service.IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.RemoveProductFromCartAsync(userId, id);

            return RedirectToAction(nameof(Cart));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await service.IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            ProductDeleteViewModel model = await service.CreateDeleteModelAsync(userId, id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDeleteViewModel model, int id)
        {
            if (await service.IsExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            await service.DeleteSoftProductAsync(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
