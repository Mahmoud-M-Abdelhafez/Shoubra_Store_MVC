using Microsoft.EntityFrameworkCore;
using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContextDB db;
        public ProductRepository(StoreContextDB context)
        {
            db = context;
        }
        public ProductsWithImagesVM GetAll()
        {
            ProductsWithImagesVM productsWithImagesVM = new ProductsWithImagesVM();
            productsWithImagesVM.products = db.Products.ToList();
            productsWithImagesVM.images = db.ProductImages.ToList();
            return productsWithImagesVM;
        }

        public ProductDetailsVM GetById(int id)
        {
            ProductDetailsVM ProductDetailsVM = new ProductDetailsVM();
            ProductDetailsVM.product = db.Products.SingleOrDefault(p => p.Id == id);
            ProductDetailsVM.productImages= db.ProductImages.Where(s=>s.ProductId == id).ToList();
            ProductDetailsVM.category = db.Categories.Where(c=>c.Id== ProductDetailsVM.product.CategoryId).Single();
            return (ProductDetailsVM);
        }
    }
}
