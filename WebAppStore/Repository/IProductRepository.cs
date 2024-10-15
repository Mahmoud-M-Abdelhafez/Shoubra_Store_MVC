using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Repository
{
    public interface IProductRepository
    {
        ProductsWithImagesVM GetAll();
        ProductDetailsVM GetById(int id);

    }
}
