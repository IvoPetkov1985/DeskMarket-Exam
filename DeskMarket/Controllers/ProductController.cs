using DeskMarket.Contracts;
using DeskMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Controllers
{
    public class ProductController(IProductService _service) : BaseController
    {
        private readonly IProductService service = _service;

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();

            IEnumerable<ProductIndexViewModel> model = await service.GetAllProductsAsync(userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            ProductFormModel model = new()
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductFormModel model)
        {
            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            if (categories.Any(c => c.Id == model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), CategoryErrorMessage);
            }

            if (DateTime.TryParseExact(model.AddedOn, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate) == false)
            {
                ModelState.AddModelError(nameof(model.AddedOn), DateInvalidErrorMessage);
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = categories;

                return View(model);
            }

            string userId = GetUserId();

            await service.AddNewProductAsync(model, userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (await service.IsProductExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            ProductDetailsViewModel model = await service.CreateDetailsModelAsync(userId, id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await service.IsProductExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            ProductEditFormModel model = await service.CreateEditModelAsync(userId, id);

            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            model.Categories = categories;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditFormModel model, int id)
        {
            if (await service.IsProductExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            if (categories.Any(c => c.Id == model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), CategoryErrorMessage);
            }

            if (DateTime.TryParseExact(model.AddedOn, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate) == false)
            {
                ModelState.AddModelError(nameof(model.AddedOn), DateInvalidErrorMessage);
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = categories;

                return View(model);
            }

            await service.EditProductAsync(model, id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string userId = GetUserId();

            IEnumerable<ProductCartViewModel> model = await service.GetAllProductsInCartAsync(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            if (await service.IsProductExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.AddProductToCartAsync(userId, id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            if (await service.IsProductExistingAsync(id) == false)
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
            if (await service.IsProductExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            ProductDeleteViewModel model = await service.CreateDeleteModelAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDeleteViewModel model, int id)
        {
            if (await service.IsProductExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            await service.SoftDeleteProductAsync(model.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
