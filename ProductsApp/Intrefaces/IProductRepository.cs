using ProductsApp.Models;

namespace ProductsApp.Intrefaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();
        Product Get(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
