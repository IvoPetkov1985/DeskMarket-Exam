using DeskMarket.Contracts;
using DeskMarket.Data;
using DeskMarket.Data.Models;
using DeskMarket.Models;
using Microsoft.EntityFrameworkCore;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Services
{
    public class ProductService(ApplicationDbContext _context) : IProductService
    {
        private readonly ApplicationDbContext context = _context;

        public async Task AddNewProductAsync(ProductFormModel model, string userId)
        {
            Product product = new()
            {
                ProductName = model.ProductName,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                AddedOn = DateTime.Parse(model.AddedOn),
                SellerId = userId,
                CategoryId = model.CategoryId
            };

            await context.Products.AddAsync(product);

            await context.SaveChangesAsync();
        }

        public async Task AddProductToCartAsync(string userId, int id)
        {
            ProductClient cartEntry = new()
            {
                ClientId = userId,
                ProductId = id
            };

            if (await context.ProductsClients.ContainsAsync(cartEntry) == false)
            {
                await context.ProductsClients.AddAsync(cartEntry);

                await context.SaveChangesAsync();
            }
        }

        public async Task<ProductDeleteViewModel> CreateDeleteModelAsync(int id)
        {
            ProductDeleteViewModel deleteModel = await context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDeleteViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Seller = p.Seller.UserName,
                    SellerId = p.SellerId
                })
                .SingleAsync();

            return deleteModel;
        }

        public async Task<ProductDetailsViewModel> CreateDetailsModelAsync(string userId, int id)
        {
            ProductDetailsViewModel productDetails = await context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price * BgnToEurRate,
                    AddedOn = p.AddedOn.ToString(DateFormat),
                    CategoryName = p.Category.Name,
                    Seller = p.Seller.UserName,
                    HasBought = p.ProductsClients.Any(pc => pc.ClientId == userId)
                })
                .SingleAsync();

            return productDetails;
        }

        public async Task<ProductEditFormModel> CreateEditModelAsync(string userId, int id)
        {
            ProductEditFormModel editModel = await context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductEditFormModel()
                {
                    ProductName = p.ProductName,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    AddedOn = p.AddedOn.ToString(DateFormat),
                    CategoryId = p.CategoryId,
                    SellerId = p.SellerId
                })
                .SingleAsync();

            return editModel;
        }

        public async Task EditProductAsync(ProductEditFormModel model, int id)
        {
            Product productToEdit = await context.Products
                .SingleAsync(p => p.Id == id);

            productToEdit.ProductName = model.ProductName;
            productToEdit.Description = model.Description;
            productToEdit.ImageUrl = model.ImageUrl;
            productToEdit.Price = model.Price;
            productToEdit.AddedOn = DateTime.Parse(model.AddedOn);
            productToEdit.CategoryId = model.CategoryId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
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

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string userId)
        {
            IEnumerable<ProductIndexViewModel> products = await context.Products
                .AsNoTracking()
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductIndexViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    IsSeller = p.SellerId == userId,
                    HasBought = p.ProductsClients.Any(pc => pc.ClientId == userId)
                })
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<ProductCartViewModel>> GetAllProductsInCartAsync(string userId)
        {
            IEnumerable<ProductCartViewModel> purchasedProducts = await context.ProductsClients
                .AsNoTracking()
                .Where(pc => pc.ClientId == userId)
                .Select(pc => new ProductCartViewModel()
                {
                    Id = pc.Product.Id,
                    ProductName = pc.Product.ProductName,
                    ImageUrl = pc.Product.ImageUrl,
                    Price = pc.Product.Price
                })
                .ToListAsync();

            return purchasedProducts;
        }

        public async Task<bool> IsProductExistingAsync(int id)
        {
            Product? product = await context.Products
                .AsNoTracking()
                .Where(p => p.Id == id && p.IsDeleted == false)
                .SingleOrDefaultAsync();

            return product != null;
        }

        public async Task<bool> IsUserAuthorizedAsync(string userId, int id)
        {
            Product productToCheck = await context.Products
                .AsNoTracking()
                .SingleAsync(p => p.Id == id);

            return productToCheck.SellerId == userId;
        }

        public async Task RemoveProductFromCartAsync(string userId, int id)
        {
            ProductClient cartEntry = new()
            {
                ClientId = userId,
                ProductId = id
            };

            if (await context.ProductsClients.ContainsAsync(cartEntry))
            {
                context.ProductsClients.Remove(cartEntry);

                await context.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteProductAsync(int id)
        {
            Product productToDelete = await context.Products
                .SingleAsync(p => p.Id == id);

            productToDelete.IsDeleted = true;

            await context.SaveChangesAsync();
        }
    }
}
