using DeskMarket.Models;

namespace DeskMarket.Contracts
{
    public interface IProductService
    {
        Task AddProductToCartAsync(string userId, int id);

        Task<IEnumerable<ProductCartViewModel>> CreateCartModel(string userId);

        Task<ProductDeleteViewModel> CreateDeleteModelAsync(string userId, int id);

        Task<ProductEditFormModel> CreateEditModelAsync(int id);

        Task CreateNewProductAsync(ProductFormModel model, string userId);

        Task DeleteSoftProductAsync(ProductDeleteViewModel model);

        Task EditProductAsync(int id, ProductEditFormModel model);

        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string userId);

        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

        Task<ProductDetailsViewModel> GetDetailsInfoAsync(int id, string userId);

        Task<bool> IsExistingAsync(int id);

        Task<bool> IsUserAuthorizedAsync(string userId, int id);

        Task RemoveProductFromCartAsync(string userId, int id);
    }
}
