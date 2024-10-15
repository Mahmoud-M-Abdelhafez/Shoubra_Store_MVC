using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Repository
{
    public interface ICartRepository
    {
        CartProductsVM GetAll(string id);
        CartDetailsVM GetById(int id);
        void Insert(AddCartVM item);
        void Edit(int id, AddCartVM item);
        string Delete(int id);
    }
}
