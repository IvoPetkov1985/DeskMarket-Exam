using DeskMarket.Contracts;
using DeskMarket.Data;
using DeskMarket.Data.DataModels;
using DeskMarket.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task AddProductToCartAsync(string userId, int id)
        {
            if (await context.ProductsClients.AnyAsync(pc => pc.ProductId == id && pc.ClientId == userId) == false)
            {
                ProductClient entry = new()
                {
                    ClientId = userId,
                    ProductId = id
                };

                await context.ProductsClients.AddAsync(entry);

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductCartViewModel>> CreateCartModel(string userId)
        {
            IEnumerable<ProductCartViewModel> cart = await context.ProductsClients
                .AsNoTracking()
                .Where(pc => pc.ClientId == userId && pc.Product.IsDeleted == false)
                .Select(pc => new ProductCartViewModel()
                {
                    Id = pc.ProductId,
                    ProductName = pc.Product.ProductName,
                    Price = pc.Product.Price,
                    ImageUrl = pc.Product.ImageUrl
                })
                .ToListAsync();

            return cart;
        }

        public async Task<ProductDeleteViewModel> CreateDeleteModelAsync(string userId, int id)
        {
            ProductDeleteViewModel deleteModel = await context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDeleteViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    SellerId = p.SellerId,
                    Seller = p.Seller.UserName
                })
                .FirstAsync();

            return deleteModel;
        }

        public async Task<ProductEditFormModel> CreateEditModelAsync(int id)
        {
            ProductEditFormModel model = await context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductEditFormModel()
                {
                    ImageUrl = p.ImageUrl,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    AddedOn = p.AddedOn.ToString(DateTimeFormat),
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    SellerId = p.SellerId
                })
                .FirstAsync();

            return model;
        }

        public async Task CreateNewProductAsync(ProductFormModel model, string userId)
        {
            Product product = new()
            {
                ProductName = model.ProductName,
                Price = model.Price,
                Description = model.Description,
                AddedOn = DateTime.ParseExact(model.AddedOn, DateTimeFormat, CultureInfo.InvariantCulture),
                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl,
                SellerId = userId
            };

            await context.Products.AddAsync(product);

            await context.SaveChangesAsync();
        }

        public async Task DeleteSoftProductAsync(ProductDeleteViewModel model)
        {
            Product product = await context.Products
                .FirstAsync(p => p.Id == model.Id);

            product.IsDeleted = true;

            await context.SaveChangesAsync();
        }

        public async Task EditProductAsync(int id, ProductEditFormModel model)
        {
            Product productToEdit = await context.Products
                .FirstAsync(p => p.Id == id);

            productToEdit.ProductName = model.ProductName;
            productToEdit.ImageUrl = model.ImageUrl;
            productToEdit.Price = model.Price;
            productToEdit.AddedOn = DateTime.ParseExact(model.AddedOn, DateTimeFormat, CultureInfo.InvariantCulture);
            productToEdit.CategoryId = model.CategoryId;
            productToEdit.Description = model.Description;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string userId)
        {
            IEnumerable<ProductIndexViewModel> allProducts = await context.Products
                .AsNoTracking()
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductIndexViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    IsSeller = p.SellerId == userId,
                    HasBought = p.ProductsClients.Any(pc => pc.ProductId == p.Id && pc.ClientId == userId)
                })
                .ToListAsync();

            return allProducts;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            IEnumerable<CategoryViewModel> categories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }

        public async Task<ProductDetailsViewModel> GetDetailsInfoAsync(int id, string userId)
        {
            return await context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    AddedOn = p.AddedOn.ToString(DateTimeFormat),
                    CategoryName = p.Category.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Seller = p.Seller.UserName,
                    HasBought = p.ProductsClients.Any(p => p.ClientId == userId)
                })
                .FirstAsync();
        }

        public async Task<bool> IsExistingAsync(int id)
        {
            Product? product = await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsUserAuthorizedAsync(string userId, int id)
        {
            Product product = await context.Products
                .AsNoTracking()
                .FirstAsync(p => p.Id == id);

            if (product.SellerId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task RemoveProductFromCartAsync(string userId, int id)
        {
            if (await context.ProductsClients.AnyAsync(pc => pc.ProductId == id && pc.ClientId == userId))
            {
                ProductClient entry = await context.ProductsClients
                    .FirstAsync(pc => pc.ProductId == id && pc.ClientId == userId);

                context.ProductsClients.Remove(entry);

                await context.SaveChangesAsync();
            }
        }
    }
}
