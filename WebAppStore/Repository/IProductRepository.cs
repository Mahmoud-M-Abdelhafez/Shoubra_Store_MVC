using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Repository
{
    public interface IProductRepository
    {
        ProductsWithImagesVM GetAll();
        ProductDetailsVM GetById(int id);
        void Insert(AddProductVM item);
        void Edit(int id, AddProductVM item);
        void Delete(int id);

    }
}
