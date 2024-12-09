using DeskMarket.Models;

namespace DeskMarket.Contracts
{
    public interface IProductService
    {
        Task AddNewProductAsync(ProductFormModel model, string userId);

        Task AddProductToCartAsync(string userId, int id);

        Task<ProductDeleteViewModel> CreateDeleteModelAsync(int id);

        Task<ProductDetailsViewModel> CreateDetailsModelAsync(string userId, int id);

        Task<ProductEditFormModel> CreateEditModelAsync(string userId, int id);

        Task EditProductAsync(ProductEditFormModel model, int id);

        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string userId);

        Task<IEnumerable<ProductCartViewModel>> GetAllProductsInCartAsync(string userId);

        Task<bool> IsProductExistingAsync(int id);

        Task<bool> IsUserAuthorizedAsync(string userId, int id);

        Task RemoveProductFromCartAsync(string userId, int id);

        Task SoftDeleteProductAsync(int id);
    }
}
